// ===========================================
// File Location : src/Core/Platform.SharedKernel/Abstractions/IDomainEventDispatcher.cs
// ===========================================

using Platform.SharedKernel.Base;

namespace Platform.SharedKernel.Abstractions;

   /// <summary>
   /// Dispatches domain events after successful transaction commit.
   /// 
   /// Responsibility:
   /// - Receives collected domain events.
   /// - Publishes them to event handlers.
   /// - Must not mutate aggregate state.
   /// 
   /// Architectural Rule:
   /// - Implementation lives in Infrastructure.
   /// - Application layer invokes dispatcher after commit.
   /// - Domain layer must not reference dispatcher.
   /// 
   /// Security Rule:
   /// - Must not expose sensitive data.
   /// - Must not log secret fields.
   /// </summary>
    public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches domain events.
    /// 
    /// Business Rule:
    /// - Must execute after transaction commit.
    /// - Must handle events sequentially or via safe async pipeline.
    /// - Must not swallow critical exceptions silently.
    /// 
    /// Failure Case:
    /// - If dispatch fails, must be logged.
    /// - Should not re-commit transaction.
    /// </summary>
    /// <param name="domainEvents">Domain events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DispatchAsync(
        IReadOnlyCollection<DomainEvent> domainEvents,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches all accumulated domain events.
    /// </summary>
    Task DispatchAsync(CancellationToken cancellationToken);
}