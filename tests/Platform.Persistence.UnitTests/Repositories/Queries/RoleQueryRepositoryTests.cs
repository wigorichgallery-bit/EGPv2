// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Repositories/Queries/RoleQueryRepositoryTests.cs
// ===========================================

using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Queries;

namespace Platform.Persistence.UnitTests.Repositories.Queries;

public sealed class RoleQueryRepositoryTests
{
    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    [Fact]
    public void Constructor_WhenDbContextIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () =>
            new RoleQueryRepository(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // LIST
    // ============================================================

    [Fact]
    public async Task ListAsync_WhenRolesExist_ShouldReturnAllRoleDtos()
    {
        // Arrange
        var role1 =
            CreateRole(
                name: "Administrator");

        var role2 =
            CreateRole(
                name: "Auditor");

        await using var context =
            CreateContext();

        context.Roles.AddRange(
            role1,
            role2);

        await context.SaveChangesAsync();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .HaveCount(2);

        result
            .Should()
            .AllBeOfType<RoleDto>();

        result
            .Select(x => x.RoleId)
            .Should()
            .Contain(role1.Id)
            .And
            .Contain(role2.Id);

        result
            .Select(x => x.Name)
            .Should()
            .Contain(
                "Administrator",
                "Auditor");
    }

    [Fact]
    public async Task ListAsync_ShouldMapRolePropertiesToDto()
    {
        // Arrange
        var role =
            CreateRole(
                name: "Administrator",
                isSystemRole: true);

        await using var context =
            CreateContext();

        context.Roles.Add(role);

        await context.SaveChangesAsync();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        var dto =
            result
                .Should()
                .ContainSingle()
                .Which;

        dto.RoleId
            .Should()
            .Be(role.Id);

        dto.Name
            .Should()
            .Be(role.Name);

        dto.IsSystemRole
            .Should()
            .BeTrue();

        dto.ScopeType
            .Should()
            .Be(role.Scope.Value);

        dto.IsActive
            .Should()
            .BeTrue();

        dto.PermissionIds
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task ListAsync_WhenNoRolesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        await using var context =
            CreateContext();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task ListAsync_ShouldReturnRolesAsNoTracking()
    {
        // Arrange
        var role =
            CreateRole(
                name: "Auditor");

        await using var context =
            CreateContext();

        context.Roles.Add(role);

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .ContainSingle();

        context.ChangeTracker
            .Entries<Role>()
            .Should()
            .BeEmpty();
    }

    // ============================================================
    // FIND BY IDS
    // ============================================================

    [Fact]
    public async Task FindByIdsAsync_WhenMatchingRolesExist_ShouldReturnMatchingDtos()
    {
        // Arrange
        var role1 =
            CreateRole(
                name: "Administrator");

        var role2 =
            CreateRole(
                name: "Auditor");

        var role3 =
            CreateRole(
                name: "Operator");

        await using var context =
            CreateContext();

        context.Roles.AddRange(
            role1,
            role2,
            role3);

        await context.SaveChangesAsync();

        var repository =
            new RoleQueryRepository(context);

        var roleIds =
            new[]
            {
                role1.Id,
                role3.Id
            };

        // Act
        var result =
            await repository.FindByIdsAsync(
                roleIds);

        // Assert
        result
            .Should()
            .HaveCount(2);

        result
            .Select(x => x.RoleId)
            .Should()
            .Contain(role1.Id)
            .And
            .Contain(role3.Id);

        result
            .Select(x => x.RoleId)
            .Should()
            .NotContain(role2.Id);
    }

    [Fact]
    public async Task FindByIdsAsync_WhenNoMatchingRolesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        await using var context =
            CreateContext();

        var repository =
            new RoleQueryRepository(context);

        var roleIds =
            new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

        // Act
        var result =
            await repository.FindByIdsAsync(
                roleIds);

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task FindByIdsAsync_WhenIdsCollectionIsEmpty_ShouldReturnEmptyCollection()
    {
        // Arrange
        await using var context =
            CreateContext();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.FindByIdsAsync(
                Array.Empty<Guid>());

        // Assert
        result
            .Should()
            .BeEmpty();

        context.ChangeTracker
            .Entries<Role>()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task FindByIdsAsync_ShouldReturnOnlyRequestedRoles()
    {
        // Arrange
        var role1 =
            CreateRole(
                name: "Administrator");

        var role2 =
            CreateRole(
                name: "Auditor");

        var role3 =
            CreateRole(
                name: "Operator");

        await using var context =
            CreateContext();

        context.Roles.AddRange(
            role1,
            role2,
            role3);

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.FindByIdsAsync(
                new[]
                {
                    role2.Id
                });

        // Assert
        result
            .Should()
            .ContainSingle();

        result[0].RoleId
            .Should()
            .Be(role2.Id);

        result[0].Name
            .Should()
            .Be("Auditor");
    }

    [Fact]
    public async Task FindByIdsAsync_ShouldReturnDtosAsNoTracking()
    {
        // Arrange
        var role =
            CreateRole(
                name: "Auditor");

        await using var context =
            CreateContext();

        context.Roles.Add(role);

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var result =
            await repository.FindByIdsAsync(
                new[]
                {
                    role.Id
                });

        // Assert
        result
            .Should()
            .ContainSingle();

        context.ChangeTracker
            .Entries<Role>()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task FindByIdsAsync_WhenRoleIdsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context =
            CreateContext();

        var repository =
            new RoleQueryRepository(context);

        // Act
        var action = () =>
            repository.FindByIdsAsync(
                null!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    // ============================================================
    // CANCELLATION
    // ============================================================

    [Fact]
    public async Task ListAsync_WhenCancellationIsRequested_ShouldObserveCancellationToken()
    {
        // Arrange
        await using var context =
            CreateContext();

        context.Roles.Add(
            CreateRole());

        await context.SaveChangesAsync();

        var repository =
            new RoleQueryRepository(context);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        // Act
        var action = () =>
            repository.ListAsync(
                cancellationTokenSource.Token);

        // Assert
        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task FindByIdsAsync_WhenCancellationIsRequested_ShouldObserveCancellationToken()
    {
        // Arrange
        var role =
            CreateRole();

        await using var context =
            CreateContext();

        context.Roles.Add(role);

        await context.SaveChangesAsync();

        var repository =
            new RoleQueryRepository(context);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        // Act
        var action = () =>
            repository.FindByIdsAsync(
                new[]
                {
                    role.Id
                },
                cancellationTokenSource.Token);

        // Assert
        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
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
        string name = "Test Role",
        bool isSystemRole = false)
    {
        return new Role(
            Guid.NewGuid(),
            name,
            isSystemRole,
            RoleScope.Global,
            DateTime.UtcNow);
    }
}