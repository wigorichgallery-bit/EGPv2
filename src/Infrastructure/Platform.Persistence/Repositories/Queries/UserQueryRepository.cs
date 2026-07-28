// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Repositories/Queries/
// UserQueryRepository.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Persistence.Context;
using Platform.Persistence.Projections;

namespace Platform.Persistence.Repositories.Queries;

/// <summary>
/// Provides Entity Framework Core implementation
/// of <see cref="IUserQueryRepository"/>.
///
/// Responsibility:
/// - Execute read-only user queries.
/// - Retrieve user read models.
/// - Convert aggregates into DTOs.
/// - Centralize query persistence.
/// - Never expose aggregates.
///
/// Architectural Rules:
/// - Query side only.
/// - Read-only.
/// - No tracking.
/// - No UnitOfWork.
/// - No business logic.
/// - No domain mutation.
///
/// Query Strategy:
/// - EF Core.
/// - AsNoTracking().
/// - Projection Layer.
/// - DTO only.
///
/// Thread Safety:
/// - Scoped lifetime.
/// - Not thread-safe.
///
/// EF Core Compatibility:
/// - EF Core 10.
/// </summary>
public sealed class UserQueryRepository
    : IUserQueryRepository
{
    private readonly GovernanceDbContext
        _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UserQueryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="dbContext"/>
    /// is null.
    /// </exception>
    public UserQueryRepository(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext =
            dbContext;
    }

    /// <summary>
    /// Retrieves a user by identifier.
    ///
    /// Responsibility:
    /// - Execute read-only query.
    /// - Project aggregate into DTO.
    /// - Return null when user
    ///   does not exist.
    ///
    /// Algorithm:
    /// 1. Query user by identifier.
    /// 2. Return null when not found.
    /// 3. Convert aggregate into DTO.
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
    /// User DTO when found;
    /// otherwise null.
    /// </returns>
    public async Task<UserDto?> FindByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user =
            await _dbContext
                .UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken)
                .ConfigureAwait(false);

        if (user is null)
        {
            return null;
        }

        return UserProjection
            .ToDto(user);
    }

    /// <summary>
    /// Retrieves a user by username.
    ///
    /// Responsibility:
    /// - Execute read-only query.
    /// - Retrieve user by username.
    /// - Convert aggregate into DTO.
    /// - Return null when user
    ///   does not exist.
    ///
    /// Algorithm:
    /// 1. Validate username.
    /// 2. Query user by username.
    /// 3. Return null when not found.
    /// 4. Convert aggregate into DTO.
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
    /// User DTO when found;
    /// otherwise null.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when
    /// <paramref name="username"/>
    /// is null, empty, or whitespace.
    /// </exception>
    public async Task<UserDto?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            username);

        var user =
            await _dbContext
                .UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Username == username,
                    cancellationToken)
                .ConfigureAwait(false);

        if (user is null)
        {
            return null;
        }

        return UserProjection
            .ToDto(user);
    }

    /// <summary>
    /// Retrieves all users.
    ///
    /// Responsibility:
    /// - Execute read-only query.
    /// - Retrieve all users.
    /// - Convert aggregates into DTOs.
    /// - Return immutable collection.
    ///
    /// Algorithm:
    /// 1. Query all users.
    /// 2. Convert aggregates into DTOs.
    /// 3. Return immutable collection.
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
    /// Read-only collection of
    /// <see cref="UserDto"/>.
    /// </returns>
    public async Task<IReadOnlyList<UserDto>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var users =
            await _dbContext
                .UserAccounts
                .AsNoTracking()
                .ToListAsync(
                    cancellationToken)
                .ConfigureAwait(false);

        return users
            .Select(
                static user =>
                    UserProjection.ToDto(
                        user))
            .ToArray();
    }
}