// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Events/TotpSecretSetDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Domain event raised when a TOTP secret has been configured for a user.
///
/// <para>
/// PURPOSE:
/// - Indicates readiness for TOTP MFA activation.
/// </para>
///
/// <para>
/// SECURITY:
/// - Secret value is NOT exposed.
/// - Only indicates that secret exists.
/// </para>
/// </summary>
public sealed class TotpSecretSetDomainEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of <see cref="TotpSecretSetDomainEvent"/>.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="occurredOn">Event timestamp (UTC).</param>
    public TotpSecretSetDomainEvent(
        Guid userId,
        DateTime occurredOn)
        : base(userId, occurredOn)
    {
    }
}