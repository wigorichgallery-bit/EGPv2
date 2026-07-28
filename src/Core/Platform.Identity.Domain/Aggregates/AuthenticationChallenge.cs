// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/Aggregates/AuthenticationChallenge.cs
// ===========================================

using Platform.Identity.Domain.DomainEvents.AuthenticationChallenge;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ErrorCodes;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.Aggregates;

/// <summary>
/// Represents the aggregate root responsible for managing an
/// authentication challenge lifecycle.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Maintains the authentication challenge lifecycle.
/// </description>
/// </item>
/// <item>
/// <description>
/// Tracks challenge completion, cancellation,
/// expiration and lock state.
/// </description>
/// </item>
/// <item>
/// <description>
/// Tracks failed authentication attempts.
/// </description>
/// </item>
/// <item>
/// <description>
/// Emits domain events for challenge lifecycle transitions.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This aggregate does <b>not</b> perform authentication
/// verification (OTP, TOTP, Passkey or WebAuthn).
/// Verification is performed by the application/domain service,
/// while this aggregate only manages business state transitions.
/// </para>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Pure Domain Model.
/// </description>
/// </item>
/// <item>
/// <description>
/// No Infrastructure dependency.
/// </description>
/// </item>
/// <item>
/// <description>
/// No persistence logic.
/// </description>
/// </item>
/// <item>
/// <description>
/// No cryptographic verification.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Aggregate Lifecycle:
/// </para>
/// <code>
/// Pending
///     ├──► Completed
///     ├──► Cancelled
///     ├──► Expired
///     └──► Locked
/// </code>
/// </summary>
public sealed class AuthenticationChallenge : AggregateRoot
{
    // ============================================================
    // Identity
    // ============================================================

    /// <summary>
    /// Gets the identifier of the user that owns this
    /// authentication challenge.
    /// </summary>
    public Guid UserId { get; private set; }

    // ============================================================
    // Challenge Configuration
    // ============================================================

    /// <summary>
    /// Gets the authentication challenge type.
    /// </summary>
    public AuthenticationChallengeType ChallengeType { get; private set; }

    /// <summary>
    /// Gets the authentication challenge purpose.
    /// </summary>
    public AuthenticationChallengePurpose Purpose { get; private set; }

    /// <summary>
    /// Gets the protected challenge secret.
    /// </summary>
    public ChallengeSecret ChallengeSecret { get; private set; } = default!;

    // ============================================================
    // Lifecycle
    // ============================================================

