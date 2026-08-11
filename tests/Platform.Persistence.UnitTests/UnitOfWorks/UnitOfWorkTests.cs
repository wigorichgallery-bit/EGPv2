// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// UnitOfWorks/UnitOfWorkTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.UnitOfWorks;

namespace Platform.Persistence.UnitTests.UnitOfWorks;

public sealed class UnitOfWorkTests
{
    private static (
        GovernanceDbContext Context,
        SqliteConnection Connection)
        CreateDbContext()
    {
        var connection =
            new SqliteConnection(
                "Data Source=:memory:");

        connection.Open();

        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseSqlite(connection)
                .Options;

        var context =
            new GovernanceDbContext(options);

        context.Database.EnsureCreated();

        return (
            context,
            connection);
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
            new UnitOfWork(null!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>();
    }

    // ============================================================
    // COMMIT
    // ============================================================

    [Fact]
    public async Task CommitAsync_ShouldPersistTrackedChanges()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var user =
                CreateUser();

            dbContext.UserAccounts.Add(
                user);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            var affectedRows =
                await unitOfWork.CommitAsync();

            // Assert
            affectedRows
                .Should()
                .Be(1);

            var persistedUser =
                await dbContext.UserAccounts
                    .AsNoTracking()
                    .SingleOrDefaultAsync(
                        x => x.Id == user.Id);

            persistedUser
                .Should()
                .NotBeNull();

            persistedUser!.Username
                .Should()
                .Be(user.Username);
        }
    }

    [Fact]
    public async Task CommitAsync_ShouldReturnAffectedRows()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
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

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            var affectedRows =
                await unitOfWork.CommitAsync();

            // Assert
            affectedRows
                .Should()
                .Be(2);
        }
    }

    [Fact]
    public async Task CommitAsync_ShouldCommitTransaction_WhenSaveSucceeds()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var user =
                CreateUser();

            dbContext.UserAccounts.Add(
                user);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            await unitOfWork.CommitAsync();

            // Assert
            var exists =
                await dbContext.UserAccounts
                    .AsNoTracking()
                    .AnyAsync(
                        x => x.Id == user.Id);

            exists
                .Should()
                .BeTrue();
        }
    }

    // ============================================================
    // ROLLBACK ON FAILURE
    // ============================================================

    [Fact]
    public async Task CommitAsync_ShouldClearChangeTracker_WhenSaveFails()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            // var user =
            //     CreateUser();

            var user1 = CreateUser(
                Guid.NewGuid(),
                "duplicate-user",
                "user1@example.com",
                "+6281234567890");

            var user2 = CreateUser(
                Guid.NewGuid(),
                "duplicate-user",
                "user2@example.com",
                "+6281234567891");

            // dbContext.UserAccounts.Add(user);
            dbContext.UserAccounts.Add(user1);
            dbContext.UserAccounts.Add(user2);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Force a persistence failure by adding
            // another entity with the same primary key.
            // var duplicate =
            //     CreateUser(
            //         user.Id,
            //         "duplicate.user",
            //         "duplicate@example.com",
            //         "+628999999999");

            // dbContext.UserAccounts.Add(
            //     duplicate);

            // Act
            var action = async () =>
                await unitOfWork.CommitAsync();

            // Assert
            await action
                .Should()
                .ThrowAsync<DbUpdateException>();

            dbContext.ChangeTracker
                .Entries()
                .Should()
                .BeEmpty();
        }
    }

    [Fact]
    public async Task CommitAsync_ShouldNotPersistChanges_WhenSaveFails()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            // var user =
            //     CreateUser();

            // dbContext.UserAccounts.Add(
            //     user);

            // var duplicate =
            //     CreateUser(
            //         user.Id,
            //         "duplicate.user",
            //         "duplicate@example.com",
            //         "+628999999999");

            // dbContext.UserAccounts.Add(
            //     duplicate);

            var user1 = CreateUser(
                Guid.NewGuid(),
                "duplicate-user",
                "user1@example.com",
                "+6281234567890");

            var user2 = CreateUser(
                Guid.NewGuid(),
                "duplicate-user",
                "user2@example.com",
                "+6281234567891");

            dbContext.UserAccounts.Add(user1);
            dbContext.UserAccounts.Add(user2);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            var action = async () =>
                await unitOfWork.CommitAsync();

            await action
                .Should()
                .ThrowAsync<DbUpdateException>();

            // Assert
            dbContext.ChangeTracker
                .Entries()
                .Should()
                .BeEmpty();
        }
    }

    // ============================================================
    // ROLLBACK AS EXPLICIT OPERATION
    // ============================================================

    [Fact]
    public async Task RollbackAsync_ShouldClearTrackedEntities()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var user =
                CreateUser();

            dbContext.UserAccounts.Add(
                user);

            dbContext.ChangeTracker
                .Entries()
                .Should()
                .NotBeEmpty();

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            await unitOfWork.RollbackAsync();

            // Assert
            dbContext.ChangeTracker
                .Entries()
                .Should()
                .BeEmpty();
        }
    }

    [Fact]
    public async Task RollbackAsync_ShouldNotPersistPendingChanges()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var user =
                CreateUser();

            dbContext.UserAccounts.Add(
                user);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            await unitOfWork.RollbackAsync();

            // Assert
            var exists =
                await dbContext.UserAccounts
                    .AsNoTracking()
                    .AnyAsync(
                        x => x.Id == user.Id);

            exists
                .Should()
                .BeFalse();
        }
    }

    [Fact]
    public async Task RollbackAsync_ShouldCompleteSuccessfully_WhenNoChangesExist()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            // Act
            var action = async () =>
                await unitOfWork.RollbackAsync();

            // Assert
            await action
                .Should()
                .NotThrowAsync();

            dbContext.ChangeTracker
                .Entries()
                .Should()
                .BeEmpty();
        }
    }

    // ============================================================
    // CANCELLATION
    // ============================================================

    [Fact]
    public async Task CommitAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            using var cancellationTokenSource =
                new CancellationTokenSource();

            await cancellationTokenSource
                .CancelAsync();

            // Act
            var action = async () =>
                await unitOfWork.CommitAsync(
                    cancellationTokenSource.Token);

            // Assert
            await action
                .Should()
                .ThrowAsync<OperationCanceledException>();
        }
    }

    [Fact]
    public async Task RollbackAsync_ShouldCompleteSuccessfully_WhenCancellationTokenIsCancelled()
    {
        // Arrange
        var (
            dbContext,
            connection) =
            CreateDbContext();

        await using (dbContext)
        await using (connection)
        {
            var user =
                CreateUser();

            dbContext.UserAccounts.Add(
                user);

            var unitOfWork =
                new UnitOfWork(
                    dbContext);

            using var cancellationTokenSource =
                new CancellationTokenSource();

            await cancellationTokenSource
                .CancelAsync();

            // Act
            var action = async () =>
                await unitOfWork.RollbackAsync(
                    cancellationTokenSource.Token);

            // Assert
            await action
                .Should()
                .NotThrowAsync();

            dbContext.ChangeTracker
                .Entries()
                .Should()
                .BeEmpty();
        }
    }
}