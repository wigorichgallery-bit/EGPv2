// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Events/AuthenticationChallenge/
// AuthenticationChallengeCancelledDomainEvent.cs
// ===========================================

using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when an authentication challenge has been cancelled.
///
/// Responsibility:
/// - Indicates that an authentication challenge was cancelled.
/// - Captures the cancellation reason.
/// - Represents an immutable domain fact.
///
/// Architectural Rules:
/// - Immutable.
/// - Inherits from <see cref="DomainEvent"/>.
/// - Contains no business behavior.
///
/// Invariants:
/// - User identifier must not be empty.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
public sealed class AuthenticationChallengeCancelledDomainEvent
    : DomainEvent
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the authentication challenge type.
    /// </summary>
    public AuthenticationChallengeType ChallengeType { get; }

    /// <summary>
    /// Gets the business purpose of the
    /// authentication challenge.
    /// </summary>
    public AuthenticationChallengePurpose Purpose { get; }

    /// <summary>
    /// Gets the cancellation reason.
    /// </summary>
    public AuthenticationChallengeCancellationReason CancellationReason { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeCancelledDomainEvent"/> class.
    /// </summary>
    /// <param name="challengeId">
    /// Authentication challenge aggregate identifier.
    /// </param>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="challengeType">
    /// Authentication challenge type.
    /// </param>
    /// <param name="cancellationReason">
    /// Cancellation reason.
    /// </param>
    /// <param name="occurredOn">
    /// UTC timestamp when the cancellation occurred.
    /// </param>
    public AuthenticationChallengeCancelledDomainEvent(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
         AuthenticationChallengePurpose purpose,
        AuthenticationChallengeCancellationReason cancellationReason,
        DateTime occurredOn)
        : base(
            challengeId,
            occurredOn)
    {
        Guard.AgainstEmpty(
            userId,
            nameof(userId));

        Guard.AgainstUndefinedEnum(
            challengeType,
            nameof(challengeType));

        Guard.AgainstUndefinedEnum(
            purpose,
            nameof(purpose));

        Guard.AgainstUndefinedEnum(
            cancellationReason,
            nameof(cancellationReason));

        UserId = userId;
        ChallengeType = challengeType;
        Purpose = purpose;
        CancellationReason = cancellationReason;
    }
}