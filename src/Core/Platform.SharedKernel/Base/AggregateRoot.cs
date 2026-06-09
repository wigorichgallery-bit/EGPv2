// ===========================================
// File Location : src/Core/Platform.SharedKernel/Base/AggregateRoot.cs
// ===========================================

namespace Platform.SharedKernel.Base;

/// <summary>
/// Represents an aggregate root in the domain model.
/// 
/// Responsibility:
/// - Root of aggregate boundary.
/// - Collects domain events.
/// - Prevents external state mutation.
/// 
/// Invariants:
/// - Only aggregate root may add domain events.
/// - Domain events are dispatched after transaction commit.
/// </summary>
public abstract class AggregateRoot : BaseEntity
{
    /// <summary>
    /// Internal domain event list.
    /// </summary>
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets read-only domain events.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes aggregate root.
    /// </summary>
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    /// <summary>
    /// Adds domain event to aggregate.
    /// 
    /// Business Rule:
    /// - Event must not be null.
    /// - Event must reference this aggregate.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        if (domainEvent.AggregateId != Id)
            throw new InvalidOperationException("DomainEvent AggregateId mismatch.");

        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears domain events.
    /// 
    /// Called after successful commit.
    /// </summary>
    protected void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}