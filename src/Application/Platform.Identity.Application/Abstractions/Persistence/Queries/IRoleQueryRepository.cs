// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Persistence/Queries/IRoleQueryRepository.cs
// ===========================================
using Platform.Identity.Application.Contracts.Roles.Dtos;

namespace Platform.Identity.Application.Abstractions.Persistence.Queries;

/// <summary>
/// Provides read-only role query operations.
///
/// Responsibility:
/// - Retrieve role read models.
/// - Projection only.
/// - Never return aggregates.
///
/// Architectural Rules:
/// - Query side only.
/// - Read-only.
/// - No tracking.
/// - No mutation.
/// - No UnitOfWork.
///
/// Thread Safety:
/// - Implementations are scoped.
/// </summary>
public interface IRoleQueryRepository
{
    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    Task<IReadOnlyList<RoleDto>> ListAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves roles by their unique identifiers.
    /// </summary>
    /// <param name="roleIds">The unique identifiers of the roles to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A list of role read models.</returns>
    Task<IReadOnlyList<RoleDto>> FindByIdsAsync(
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default);
}