    /// <summary>
    /// Gets the current authentication challenge status.
    /// </summary>
    public AuthenticationChallengeStatus Status { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp indicating when the
    /// authentication challenge was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp indicating when the
    /// authentication challenge expires.
    /// </summary>
    public DateTime ExpiresAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp indicating when the
    /// authentication challenge was successfully completed.
    /// </summary>
    public DateTime? CompletedAtUtc { get; private set; }

    /// <summary>
    /// Gets the reason why the authentication challenge
    /// was cancelled.
    /// </summary>
    public AuthenticationChallengeCancellationReason? CancellationReason
    {
        get;
        private set;
    }
    /// <summary>
    /// Gets the UTC timestamp indicating when the
    /// authentication challenge was cancelled.
    /// </summary>
    public DateTime? CancelledAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp indicating when the
    /// authentication challenge was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; private set; }

    // ============================================================
    // Retry
    // ============================================================

    /// <summary>
    /// Gets the number of failed authentication attempts
    /// associated with this challenge.
    /// </summary>
    public int FailedAttemptCount { get; private set; }

    // ============================================================
    // Constructors
    // ============================================================

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallenge"/> class for
    /// Entity Framework Core materialization.
    /// </summary>
    private AuthenticationChallenge()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallenge"/> class.
    /// </summary>
    /// <param name="challengeId">
    /// The unique authentication challenge identifier.
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
    /// <param name="challengeSecret">
    /// The protected challenge secret.
    /// </param>
    /// <param name="createdAtUtc">
    /// The UTC timestamp indicating when the challenge
    /// was created.
    /// </param>
    /// <param name="expiresAtUtc">
    /// The UTC timestamp indicating when the challenge
    /// expires.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when any supplied parameter is invalid.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the expiration timestamp is earlier than
    /// or equal to the creation timestamp.
    /// </exception>
    private AuthenticationChallenge(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
        AuthenticationChallengePurpose purpose,
        ChallengeSecret challengeSecret,
        DateTime createdAtUtc,
        DateTime expiresAtUtc)
        : base(challengeId)
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

        Guard.AgainstNull(
            challengeSecret,
            nameof(challengeSecret));

        Guard.AgainstNonUtc(
            createdAtUtc,
            nameof(createdAtUtc));

        Guard.AgainstNonUtc(
            expiresAtUtc,
            nameof(expiresAtUtc));

        if (expiresAtUtc <= createdAtUtc)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidChallengeExpiration,
                "Challenge expiration must be later than creation time.");
        }

        UserId = userId;
        ChallengeType = challengeType;
        Purpose = purpose;
        ChallengeSecret = challengeSecret;

        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;

        Status = AuthenticationChallengeStatus.Pending;

        FailedAttemptCount = 0;

        AddDomainEvent(
            new AuthenticationChallengeCreatedDomainEvent(
                Id,
                UserId,
                ChallengeType,
                Purpose,
                createdAtUtc,
                expiresAtUtc));
    }

    /// <summary>
    /// Creates a new authentication challenge.
    ///
    /// <para>
    /// This factory method represents the canonical entry point
    /// for creating authentication challenge aggregates.
    /// </para>
    ///
    /// <para>
    /// The created aggregate is initialized in the
    /// <see cref="AuthenticationChallengeStatus.Pending"/>
    /// state and emits the corresponding domain event.
    /// </para>
    /// </summary>
    /// <param name="challengeId">
    /// The authentication challenge identifier.
    /// </param>
    /// <param name="userId">
    /// The user identifier.
    /// </param>
    /// <param name="challengeType">
    /// The authentication challenge type.
    /// </param>
    /// <param name="purpose">
    /// The business purpose of the challenge.
    /// </param>
    /// <param name="challengeSecret">
    /// The protected challenge secret.
    /// </param>
    /// <param name="createdAtUtc">
    /// The UTC timestamp indicating when the challenge
    /// was created.
    /// </param>
    /// <param name="expiresAtUtc">
    /// The UTC timestamp indicating when the challenge
    /// expires.
    /// </param>
    /// <returns>
    /// A newly created
    /// <see cref="AuthenticationChallenge"/> aggregate.
    /// </returns>
    public static AuthenticationChallenge Create(
        Guid challengeId,
        Guid userId,
        AuthenticationChallengeType challengeType,
        AuthenticationChallengePurpose purpose,
        ChallengeSecret challengeSecret,
        DateTime createdAtUtc,
        DateTime expiresAtUtc)
    {
        return new AuthenticationChallenge(
            challengeId,
            userId,
            challengeType,
            purpose,
            challengeSecret,
            createdAtUtc,
            expiresAtUtc);
    }

    // ============================================================
    // Aggregate Invariants
    // ============================================================

    /// <summary>
    /// Ensures the authentication challenge is still pending.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge is no longer pending.
    /// </exception>
    private void EnsurePending()
    {
        if (Status != AuthenticationChallengeStatus.Pending)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidState,
                $"Authentication challenge must be in Pending state. Current state: {Status}.");
        }
    }

    /// <summary>
    /// Ensures the authentication challenge has not expired.
    /// </summary>
    /// <param name="nowUtc">
    /// The current UTC timestamp.
    /// </param>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge has already expired.
    /// </exception>
    private void EnsureNotExpired(DateTime nowUtc)
    {
        if (nowUtc > ExpiresAtUtc)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidState,
                $"Authentication challenge expired at {ExpiresAtUtc:O}.");
        }
    }

    /// <summary>
    /// Ensures the authentication challenge has reached its expiration time.
    /// </summary>
    /// <param name="nowUtc">The current UTC timestamp.</param>
    /// <exception cref="DomainException">Thrown when the authentication challenge has not reached its expiration time.</exception>
    private void EnsureExpirationReached(
    DateTime nowUtc)
    {
        if (nowUtc < ExpiresAtUtc)
        {
            throw new DomainException(
                IdentityDomainErrorCodes.InvalidState,
                $"Authentication challenge cannot expire before {ExpiresAtUtc:O}.");
        }
    }

    // ============================================================
    // Lifecycle
    // ============================================================

    /// <summary>
    /// Marks the authentication challenge as successfully completed.
    /// </summary>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied timestamp is not expressed in UTC.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge is not in the
    /// pending state.
    /// </exception>
    public void Complete(
        DateTime nowUtc)
    {
        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        EnsurePending();
        EnsureNotExpired(nowUtc);
        CompletedAtUtc = nowUtc;
        Status = AuthenticationChallengeStatus.Completed;

        AddDomainEvent(
            new AuthenticationChallengeCompletedDomainEvent(
                Id,
                UserId,
                ChallengeType,
                Purpose,
                nowUtc));
    }

    /// <summary>
    /// Cancels the authentication challenge.
    /// </summary>
    /// <param name="reason">
    /// The business reason for cancelling the authentication challenge.
    /// </param>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied timestamp is not expressed in UTC.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge cannot be cancelled.
    /// </exception>
    public void Cancel(
        AuthenticationChallengeCancellationReason reason,
        DateTime nowUtc)
    {
        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        Guard.AgainstUndefinedEnum(
            reason,
            nameof(reason));

        EnsurePending();

        EnsureNotExpired(nowUtc);

        CancellationReason = reason;
        CancelledAtUtc = nowUtc;
        Status = AuthenticationChallengeStatus.Cancelled;

        AddDomainEvent(
            new AuthenticationChallengeCancelledDomainEvent(
                Id,
                UserId,
                ChallengeType,
                Purpose,
                reason,
                nowUtc));
    }

    /// <summary>
    /// Marks the authentication challenge as expired.
    /// </summary>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied timestamp is not expressed in UTC.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge cannot be expired.
    /// </exception>
    public void Expire(
        DateTime nowUtc)
    {
        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        EnsurePending();

        // Business invariant:
        // Expire() should only execute after the configured
        // expiration timestamp has been reached.
        EnsureExpirationReached(nowUtc);

        Status = AuthenticationChallengeStatus.Expired;

        AddDomainEvent(
            new AuthenticationChallengeExpiredDomainEvent(
                Id,
                UserId,
                ChallengeType,
                Purpose,
                nowUtc));
    }

    /// <summary>
    /// Registers a failed authentication attempt.
    ///
    /// <para>
    /// Increments the failed attempt counter and locks the
    /// authentication challenge when the configured threshold
    /// is reached.
    /// </para>
    /// </summary>
    /// <param name="maximumFailedAttempts">
    /// Maximum number of failed authentication attempts allowed
    /// before the challenge is locked.
    /// </param>
    /// <param name="nowUtc">
    /// The current UTC timestamp.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more supplied arguments are invalid.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge cannot accept
    /// additional failed attempts.
    /// </exception>
    public void RegisterFailedAttempt(
        int maximumFailedAttempts,
        DateTime nowUtc)
    {
        Guard.AgainstNegativeOrZero(
            maximumFailedAttempts,
            nameof(maximumFailedAttempts));

        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        EnsurePending();

        EnsureNotExpired(nowUtc);

        FailedAttemptCount++;

        if (FailedAttemptCount >= maximumFailedAttempts)
        {
            Lock(nowUtc);
        }
    }
    
    /// <summary>
    /// Locks the authentication challenge.
    ///
    /// <para>
    /// Transitions the authentication challenge from the
    /// <see cref="AuthenticationChallengeStatus.Pending"/>
    /// state to the
    /// <see cref="AuthenticationChallengeStatus.Locked"/>
    /// state after the maximum number of failed
    /// authentication attempts has been reached.
    /// </para>
    ///
    /// <para>
    /// This method is responsible only for performing the
    /// aggregate lifecycle transition. The decision of when
    /// the challenge should be locked is delegated to the
    /// caller, typically through
    /// <see cref="RegisterFailedAttempt(int, DateTime)"/>.
    /// </para>
    ///
    /// <para>
    /// Domain Event:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="AuthenticationChallengeLockedDomainEvent"/>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Aggregate Pattern:
    /// Guard
    /// → Aggregate Invariants
    /// → Business State Mutation
    /// → Domain Event
    /// </para>
    /// </summary>
    /// <param name="nowUtc">
    /// Current UTC timestamp.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when
    /// <paramref name="nowUtc"/>
    /// is not expressed in UTC.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the authentication challenge
    /// cannot transition to the locked state.
    /// </exception>
    public void Lock(
        DateTime nowUtc)
    {
        Guard.AgainstNonUtc(
            nowUtc,
            nameof(nowUtc));

        EnsurePending();

        EnsureNotExpired(nowUtc);

        Status = AuthenticationChallengeStatus.Locked;

        LockedAtUtc = nowUtc;

        AddDomainEvent(
            new AuthenticationChallengeLockedDomainEvent(
                Id,
                UserId,
                ChallengeType,
                Purpose,
                FailedAttemptCount,
                nowUtc));
    }
}