// ===========================================
// File Location : src/Core/Platform.SharedKernel/Abstractions/IUnitOfWork.cs
// ===========================================
namespace Platform.SharedKernel.Abstractions;

/// <summary>
/// Represents a transactional unit of work abstraction.
/// 
/// Responsibility:
/// - Defines transaction boundary contract.
/// - Ensures atomic commit of aggregate changes.
/// - Coordinates persistence and domain event dispatch trigger.
/// 
/// Architectural Rule:
/// - Implemented in Infrastructure layer only.
/// - Used by Application layer.
/// - Never referenced in Domain logic directly.
/// 
/// Transaction Policy:
/// - One UnitOfWork per request.
/// - Commit must occur before domain event dispatch.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes within the transaction boundary.
    /// 
    /// Business Rule:
    /// - Must persist aggregate changes.
    /// - Must guarantee atomicity.
    /// - Must throw on infrastructure failure.
    /// 
    /// Failure Case:
    /// - Infrastructure exception triggers rollback.
    /// 
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of state entries written.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// 
    /// Business Rule:
    /// - Must revert uncommitted changes.
    /// - Safe to call multiple times.
    /// 
    /// Failure Case:
    /// - Should not throw if already rolled back.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
