// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Events/MFAEnabledDomainEvent.cs
// ===========================================
using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when MFA is enabled.
/// 
/// Responsibility:
/// - Signals MFA activation.
/// - Triggers security stamp rotation.
/// </summary>
public sealed class MFAEnabledDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the enabled MFA method.
    /// </summary>
    public MFAMethod Method { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="MFAEnabledDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of enablement.</param>
    /// <param name="method">The MFA method enabled.</param>
    public MFAEnabledDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        MFAMethod method)
        : base(aggregateId, occurredOn)
    {
        Method = method;
    }
}