// ===========================================
// File Location : src/Core/Platform.SharedKernel/Base/DomainEvent.cs
// ===========================================
using Platform.SharedKernel.Utilities;

namespace Platform.SharedKernel.Base;

/// <summary>
/// Base class for all domain events.
/// 
/// Responsibility:
/// - Represents immutable fact that occurred inside aggregate.
/// - Carries minimal event data.
/// 
/// Invariants:
/// - OccurredOn must be set.
/// - AggregateId must not be empty.
/// 
/// Side Effects:
/// - None.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Gets event occurrence timestamp (UTC).
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Gets aggregate identifier.
    /// </summary>
    public Guid AggregateId { get; }

    /// <summary>
    /// Initializes new domain event.
    /// 
    /// Validation Logic:
    /// - AggregateId must not be empty.
    /// - OccurredOn must be in UTC.
    /// </summary>
    protected DomainEvent(Guid aggregateId, DateTime occurredOn)
    {
        Guard.AgainstEmpty(aggregateId, nameof(aggregateId));
        Guard.AgainstNull(OccurredOn, nameof(occurredOn));
      
        AggregateId = aggregateId;
        OccurredOn = occurredOn;
    }
}