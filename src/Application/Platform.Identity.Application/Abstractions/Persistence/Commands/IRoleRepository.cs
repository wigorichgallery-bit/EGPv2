// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Persistence/Commands/IRoleRepository.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Abstractions.Persistence.Commands;

/// <summary>
/// Defines persistence operations for the Role aggregate root.
///
/// Responsibility:
/// - Aggregate retrieval.
/// - Aggregate existence checks.
/// - Aggregate persistence lifecycle.
///
/// Invariants:
/// - Works only with Role aggregate roots.
/// - Does not expose persistence implementation details.
/// - Does not expose IQueryable or infrastructure concerns.
///
/// Side Effects:
/// - None. Contract definition only.
///
/// Algorithm:
/// 1. Retrieve aggregate by identity criteria.
/// 2. Check aggregate existence.
/// 3. Persist aggregate state transitions.
/// 4. Defer transaction control to IUnitOfWork.
///
/// Complexity:
/// O(1) contract definition.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Retrieves a role by identifier.
    /// </summary>
    /// <param name="roleId">Role identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The matching role aggregate if found; otherwise null.
    /// </returns>
    Task<Role?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a role by name.
    /// </summary>
    /// <param name="roleName">Role name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The matching role aggregate if found; otherwise null.
    /// </returns>
    Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a role name already exists.
    /// </summary>
    /// <param name="roleName">Role name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// True when the role exists; otherwise false.
    /// </returns>
    Task<bool> ExistsByNameAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Read-only collection of role aggregates.
    /// </returns>
    Task<IReadOnlyList<Role>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a role with the specified identifier exists.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new aggregate instance.
    /// </summary>
    /// <param name="role">Aggregate instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(
        Role role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an aggregate as modified.
    /// </summary>
    /// <param name="role">Aggregate instance.</param>
    void Update(
        Role role);

    /// <summary>
    /// Marks an aggregate for removal.
    /// </summary>
    /// <param name="role">Aggregate instance.</param>
    void Remove(
        Role role);
}