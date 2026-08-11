// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Repositories/Commands/RoleRepositoryTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Commands;

namespace Platform.Persistence.UnitTests.Repositories.Commands;

public sealed class RoleRepositoryTests
{
    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    [Fact]
    public void Constructor_WhenDbContextIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () =>
            new RoleRepository(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // GET BY ID
    // ============================================================

    [Fact]
    public async Task GetByIdAsync_WhenRoleExists_ShouldReturnRole()
    {
        // Arrange
        var role = CreateRole();

        await using var context = CreateContext();
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetByIdAsync(
                role.Id);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(role.Id);

        result.Name
            .Should()
            .Be(role.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRoleDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetByIdAsync(
                Guid.NewGuid());

        // Assert
        result
            .Should()
            .BeNull();
    }

    // ============================================================
    // GET BY NAME
    // ============================================================

    [Fact]
    public async Task GetByNameAsync_WhenRoleExists_ShouldReturnRole()
    {
        // Arrange
        var role = CreateRole(
            name: "Administrator");

        await using var context = CreateContext();
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetByNameAsync(
                "Administrator");

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(role.Id);

        result.Name
            .Should()
            .Be("Administrator");
    }

    [Fact]
    public async Task GetByNameAsync_WhenRoleDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetByNameAsync(
                "NonExistingRole");

        // Assert
        result
            .Should()
            .BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task GetByNameAsync_WhenRoleNameIsInvalid_ShouldThrowArgumentException(
        string? roleName)
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var action = () =>
            repository.GetByNameAsync(
                roleName!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    // ============================================================
    // EXISTS BY NAME
    // ============================================================

    [Fact]
    public async Task ExistsByNameAsync_WhenRoleExists_ShouldReturnTrue()
    {
        // Arrange
        var role = CreateRole(
            name: "Administrator");

        await using var context = CreateContext();
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.ExistsByNameAsync(
                "Administrator");

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByNameAsync_WhenRoleDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.ExistsByNameAsync(
                "NonExistingRole");

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task ExistsByNameAsync_WhenRoleNameIsInvalid_ShouldThrowArgumentException(
        string? roleName)
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var action = () =>
            repository.ExistsByNameAsync(
                roleName!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    // ============================================================
    // GET ALL
    // ============================================================

    [Fact]
    public async Task GetAllAsync_WhenRolesExist_ShouldReturnAllRoles()
    {
        // Arrange
        var role1 =
            CreateRole(
                name: "Administrator");

        var role2 =
            CreateRole(
                name: "Auditor");

        await using var context = CreateContext();

        context.Roles.AddRange(
            role1,
            role2);

        await context.SaveChangesAsync();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetAllAsync();

        // Assert
        result
            .Should()
            .HaveCount(2);

        result
            .Select(x => x.Name)
            .Should()
            .Contain(
                "Administrator",
                "Auditor");
    }

    [Fact]
    public async Task GetAllAsync_WhenNoRolesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.GetAllAsync();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    // ============================================================
    // ADD
    // ============================================================

    [Fact]
    public async Task AddAsync_ShouldAddRoleToPersistenceContext()
    {
        // Arrange
        var role = CreateRole();

        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        await repository.AddAsync(role);

        // Assert
        context.Entry(role)
            .State
            .Should()
            .Be(EntityState.Added);

        await context.SaveChangesAsync();

        var persisted =
            await context.Roles
                .SingleAsync(
                    x => x.Id == role.Id);

        persisted.Name
            .Should()
            .Be(role.Name);
    }

    [Fact]
    public async Task AddAsync_WhenRoleIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var action = () =>
            repository.AddAsync(null!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    // ============================================================
    // UPDATE
    // ============================================================

    [Fact]
    public void Update_ShouldMarkRoleAsModified()
    {
        // Arrange
        var role = CreateRole();

        using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        repository.Update(role);

        // Assert
        context.Entry(role)
            .State
            .Should()
            .Be(EntityState.Modified);
    }

    [Fact]
    public void Update_WhenRoleIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var action = () =>
            repository.Update(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // REMOVE
    // ============================================================

    [Fact]
    public void Remove_ShouldMarkRoleAsDeleted()
    {
        // Arrange
        var role = CreateRole();

        using var context = CreateContext();

        context.Roles.Attach(role);

        var repository =
            new RoleRepository(context);

        // Act
        repository.Remove(role);

        // Assert
        context.Entry(role)
            .State
            .Should()
            .Be(EntityState.Deleted);
    }

    [Fact]
    public void Remove_WhenRoleIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var action = () =>
            repository.Remove(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // EXISTS BY ID
    // ============================================================

    [Fact]
    public async Task ExistsAsync_WhenRoleExists_ShouldReturnTrue()
    {
        // Arrange
        var role = CreateRole();

        await using var context = CreateContext();

        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.ExistsAsync(
                role.Id);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenRoleDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new RoleRepository(context);

        // Act
        var result =
            await repository.ExistsAsync(
                Guid.NewGuid());

        // Assert
        result
            .Should()
            .BeFalse();
    }

    // ============================================================
    // TEST HELPERS
    // ============================================================

    private static GovernanceDbContext CreateContext()
    {
        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        return new GovernanceDbContext(
            options);
    }

    private static Role CreateRole(
        string name = "Test Role")
    {
        return new Role(
            Guid.NewGuid(),
            name,
            false,
            RoleScope.Global,
            DateTime.UtcNow);
    }
}