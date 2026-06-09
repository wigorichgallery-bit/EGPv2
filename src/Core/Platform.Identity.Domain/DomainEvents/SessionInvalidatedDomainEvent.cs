// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/SessionInvalidatedDomainEvent.cs
// ===========================================

using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when user sessions must be invalidated.
/// 
/// Responsibility:
/// - Signals token invalidation.
/// - Triggered after password change, MFA change, or role modification.
/// </summary>
public sealed class SessionInvalidatedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the reason for session invalidation.
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SessionInvalidatedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of invalidation.</param>
    /// <param name="reason">The invalidation reason description.</param>
    public SessionInvalidatedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        string reason)
        : base(aggregateId, occurredOn)
    {
        Reason = reason;
    }
}