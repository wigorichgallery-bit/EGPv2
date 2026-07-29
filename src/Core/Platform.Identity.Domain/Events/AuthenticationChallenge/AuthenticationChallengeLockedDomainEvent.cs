// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Events/AuthenticationChallenge/
// AuthenticationChallengeLockedDomainEvent.cs
// ===========================================

using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when an authentication challenge is locked after
/// exceeding the maximum allowed failed verification attempts.
///
/// Responsibility:
/// - Indicates that an authentication challenge has been locked.
/// - Captures the authentication challenge context.
/// - Captures the number of failed verification attempts.
/// - Represents an immutable domain fact.
///
/// Architectural Rules:
/// - Immutable.
/// - Inherits from <see cref="DomainEvent"/>.
/// - Contains no business behavior.
///
/// Invariants:
/// - User identifier must not be empty.
/// - Failed attempt count must be greater than zero.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
public sealed class AuthenticationChallengeLockedDomainEvent
    : DomainEvent
{
    /// <summary>
    /// Gets the identifier of the user associated with the
    /// locked authentication challenge.
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
    /// Gets the failed verification attempt count that
    /// caused the authentication challenge to be locked.
    /// </summary>
    public int FailedAttemptCount { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeLockedDomainEvent"/> class.
    /// </summary>
    /// <param name="challengeId">
    /// The authentication challenge aggregate identifier.
    /// This value is assigned to
    /// <see cref="DomainEvent.AggregateId"/>.
    /// </param>
    /// <param name="userId">
    /// The identifier of the associated user.
    /// </param>
    /// <param name="challengeType">
    /// The authentication challenge type.
    /// </param>
    /// <param name="purpose">
    /// The business purpose of the authentication challenge.
    /// </param>
    /// <param name="failedAttemptCount">
    /// The failed verification attempt count that caused
    /// the authentication challenge to be locked.
    /// </param>
    /// <param name="occurredOn">
    /// The UTC timestamp when the challenge was locked.
    /// This value is assigned to
    /// <see cref="DomainEvent.OccurredOn"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more supplied arguments are invalid.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when
    /// <paramref name="failedAttemptCount"/>
    /// is less than or equal to zero.
    /// </exception>
    public AuthenticationChallengeLockedDomainEvent(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
        AuthenticationChallengePurpose purpose,
        int failedAttemptCount,
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

        Guard.AgainstNegativeOrZero(
            failedAttemptCount,
            nameof(failedAttemptCount));

        UserId = userId;
        ChallengeType = challengeType;
        Purpose = purpose;
        FailedAttemptCount = failedAttemptCount;
    }
}