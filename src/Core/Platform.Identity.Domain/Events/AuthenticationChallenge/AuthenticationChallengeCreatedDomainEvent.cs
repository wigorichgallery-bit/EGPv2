// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Events/AuthenticationChallenge/
// AuthenticationChallengeCreatedDomainEvent.cs
// ===========================================

using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ErrorCodes;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Events;

/// <summary>
/// Raised when a new authentication challenge is created.
///
/// Responsibility:
/// - Indicates that an authentication challenge has been created.
/// - Captures the challenge configuration at creation time.
/// - Provides immutable event data for downstream processing.
///
/// Architectural Rules:
/// - Immutable.
/// - Inherits from <see cref="DomainEvent"/>.
/// - Contains no business behavior.
/// - Represents a completed domain fact.
///
/// Invariants:
/// - User identifier must not be empty.
/// - Expiration time must be UTC.
/// - Expiration time must be later than the event occurrence time.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
public sealed class AuthenticationChallengeCreatedDomainEvent
    : DomainEvent
{
    /// <summary>
    /// Gets the identifier of the user associated with the
    /// authentication challenge.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the authentication challenge type.
    /// </summary>
    public AuthenticationChallengeType ChallengeType { get; }

    /// <summary>
    /// Gets the authentication challenge purpose.
    /// </summary>
    public AuthenticationChallengePurpose Purpose { get; }

    /// <summary>
    /// Gets the UTC expiration time of the challenge.
    /// </summary>
    public DateTime ExpiresAtUtc { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="challengeId">
    /// The authentication challenge aggregate identifier.
    /// This value is assigned to
    /// <see cref="DomainEvent.AggregateId"/>.
    /// </param>
    /// <param name="userId">
    /// The user identifier.
    /// </param>
    /// <param name="challengeType">
    /// The authentication challenge type.
    /// </param>
    /// <param name="purpose">
    /// The authentication challenge purpose.
    /// </param>
    /// <param name="occurredOn">
    /// The UTC timestamp when the challenge was created.
    /// This value is assigned to
    /// <see cref="DomainEvent.OccurredOn"/>.
    /// </param>
    /// <param name="expiresAtUtc">
    /// The UTC expiration timestamp of the challenge.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when a parameter is invalid.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the expiration time is not later than the
    /// event occurrence time.
    /// </exception>
    public AuthenticationChallengeCreatedDomainEvent(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
        AuthenticationChallengePurpose purpose,
        DateTime occurredOn,
        DateTime expiresAtUtc)
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
            
        Guard.AgainstNonUtc(
            expiresAtUtc,
            nameof(expiresAtUtc));

        if (expiresAtUtc <= occurredOn)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidChallengeExpiration,
                "Challenge expiration must be later than the creation time.");
        }

        UserId = userId;
        ChallengeType = challengeType;
        Purpose = purpose;
        ExpiresAtUtc = expiresAtUtc;
    }
}