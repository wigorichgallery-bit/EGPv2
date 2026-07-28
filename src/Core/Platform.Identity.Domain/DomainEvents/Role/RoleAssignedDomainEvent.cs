// ===========================================
// File Location : src/Core/Platform.Identity.Domain/DomainEvents/RoleAssignedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a role is assigned to a user.
/// 
/// Responsibility:
/// - Signals privilege change.
/// - Triggers security stamp update.
/// </summary>
public sealed class RoleAssignedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the assigned role identifier.
    /// </summary>
    public Guid RoleId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="RoleAssignedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp of assignment.</param>
    /// <param name="roleId">The role identifier assigned.</param>
    public RoleAssignedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        Guid roleId)
        : base(aggregateId, occurredOn)
    {
        RoleId = roleId;
    }
}