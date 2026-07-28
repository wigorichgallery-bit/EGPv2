// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Repositories/Queries/
// RoleQueryRepository.cs
// ===========================================
using Platform.Identity.Application.Abstractions.Persistence.Queries;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Persistence.Context;
using Platform.Persistence.Projections;

namespace Platform.Persistence.Repositories.Queries;

/// <summary>
/// Provides Entity Framework Core implementation
/// of <see cref="IRoleQueryRepository"/>.
///
/// Responsibility:
/// - Execute read-only role queries.
/// - Retrieve role read models.
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
public sealed class RoleQueryRepository
    : IRoleQueryRepository
{
    private readonly GovernanceDbContext
        _dbContext;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="RoleQueryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// Database context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="dbContext"/>
    /// is null.
    /// </exception>
    public RoleQueryRepository(
        GovernanceDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(
            dbContext);

        _dbContext =
            dbContext;
    }

    /// <summary>
    /// Retrieves all roles.
    ///
    /// Responsibility:
    /// - Execute read-only query.
    /// - Retrieve all roles.
    /// - Convert aggregates into DTOs.
    /// - Return immutable collection.
    ///
    /// Algorithm:
    /// 1. Query all roles.
    /// 2. Convert aggregates into DTOs.
    /// 3. Return immutable collection.
    ///
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of roles.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Read-only collection of
    /// <see cref="RoleDto"/>.
    /// </returns>
    public async Task<IReadOnlyList<RoleDto>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var roles =
            await _dbContext
                .Roles
                .AsNoTracking()
                .ToListAsync(
                    cancellationToken)
                .ConfigureAwait(false);

        return roles
            .Select(
                static role =>
                    RoleProjection.ToDto(
                        role))
            .ToArray();
    }

    /// <summary>
    /// Retrieves the roles matching the specified identifiers.
    ///
    /// <para>
    /// Responsibility:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Execute a read-only query.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Retrieve only the requested roles.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Convert persistence entities into application DTOs.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Return an immutable read-only collection.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// Query Strategy:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Uses <c>AsNoTracking()</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Filters by role identifiers.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Projects entities into <see cref="RoleDto"/>.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// Complexity:
    /// O(n)
    ///
    /// Where:
    /// n = number of requested role identifiers.
    /// </para>
    /// </summary>
    /// <param name="roleIds">
    /// The unique identifiers of the roles to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A read-only collection containing the matching
    /// <see cref="RoleDto"/> instances.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="roleIds"/> is <see langword="null"/>.
    /// </exception>
    public async Task<IReadOnlyList<RoleDto>> FindByIdsAsync(
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(roleIds);

        if (roleIds.Count == 0)
        {
            return [];
        }

        var roles = await _dbContext
            .Roles
            .AsNoTracking()
            .Where(role => roleIds.Contains(role.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return roles
            .Select(
                static role =>
                    RoleProjection.ToDto(
                        role))
            .ToArray();        
    }
}