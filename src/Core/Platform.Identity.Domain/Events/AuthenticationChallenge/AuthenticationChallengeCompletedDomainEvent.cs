// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Events/AuthenticationChallenge/
// AuthenticationChallengeCompletedDomainEvent.cs
// ===========================================

using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when an authentication challenge has been
/// completed successfully.
///
/// Responsibility:
/// - Indicates successful completion of an authentication challenge.
/// - Provides immutable event data for downstream processing.
/// - Represents a completed domain fact.
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
public sealed class AuthenticationChallengeCompletedDomainEvent
    : DomainEvent
{
    /// <summary>
    /// Gets the identifier of the user that completed
    /// the authentication challenge.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the authentication challenge type that
    /// was successfully completed.
    /// </summary>
    public AuthenticationChallengeType ChallengeType { get; }

    /// <summary>
    /// Gets the business purpose of the
    /// authentication challenge.
    /// </summary>
    public AuthenticationChallengePurpose Purpose { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeCompletedDomainEvent"/> class.
    /// </summary>
    /// <param name="challengeId">
    /// The authentication challenge aggregate identifier.
    /// This value is assigned to
    /// <see cref="DomainEvent.AggregateId"/>.
    /// </param>
    /// <param name="userId">
    /// The identifier of the authenticated user.
    /// </param>
    /// <param name="challengeType">
    /// The completed authentication challenge type.
    /// </param>
    /// <param name="occurredOn">
    /// The UTC timestamp when the challenge
    /// was completed.
    /// This value is assigned to
    /// <see cref="DomainEvent.OccurredOn"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when a parameter is invalid.
    /// </exception>
    public AuthenticationChallengeCompletedDomainEvent(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
        AuthenticationChallengePurpose purpose,
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

        UserId = userId;
        ChallengeType = challengeType;
        Purpose = purpose;
    }
}