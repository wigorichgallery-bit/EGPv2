// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/UserUnlockedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a locked user account is manually unlocked.
/// 
/// Responsibility:
/// - Signals administrative unlock.
/// </summary>
public sealed class UserUnlockedDomainEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of <see cref="UserUnlockedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of unlock.</param>
    public UserUnlockedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn)
        : base(aggregateId, occurredOn)
    {
    }
}