// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Events/UserCreatedDomainEvent.cs
// ===========================================
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a new user account is created.
/// 
/// Responsibility:
/// - Signals successful creation of a UserAccount aggregate.
/// - Provides identity context for audit and post-commit handlers.
/// 
/// Invariants:
/// - AggregateId must not be empty.
/// - OccurredOn must be UTC.
/// - Username must not be null or whitespace.
/// 
/// Side Effects:
/// - None. Pure immutable event.
/// </summary>
public sealed class UserCreatedDomainEvent : DomainEvent
{
    /// <summary>
    /// Gets the username of the created user.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the email of the created user.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="UserCreatedDomainEvent"/>.
    /// </summary>
    /// <param name="aggregateId">The UserAccount aggregate identifier.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    /// <param name="username">The username of the new user.</param>
    /// <param name="email">The email of the new user.</param>
    public UserCreatedDomainEvent(
        Guid aggregateId,
        DateTime occurredOn,
        string username,
        string email)
        : base(aggregateId, occurredOn)
    {
        Guard.AgainstNullOrWhiteSpace(username, nameof(username));
        Guard.AgainstNullOrWhiteSpace(email, nameof(email));

        Username = username;
        Email = email;
    }
}