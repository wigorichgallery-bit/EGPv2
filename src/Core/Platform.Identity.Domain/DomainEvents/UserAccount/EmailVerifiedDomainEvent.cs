// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/EmailVerifiedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Domain event raised when a user's email has been verified.
///
/// <para>
/// PURPOSE:
/// - Indicates that email verification process is completed.
/// - Enables features such as Email-based MFA.
/// </para>
///
/// <para>
/// SECURITY:
/// - Used for audit logging.
/// - May trigger notification or compliance tracking.
/// </para>
/// </summary>
public sealed class EmailVerifiedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets verified email address.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EmailVerifiedDomainEvent"/>.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="occurredOn">Event timestamp (UTC).</param>
    /// <param name="email">Verified email address.</param>
    public EmailVerifiedDomainEvent(
        Guid userId,
        DateTime occurredOn,
        string email)
        : base(userId, occurredOn)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}