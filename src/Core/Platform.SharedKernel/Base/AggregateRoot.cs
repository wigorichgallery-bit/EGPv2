// ===========================================
// File Location : src/Core/Platform.SharedKernel/Base/AggregateRoot.cs
// ===========================================
using Platform.SharedKernel.Utilities;

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
    private const string AggregateIdMismatchMessage =
    "DomainEvent AggregateId must match the AggregateRoot identifier.";

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
    /// Parameterless constructor for ORM and serialization.
    /// </summary>
    protected AggregateRoot() : base()
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
        Guard.AgainstNull(
            domainEvent,
            nameof(domainEvent));

        if (domainEvent.AggregateId != Id)
            throw new InvalidOperationException(AggregateIdMismatchMessage);

        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears domain events.
    /// 
    /// Called after successful commit.
    /// </summary>
    internal void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}