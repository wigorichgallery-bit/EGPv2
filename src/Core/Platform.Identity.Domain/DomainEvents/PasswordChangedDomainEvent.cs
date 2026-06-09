// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/PasswordChangedDomainEvent.cs
// ===========================================

using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a user's password is changed.
/// 
/// Responsibility:
/// - Signals credential rotation.
/// - Triggers session invalidation.
/// 
/// Invariants:
/// - AggregateId must not be empty.
/// - OccurredOn must be UTC.
/// </summary>
public sealed class PasswordChangedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the new password version after change.
    /// </summary>
    public int PasswordVersion { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PasswordChangedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp when password changed.</param>
    /// <param name="passwordVersion">The new password version.</param>
    public PasswordChangedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        int passwordVersion)
        : base(aggregateId, occurredOn)
    {
        PasswordVersion = passwordVersion;
    }
}