
// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Repositories/Commands/
// AuthenticationChallengeRepositoryTests.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;
using Platform.Persistence.Repositories.Commands;

namespace Platform.Persistence.UnitTests.Repositories.Commands;

/// <summary>
/// Unit tests for
/// <see cref="AuthenticationChallengeRepository"/>.
///
/// <para>
/// Test strategy:
/// - Use a real EF Core DbContext.
/// - Use SQLite in-memory persistence.
/// - Verify tracked aggregate behavior.
/// - Verify repository contract behavior.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - No mocking of EF Core DbContext.
/// - No transaction testing.
/// - No application orchestration.
/// - No domain business-rule duplication.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeRepositoryTests
    : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<GovernanceDbContext> _options;

    /// <summary>
    /// Initializes a new test fixture.
    /// </summary>
    public AuthenticationChallengeRepositoryTests()
    {
        _connection =
            new SqliteConnection(
                "DataSource=:memory:");

        _connection.Open();

        _options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseSqlite(_connection)
                .EnableDetailedErrors()
                .Options;

        using GovernanceDbContext dbContext =
            CreateDbContext();

        dbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Releases the SQLite in-memory connection.
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }

    // ============================================================
    // Constructor
    // ============================================================

    /// <summary>
    /// Verifies that the constructor creates a valid repository.
    /// </summary>
    [Fact]
    public void Constructor_Should_Create_Instance()
    {
        using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        sut.Should()
            .NotBeNull();
    }

    /// <summary>
    /// Verifies that the constructor rejects
    /// a null database context.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_DbContext_Is_Null()
    {
        Action act =
            () => new AuthenticationChallengeRepository(
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("dbContext");
    }

    // ============================================================
    // GetByIdAsync
    // ============================================================

    /// <summary>
    /// Verifies that GetByIdAsync returns the requested
    /// authentication challenge when it exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_Should_ReturnChallenge_When_ChallengeExists()
    {
        Guid challengeId =
            Guid.NewGuid();

        AuthenticationChallenge challenge =
            CreateChallenge(challengeId);

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            await dbContext
                .AuthenticationChallenges
                .AddAsync(challenge);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallengeRepository sut =
                new(dbContext);

            AuthenticationChallenge? result =
                await sut.GetByIdAsync(
                    challengeId,
                    CancellationToken.None);

            result.Should()
                .NotBeNull();

            result!
                .Id
                .Should()
                .Be(challengeId);

            result
                .UserId
                .Should()
                .Be(challenge.UserId);

            result
                .ChallengeType
                .Should()
                .Be(challenge.ChallengeType);

            result
                .Purpose
                .Should()
                .Be(challenge.Purpose);

            result
                .ChallengeSecret
                .Value
                .Should()
                .Be(challenge.ChallengeSecret.Value);
        }
    }

    /// <summary>
    /// Verifies that GetByIdAsync returns null when
    /// the requested challenge does not exist.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_ChallengeDoesNotExist()
    {
        await using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        AuthenticationChallenge? result =
            await sut.GetByIdAsync(
                Guid.NewGuid(),
                CancellationToken.None);

        result.Should()
            .BeNull();
    }

    /// <summary>
    /// Verifies that GetByIdAsync preserves EF Core
    /// tracking for the returned aggregate.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_Should_ReturnTrackedEntity_When_ChallengeExists()
    {
        Guid challengeId =
            Guid.NewGuid();

        AuthenticationChallenge challenge =
            CreateChallenge(challengeId);

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            await dbContext
                .AuthenticationChallenges
                .AddAsync(challenge);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallengeRepository sut =
                new(dbContext);

            AuthenticationChallenge? result =
                await sut.GetByIdAsync(
                    challengeId,
                    CancellationToken.None);

            result.Should()
                .NotBeNull();

            dbContext.Entry(result!)
                .State
                .Should()
                .Be(EntityState.Unchanged);
        }
    }

    /// <summary>
    /// Verifies that GetByIdAsync observes a cancelled
    /// cancellation token.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        await using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        using CancellationTokenSource
            cancellationTokenSource =
                new();

        cancellationTokenSource.Cancel();

        Func<Task> act =
            () => sut.GetByIdAsync(
                Guid.NewGuid(),
                cancellationTokenSource.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    // ============================================================
    // AddAsync
    // ============================================================

    /// <summary>
    /// Verifies that AddAsync registers a new
    /// authentication challenge for insertion.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_AddChallenge_ToDbContext()
    {
        AuthenticationChallenge challenge =
            CreateChallenge();

        await using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        await sut.AddAsync(
            challenge,
            CancellationToken.None);

        dbContext.Entry(challenge)
            .State
            .Should()
            .Be(EntityState.Added);
    }

    /// <summary>
    /// Verifies that AddAsync persists the aggregate
    /// when the UnitOfWork-equivalent SaveChanges operation
    /// is executed by the test.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_PersistChallenge_When_SaveChangesIsCalled()
    {
        AuthenticationChallenge challenge =
            CreateChallenge();

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallengeRepository sut =
                new(dbContext);

            await sut.AddAsync(
                challenge,
                CancellationToken.None);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallenge? persisted =
                await dbContext
                    .AuthenticationChallenges
                    .FirstOrDefaultAsync(
                        x => x.Id == challenge.Id);

            persisted.Should()
                .NotBeNull();

            persisted!
                .ChallengeSecret
                .Value
                .Should()
                .Be(challenge.ChallengeSecret.Value);
        }
    }

    /// <summary>
    /// Verifies that AddAsync rejects a null aggregate.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_ThrowArgumentNullException_When_Challenge_Is_Null()
    {
        await using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        Func<Task> act =
            () => sut.AddAsync(
                null!,
                CancellationToken.None);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "authenticationChallenge");
    }

    // /// <summary>
    // /// Verifies that AddAsync observes a cancelled
    // /// cancellation token.
    // /// </summary>
    // [Fact]
    // public async Task AddAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    // {
    //     AuthenticationChallenge challenge =
    //         CreateChallenge();

    //     await using GovernanceDbContext dbContext =
    //         CreateDbContext();

    //     AuthenticationChallengeRepository sut =
    //         new(dbContext);

    //     using CancellationTokenSource
    //         cancellationTokenSource =
    //             new();

    //     cancellationTokenSource.Cancel();

    //     Func<Task> act =
    //         () => sut.AddAsync(
    //             challenge,
    //             cancellationTokenSource.Token);

    //     await act.Should()
    //         .ThrowAsync<OperationCanceledException>();
    // }

    // ============================================================
    // UpdateAsync
    // ============================================================

    /// <summary>
    /// Verifies that UpdateAsync marks the aggregate
    /// as modified.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_Should_MarkChallengeAsModified()
    {
        AuthenticationChallenge challenge =
            CreateChallenge();

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            await dbContext
                .AuthenticationChallenges
                .AddAsync(challenge);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallengeRepository sut =
                new(dbContext);

            AuthenticationChallenge? trackedChallenge =
                await dbContext
                    .AuthenticationChallenges
                    .FirstOrDefaultAsync(
                        x => x.Id == challenge.Id);

            trackedChallenge.Should()
                .NotBeNull();

            sut.UpdateAsync(
                    trackedChallenge!,
                    CancellationToken.None)
                .Should()
                .NotBeNull();

            dbContext.Entry(trackedChallenge!)
                .State
                .Should()
                .Be(EntityState.Modified);
        }
    }

    /// <summary>
    /// Verifies that UpdateAsync persists an aggregate
    /// state transition when SaveChanges is executed.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_Should_PersistAggregateState_When_SaveChangesIsCalled()
    {
        AuthenticationChallenge challenge =
            CreateChallenge();

        DateTime completionTime =
            challenge.ExpiresAtUtc.AddMinutes(-1);

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            await dbContext
                .AuthenticationChallenges
                .AddAsync(challenge);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallengeRepository sut =
                new(dbContext);

            AuthenticationChallenge? trackedChallenge =
                await dbContext
                    .AuthenticationChallenges
                    .FirstOrDefaultAsync(
                        x => x.Id == challenge.Id);

            trackedChallenge.Should()
                .NotBeNull();

            trackedChallenge!
                .Complete(completionTime);

            await sut.UpdateAsync(
                trackedChallenge,
                CancellationToken.None);

            await dbContext
                .SaveChangesAsync();
        }

        await using (
            GovernanceDbContext dbContext =
                CreateDbContext())
        {
            AuthenticationChallenge? persisted =
                await dbContext
                    .AuthenticationChallenges
                    .FirstOrDefaultAsync(
                        x => x.Id == challenge.Id);

            persisted.Should()
                .NotBeNull();

            persisted!
                .Status
                .Should()
                .Be(AuthenticationChallengeStatus.Completed);

            persisted
                .CompletedAtUtc
                .Should()
                .Be(completionTime);
        }
    }

    /// <summary>
    /// Verifies that UpdateAsync rejects a null aggregate.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_Should_ThrowArgumentNullException_When_Challenge_Is_Null()
    {
        await using GovernanceDbContext dbContext =
            CreateDbContext();

        AuthenticationChallengeRepository sut =
            new(dbContext);

        Func<Task> act =
            () => sut.UpdateAsync(
                null!,
                CancellationToken.None);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "authenticationChallenge");
    }

    // ============================================================
    // Helpers
    // ============================================================

    /// <summary>
    /// Creates a new database context using the
    /// shared SQLite in-memory connection.
    /// </summary>
    private GovernanceDbContext CreateDbContext()
    {
        return new GovernanceDbContext(
            _options);
    }

    /// <summary>
    /// Creates a valid authentication challenge aggregate.
    /// </summary>
    private static AuthenticationChallenge CreateChallenge(
        Guid? challengeId = null)
    {
        DateTime createdAtUtc =
            DateTime.UtcNow.AddMinutes(-1);

        DateTime expiresAtUtc =
            createdAtUtc.AddMinutes(10);

        return AuthenticationChallenge.Create(
            challengeId ?? Guid.NewGuid(),
            Guid.NewGuid(),
            GetAuthenticationChallengeType(),
            GetAuthenticationChallengePurpose(),
            new ChallengeSecret(
                "protected-test-secret"),
            createdAtUtc,
            expiresAtUtc);
    }

    /// <summary>
    /// Returns a valid authentication challenge type
    /// without depending on a specific numeric enum value.
    /// </summary>
    private static AuthenticationChallengeType
        GetAuthenticationChallengeType()
    {
        return Enum
            .GetValues<AuthenticationChallengeType>()
            .First(value =>
                Convert.ToInt32(value) != 0);
    }

    /// <summary>
    /// Returns a valid authentication challenge purpose
    /// without depending on a specific numeric enum value.
    /// </summary>
    private static AuthenticationChallengePurpose
        GetAuthenticationChallengePurpose()
    {
        return Enum
            .GetValues<AuthenticationChallengePurpose>()
            .First(value =>
                Convert.ToInt32(value) != 0);
    }
}