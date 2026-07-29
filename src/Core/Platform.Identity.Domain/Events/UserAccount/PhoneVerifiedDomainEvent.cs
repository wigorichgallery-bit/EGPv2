// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Events/PhoneVerifiedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

    /// <summary>
    /// Domain event raised when a user's phone number has been verified.
    ///
    /// <para>
    /// PURPOSE:
    /// - Enables SMS/WhatsApp MFA.
    /// - Confirms ownership of phone number.
    /// </para>
    /// </summary>
    public sealed class PhoneVerifiedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets verified phone number.
    /// </summary>
    public string PhoneNumber { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PhoneVerifiedDomainEvent"/>.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="occurredOn">Event timestamp (UTC).</param>
    /// <param name="phoneNumber">Verified phone number.</param>
    public PhoneVerifiedDomainEvent(
        Guid userId,
        DateTime occurredOn,
        string phoneNumber)
        : base(userId, occurredOn)
    {
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }
}