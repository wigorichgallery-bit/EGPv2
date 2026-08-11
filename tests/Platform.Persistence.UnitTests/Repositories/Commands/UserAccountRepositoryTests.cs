// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Repositories/Commands/UserAccountRepositoryTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Commands;

namespace Platform.Persistence.UnitTests.Repositories.Commands;

public sealed class UserAccountRepositoryTests
{
    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    [Fact]
    public void Constructor_WhenDbContextIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () =>
            new UserAccountRepository(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // GET BY ID
    // ============================================================

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user = CreateUser();

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.GetByIdAsync(user.Id);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(user.Id);

        result.Username
            .Should()
            .Be(user.Username);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

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
    // GET BY USERNAME
    // ============================================================

    [Fact]
    public async Task GetByUsernameAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user =
            CreateUser(
                username: "john.doe");

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.GetByUsernameAsync(
                "john.doe");

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(user.Id);

        result.Username
            .Should()
            .Be("john.doe");
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.GetByUsernameAsync(
                "unknown.user");

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
    public async Task GetByUsernameAsync_WhenUsernameIsInvalid_ShouldThrowArgumentException(
        string? username)
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.GetByUsernameAsync(
                username!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    // ============================================================
    // GET BY EMAIL
    // ============================================================

    [Fact]
    public async Task GetByEmailAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var email =
            new EmailAddress(
                "john@example.com");

        var user =
            CreateUser(
                email: email);

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.GetByEmailAsync(
                email);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Id
            .Should()
            .Be(user.Id);

        result.Email.Value
            .Should()
            .Be("john@example.com");
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        var email =
            new EmailAddress(
                "missing@example.com");

        // Act
        var result =
            await repository.GetByEmailAsync(
                email);

        // Assert
        result
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WhenEmailIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.GetByEmailAsync(
                null!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    // ============================================================
    // EXISTS BY ID
    // ============================================================

    [Fact]
    public async Task ExistsByIdAsync_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        var user = CreateUser();

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByIdAsync(
                user.Id);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByIdAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByIdAsync(
                Guid.NewGuid());

        // Assert
        result
            .Should()
            .BeFalse();
    }

    // ============================================================
    // EXISTS BY USERNAME
    // ============================================================

    [Fact]
    public async Task ExistsByUsernameAsync_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        var user =
            CreateUser(
                username: "john.doe");

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByUsernameAsync(
                "john.doe");

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByUsernameAsync(
                "unknown.user");

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
    public async Task ExistsByUsernameAsync_WhenUsernameIsInvalid_ShouldThrowArgumentException(
        string? username)
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.ExistsByUsernameAsync(
                username!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    // ============================================================
    // EXISTS BY EMAIL
    // ============================================================

    [Fact]
    public async Task ExistsByEmailAsync_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        var email =
            new EmailAddress(
                "john@example.com");

        var user =
            CreateUser(
                email: email);

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByEmailAsync(
                email);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByEmailAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        var email =
            new EmailAddress(
                "missing@example.com");

        // Act
        var result =
            await repository.ExistsByEmailAsync(
                email);

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task ExistsByEmailAsync_WhenEmailIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.ExistsByEmailAsync(
                null!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    // ============================================================
    // EXISTS BY PHONE
    // ============================================================

    [Fact]
    public async Task ExistsByPhoneAsync_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        var phone =
            new PhoneNumber(
                "+628123456789");

        var user =
            CreateUser(
                phoneNumber: phone);

        await using var context = CreateContext();

        context.UserAccounts.Add(user);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.ExistsByPhoneAsync(
                phone);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByPhoneAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        var phone =
            new PhoneNumber(
                "+628987654321");

        // Act
        var result =
            await repository.ExistsByPhoneAsync(
                phone);

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task ExistsByPhoneAsync_WhenPhoneNumberIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.ExistsByPhoneAsync(
                null!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    // ============================================================
    // GET ALL
    // ============================================================

    [Fact]
    public async Task GetAllAsync_WhenUsersExist_ShouldReturnAllUsers()
    {
        // Arrange
        var user1 =
            CreateUser(
                username: "john.doe",
                email:
                    new EmailAddress(
                        "john@example.com"),
                phoneNumber:
                    new PhoneNumber(
                        "+628123456789"));

        var user2 =
            CreateUser(
                username: "jane.doe",
                email:
                    new EmailAddress(
                        "jane@example.com"),
                phoneNumber:
                    new PhoneNumber(
                        "+628987654321"));

        await using var context = CreateContext();

        context.UserAccounts.AddRange(
            user1,
            user2);

        await context.SaveChangesAsync();

        var repository =
            new UserAccountRepository(context);

        // Act
        var result =
            await repository.GetAllAsync();

        // Assert
        result
            .Should()
            .HaveCount(2);

        result
            .Select(x => x.Username)
            .Should()
            .Contain(
                "john.doe",
                "jane.doe");
    }

    [Fact]
    public async Task GetAllAsync_WhenNoUsersExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

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
    public async Task AddAsync_ShouldAddUserToPersistenceContext()
    {
        // Arrange
        var user = CreateUser();

        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        await repository.AddAsync(user);

        // Assert
        context.Entry(user)
            .State
            .Should()
            .Be(EntityState.Added);

        await context.SaveChangesAsync();

        var persisted =
            await context.UserAccounts
                .SingleAsync(
                    x => x.Id == user.Id);

        persisted.Username
            .Should()
            .Be(user.Username);
    }

    [Fact]
    public async Task AddAsync_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

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
    public void Update_ShouldMarkUserAsModified()
    {
        // Arrange
        var user = CreateUser();

        using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        repository.Update(user);

        // Assert
        context.Entry(user)
            .State
            .Should()
            .Be(EntityState.Modified);
    }

    [Fact]
    public void Update_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

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
    public void Remove_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var user = CreateUser();

        using var context = CreateContext();

        context.UserAccounts.Attach(user);

        var repository =
            new UserAccountRepository(context);

        // Act
        repository.Remove(user);

        // Assert
        context.Entry(user)
            .State
            .Should()
            .Be(EntityState.Deleted);
    }

    [Fact]
    public void Remove_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();

        var repository =
            new UserAccountRepository(context);

        // Act
        var action = () =>
            repository.Remove(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
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

    private static UserAccount CreateUser(
        string username = "test.user",
        EmailAddress? email = null,
        PhoneNumber? phoneNumber = null)
    {
        return new UserAccount(
            Guid.NewGuid(),
            username,
            email ??
                new EmailAddress(
                    "test@example.com"),
            phoneNumber ??
                new PhoneNumber(
                    "+628123456789"),
            "password-hash",
            DateTime.UtcNow);
    }
}