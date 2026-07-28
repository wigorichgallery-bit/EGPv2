// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Repositories/RoleRepository.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Domain.Aggregates;
using Platform.Persistence.Context;

namespace Platform.Persistence.Repositories.Commands;

/// <summary>
/// EF Core implementation of
/// <see cref="IRoleRepository"/>.
///
/// Responsibility:
/// - Retrieve Role aggregates.
/// - Persist Role aggregates.
/// - Execute role existence checks.
///
/// Architectural Rules:
/// - Infrastructure implementation.
/// - No business logic.
/// - No transaction management.
/// - No orchestration logic.
///
/// Thread Safety:
/// - Scoped service.
///
/// Complexity:
/// - Query dependent.
/// </summary>
public sealed class RoleRepository
    : IRoleRepository
{
    private readonly GovernanceDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    public RoleRepository(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Role?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
        .Roles
        .FirstOrDefaultAsync(
            x => x.Id == roleId,
            cancellationToken)
        .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
        roleName);

        return await _dbContext
        .Roles
        .SingleOrDefaultAsync(
            x => x.Name == roleName,
            cancellationToken)
        .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
        roleName);

        return await _dbContext
        .Roles
        .AnyAsync(
            x => x.Name == roleName,
            cancellationToken)
        .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Role>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
       return await _dbContext
            .Roles
            .ToArrayAsync(
                cancellationToken)
            .ConfigureAwait(false);        
    }

    /// <inheritdoc />
    public async Task AddAsync(
        Role role,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(role);

        await _dbContext
        .Roles
        .AddAsync(
            role,
            cancellationToken)
        .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Update(
        Role role)
    {
        ArgumentNullException.ThrowIfNull(role);

        _dbContext
        .Roles
        .Update(role);
    }

    /// <inheritdoc />
    public void Remove(
        Role role)
    {
        ArgumentNullException.ThrowIfNull(role);

        _dbContext
        .Roles
        .Remove(role);
    }

    /// <summary>
    /// Determines whether the specified
    /// role identifier exists.
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
    /// <param name="roleId">
    /// Role identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// <c>true</c> when the role exists;
    /// otherwise <c>false</c>.
    /// </returns>
    public async Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Roles
            .AnyAsync(
                role => role.Id == roleId,
                cancellationToken)
            .ConfigureAwait(false);
    }
}