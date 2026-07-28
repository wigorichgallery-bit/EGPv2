// ===========================================
// File Location : src/Application/Platform.Identity.Application/Abstractions/Persistence/Queries/IUserQueryRepository.cs
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;

namespace Platform.Identity.Application.Abstractions.Persistence.Queries;

/// <summary>
/// Provides read-only user query operations.
///
/// Responsibility:
/// - Retrieve user read models.
/// - Perform projection-only queries.
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
public interface IUserQueryRepository
{
    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    Task<UserDto?> FindByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    Task<UserDto?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    Task<IReadOnlyList<UserDto>> ListAsync(
        CancellationToken cancellationToken = default);
}