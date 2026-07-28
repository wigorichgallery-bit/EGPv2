// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/UserLockedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a user account becomes locked due to failed login threshold.
/// 
/// Responsibility:
/// - Signals lockout enforcement.
/// 
/// Invariants:
/// - LockoutUntil must be future UTC time.
/// </summary>
public sealed class UserLockedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the lockout expiration timestamp.
    /// </summary>
    public DateTime LockoutUntil { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="UserLockedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of lockout.</param>
    /// <param name="lockoutUntil">The UTC time until which user is locked.</param>
    public UserLockedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        DateTime lockoutUntil)
        : base(aggregateId, occurredOn)
    {
        LockoutUntil = lockoutUntil;
    }
}