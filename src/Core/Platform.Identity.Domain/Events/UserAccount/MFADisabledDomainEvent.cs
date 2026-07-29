// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Events/MFADisabledDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when MFA is disabled.
/// 
/// Responsibility:
/// - Signals MFA deactivation.
/// - Triggers security stamp rotation.
/// </summary>
public sealed class MFADisabledDomainEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of <see cref="MFADisabledDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of disablement.</param>
    public MFADisabledDomainEvent(
        Guid aggregateId,
        DateTime occurredOn)
        : base(aggregateId, occurredOn)
    {
    }
}