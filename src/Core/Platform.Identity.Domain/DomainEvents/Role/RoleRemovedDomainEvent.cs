// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/RoleAssignedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a role is removed from a user.
/// 
/// Responsibility:
/// - Signals privilege reduction.
/// - Triggers security stamp update.
/// </summary>
public sealed class RoleRemovedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the removed role identifier.
    /// </summary>
    public Guid RoleId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="RoleRemovedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of removal.</param>
    /// <param name="roleId">The role identifier removed.</param>
    public RoleRemovedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        Guid roleId)
        : base(aggregateId, occurredOn)
    {
        RoleId = roleId;
    }
}