// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Repositories/Commands/
// UserAccountRepository.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Context;

namespace Platform.Persistence.Repositories.Commands;

/// <summary>
/// Provides Entity Framework Core implementation
/// of <see cref="IUserAccountRepository"/>.
///
/// Responsibility:
/// - Retrieve tracked user aggregates.
/// - Persist aggregate state.
/// - Execute aggregate existence checks.
/// - Support aggregate lifecycle.
/// - Never return DTOs.
///
/// Architectural Rules:
/// - Command side only.
/// - Tracked entities only.
/// - No business logic.
/// - No application orchestration.
/// - No transaction management.
///
/// Persistence Strategy:
/// - EF Core.
/// - Tracked entities.
/// - Aggregate persistence.
/// - UnitOfWork commit.
///
/// Thread Safety:
/// - Scoped lifetime.
/// - Not thread-safe.
///
/// EF Core Compatibility:
/// - EF Core 10.
/// </summary>
public sealed class UserAccountRepository
    : IUserAccountRepository
{
    private readonly GovernanceDbContext
        _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UserAccountRepository"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="dbContext"/>
    /// is null.
    /// </exception>
    public UserAccountRepository(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext =
            dbContext;
    }

    /// <summary>
    /// Retrieves a tracked user aggregate
    /// by identifier.
    ///
    /// Responsibility:
    /// - Retrieve aggregate.
    /// - Preserve EF Core tracking.
    ///
    /// Algorithm:
    /// 1. Query aggregate by identifier.
    /// 2. Return aggregate when found.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// User aggregate when found;
    /// otherwise null.
    /// </returns>
    public async Task<UserAccount?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .UserAccounts
            .FirstOrDefaultAsync(
                user =>
                    user.Id == userId,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a tracked user aggregate
    /// by username.
    ///
    /// Responsibility:
    /// - Retrieve aggregate.
    /// - Preserve EF Core tracking.
    ///
    /// Algorithm:
    /// 1. Validate username.
    /// 2. Query aggregate.
    /// 3. Return aggregate when found.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="username">
    /// User name.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// User aggregate when found;
    /// otherwise null.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when
    /// <paramref name="username"/>
    /// is null, empty, or whitespace.
    /// </exception>
    public async Task<UserAccount?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            username);

        return await _dbContext
            .UserAccounts
            .SingleOrDefaultAsync(
                user =>
                    user.Username == username,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a tracked user aggregate
    /// by email address.
    ///
    /// Responsibility:
    /// - Retrieve aggregate.
    /// - Preserve EF Core tracking.
    ///
    /// Algorithm:
    /// 1. Validate email.
    /// 2. Query aggregate.
    /// 3. Return aggregate when found.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="email">
    /// Email address.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// User aggregate when found;
    /// otherwise null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="email"/>
    /// is null.
    /// </exception>
    public async Task<UserAccount?> GetByEmailAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            email);

        return await _dbContext
            .UserAccounts
            .SingleOrDefaultAsync(
                user =>
                    user.Email.Value ==
                    email.Value,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Determines whether the specified
    /// user identifier exists.
    ///
    /// Responsibility:
    /// - Execute existence check.
    /// - Preserve aggregate identity.
    ///
    /// Algorithm:
    /// 1. Query by identifier.
    /// 2. Return existence result.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// <c>true</c> when the user exists;
    /// otherwise <c>false</c>.
    /// </returns>
    public async Task<bool> ExistsByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .UserAccounts
            .AnyAsync(
                user => user.Id == userId,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Determines whether the specified
    /// username already exists.
    ///
    /// Responsibility:
    /// - Execute existence check.
    /// - Preserve aggregate uniqueness.
    ///
    /// Algorithm:
    /// 1. Validate username.
    /// 2. Execute existence query.
    /// 3. Return existence result.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="username">
    /// User name.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// <c>true</c> when the username
    /// already exists; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when
    /// <paramref name="username"/>
    /// is null, empty, or whitespace.
    /// </exception>
    public async Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            username);

        return await _dbContext
            .UserAccounts
            .AnyAsync(
                user =>
                    user.Username == username,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Determines whether the specified
    /// email address already exists.
    ///
    /// Responsibility:
    /// - Execute existence check.
    /// - Preserve aggregate uniqueness.
    ///
    /// Algorithm:
    /// 1. Validate email address.
    /// 2. Execute existence query.
    /// 3. Return existence result.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="email">
    /// Email address.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// <c>true</c> when the email
    /// already exists; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="email"/>
    /// is null.
    /// </exception>
    public async Task<bool> ExistsByEmailAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            email);

        return await _dbContext
            .UserAccounts
            .AnyAsync(
                user =>
                    user.Email.Value ==
                    email.Value,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Determines whether the specified
    /// phone number already exists.
    ///
    /// Responsibility:
    /// - Execute existence check.
    /// - Preserve aggregate uniqueness.
    ///
    /// Algorithm:
    /// 1. Validate phone number.
    /// 2. Execute existence query.
    /// 3. Return existence result.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="phoneNumber">
    /// Phone number.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// <c>true</c> when the phone number
    /// already exists; otherwise
    /// <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="phoneNumber"/>
    /// is null.
    /// </exception>
    public async Task<bool> ExistsByPhoneAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            phoneNumber);

        return await _dbContext
            .UserAccounts
            .AnyAsync(
                user =>
                    user.PhoneNumber.Value ==
                    phoneNumber.Value,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all tracked user aggregates.
    ///
    /// Responsibility:
    /// - Retrieve aggregate collection.
    /// - Preserve entity tracking.
    /// - Return immutable collection.
    ///
    /// Algorithm:
    /// 1. Query all aggregates.
    /// 2. Materialize collection.
    /// 3. Return immutable result.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of users.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Read-only list of
    /// <see cref="UserAccount"/>
    /// aggregates.
    /// </returns>
    public async Task<IReadOnlyList<UserAccount>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .UserAccounts
            .ToArrayAsync(
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a new user aggregate
    /// into the persistence context.
    ///
    /// Responsibility:
    /// - Register aggregate for insertion.
    /// - Preserve aggregate tracking.
    /// - Defer persistence to UnitOfWork.
    ///
    /// Algorithm:
    /// 1. Validate aggregate.
    /// 2. Register aggregate.
    /// 3. Return.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userAccount">
    /// User aggregate.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userAccount"/>
    /// is null.
    /// </exception>
    public async Task AddAsync(
        UserAccount userAccount,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            userAccount);

        await _dbContext
            .UserAccounts
            .AddAsync(
                userAccount,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Marks the specified aggregate
    /// as modified.
    ///
    /// Responsibility:
    /// - Mark aggregate as modified.
    /// - Preserve aggregate tracking.
    /// - Defer persistence to UnitOfWork.
    ///
    /// Algorithm:
    /// 1. Validate aggregate.
    /// 2. Mark aggregate as modified.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userAccount">
    /// User aggregate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userAccount"/>
    /// is null.
    /// </exception>
    public void Update(
        UserAccount userAccount)
    {
        ArgumentNullException.ThrowIfNull(
            userAccount);

        _dbContext
            .UserAccounts
            .Update(
                userAccount);
    }

    /// <summary>
    /// Marks the specified aggregate
    /// for deletion.
    ///
    /// Responsibility:
    /// - Mark aggregate for removal.
    /// - Preserve aggregate tracking.
    /// - Defer persistence to UnitOfWork.
    ///
    /// Algorithm:
    /// 1. Validate aggregate.
    /// 2. Mark aggregate for removal.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="userAccount">
    /// User aggregate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userAccount"/>
    /// is null.
    /// </exception>
    public void Remove(
        UserAccount userAccount)
    {
        ArgumentNullException.ThrowIfNull(
            userAccount);

        _dbContext
            .UserAccounts
            .Remove(
                userAccount);
    }

}   
