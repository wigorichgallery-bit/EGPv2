// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Repositories/Queries/UserQueryRepositoryTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Queries;

namespace Platform.Persistence.UnitTests.Repositories.Queries;

public sealed class UserQueryRepositoryTests
{
    private static GovernanceDbContext CreateDbContext()
    {
        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        return new GovernanceDbContext(options);
    }

    private static UserAccount CreateUser(
        Guid? id = null,
        string username = "john.doe",
        string email = "john.doe@example.com",
        string phoneNumber = "+628123456789")
    {
        return new UserAccount(
            id ?? Guid.NewGuid(),
            username,
            new EmailAddress(email),
            new PhoneNumber(phoneNumber),
            "password-hash",
            DateTime.UtcNow);
    }

    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbContextIsNull()
    {
        // Act
        var action = () =>
            new UserQueryRepository(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // FIND BY ID
    // ============================================================

    [Fact]
    public async Task FindByIdAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var userId =
            Guid.NewGuid();

        var user =
            CreateUser(
                userId,
                "john.doe",
                "john.doe@example.com",
                "+628123456789");

        dbContext.UserAccounts.Add(user);

        await dbContext.SaveChangesAsync();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.FindByIdAsync(
                userId);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.UserId
            .Should()
            .Be(userId);

        result.Username
            .Should()
            .Be("john.doe");

        result.Email
            .Should()
            .Be("john.doe@example.com");

        result.PhoneNumber
            .Should()
            .Be("+628123456789");

        result.EmailVerified
            .Should()
            .BeFalse();

        result.PhoneVerified
            .Should()
            .BeFalse();

        result.Status
            .Should()
            .Be(UserStatus.Active);

        result.MfaEnabled
            .Should()
            .BeFalse();

        result.MfaMethod
            .Should()
            .Be(MFAMethod.None);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        var userId =
            Guid.NewGuid();

        // Act
        var result =
            await repository.FindByIdAsync(
                userId);

        // Assert
        result
            .Should()
            .BeNull();
    }

    // ============================================================
    // FIND BY USERNAME
    // ============================================================

    [Fact]
    public async Task FindByUsernameAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user =
            CreateUser(
                username: "jane.doe",
                email: "jane.doe@example.com",
                phoneNumber: "+628987654321");

        dbContext.UserAccounts.Add(user);

        await dbContext.SaveChangesAsync();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.FindByUsernameAsync(
                "jane.doe");

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.UserId
            .Should()
            .Be(user.Id);

        result.Username
            .Should()
            .Be("jane.doe");

        result.Email
            .Should()
            .Be("jane.doe@example.com");

        result.PhoneNumber
            .Should()
            .Be("+628987654321");
    }

    [Fact]
    public async Task FindByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.FindByUsernameAsync(
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
    public async Task FindByUsernameAsync_ShouldThrowArgumentException_WhenUsernameIsInvalid(
        string? username)
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var action = async () =>
            await repository.FindByUsernameAsync(
                username!);

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    // ============================================================
    // LIST
    // ============================================================

    [Fact]
    public async Task ListAsync_ShouldReturnAllUsers()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user1 =
            CreateUser(
                username: "john.doe",
                email: "john.doe@example.com",
                phoneNumber: "+628123456789");

        var user2 =
            CreateUser(
                username: "jane.doe",
                email: "jane.doe@example.com",
                phoneNumber: "+628987654321");

        dbContext.UserAccounts.AddRange(
            user1,
            user2);

        await dbContext.SaveChangesAsync();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .HaveCount(2);

        result
            .Select(x => x.UserId)
            .Should()
            .Contain(user1.Id)
            .And
            .Contain(user2.Id);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnEmptyCollection_WhenNoUsersExist()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeEmpty();
    }

    // ============================================================
    // PROJECTION
    // ============================================================

    [Fact]
    public async Task ListAsync_ShouldProjectDomainValuesIntoDtoValues()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user =
            CreateUser(
                username: "projection.user",
                email: "Projection.User@Example.COM",
                phoneNumber: "+628111222333");

        dbContext.UserAccounts.Add(user);

        await dbContext.SaveChangesAsync();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        var dto =
            result
                .Single();

        dto.UserId
            .Should()
            .Be(user.Id);

        dto.Username
            .Should()
            .Be(user.Username);

        dto.Email
            .Should()
            .Be(user.Email.Value);

        dto.PhoneNumber
            .Should()
            .Be(user.PhoneNumber.Value);

        dto.EmailVerified
            .Should()
            .Be(user.EmailVerified);

        dto.PhoneVerified
            .Should()
            .Be(user.PhoneVerified);

        dto.Status
            .Should()
            .Be(user.Status);

        dto.MfaEnabled
            .Should()
            .Be(user.MFAEnabled);

        dto.MfaMethod
            .Should()
            .Be(user.MFAMethod);
    }

    // ============================================================
    // NO TRACKING
    // ============================================================

    [Fact]
    public async Task FindByIdAsync_ShouldNotTrackUser()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user =
            CreateUser();

        dbContext.UserAccounts.Add(user);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.FindByIdAsync(
                user.Id);

        // Assert
        result
            .Should()
            .NotBeNull();

        dbContext.ChangeTracker
            .Entries<UserAccount>()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task FindByUsernameAsync_ShouldNotTrackUser()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user =
            CreateUser();

        dbContext.UserAccounts.Add(user);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.FindByUsernameAsync(
                user.Username);

        // Assert
        result
            .Should()
            .NotBeNull();

        dbContext.ChangeTracker
            .Entries<UserAccount>()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task ListAsync_ShouldNotTrackUsers()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var user1 =
            CreateUser(
                username: "user.one",
                email: "user.one@example.com",
                phoneNumber: "+628111111111");

        var user2 =
            CreateUser(
                username: "user.two",
                email: "user.two@example.com",
                phoneNumber: "+628222222222");

        dbContext.UserAccounts.AddRange(
            user1,
            user2);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        var repository =
            new UserQueryRepository(
                dbContext);

        // Act
        var result =
            await repository.ListAsync();

        // Assert
        result
            .Should()
            .HaveCount(2);

        dbContext.ChangeTracker
            .Entries<UserAccount>()
            .Should()
            .BeEmpty();
    }

    // ============================================================
    // CANCELLATION
    // ============================================================

    [Fact]
    public async Task FindByIdAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        await cancellationTokenSource
            .CancelAsync();

        // Act
        var action = async () =>
            await repository.FindByIdAsync(
                Guid.NewGuid(),
                cancellationTokenSource.Token);

        // Assert
        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task FindByUsernameAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        await cancellationTokenSource
            .CancelAsync();

        // Act
        var action = async () =>
            await repository.FindByUsernameAsync(
                "cancelled.user",
                cancellationTokenSource.Token);

        // Assert
        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ListAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        await using var dbContext =
            CreateDbContext();

        var repository =
            new UserQueryRepository(
                dbContext);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        await cancellationTokenSource
            .CancelAsync();

        // Act
        var action = async () =>
            await repository.ListAsync(
                cancellationTokenSource.Token);

        // Assert
        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }
}