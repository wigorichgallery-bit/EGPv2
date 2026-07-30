using FluentAssertions;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ErrorCodes;
using Platform.Identity.Domain.Events;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Aggregates;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallenge"/>.
/// </summary>
public sealed partial class AuthenticationChallengeTests
{
    #region Factory Test
    /// <summary>
    /// Verifies that creating an authentication challenge
    /// initializes every property correctly.
    /// </summary>
    [Fact]
    public void Create_ShouldInitializeAllProperties()
    {
        // Arrange
        var challengeId = Guid.NewGuid();

        var userId = Guid.NewGuid();

        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            createdAt.AddMinutes(5);

        var secret =
            new ChallengeSecret("ENCRYPTED_SECRET");

        // Act
        var challenge =
            AuthenticationChallenge.Create(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                secret,
                createdAt,
                expiresAt);

        // Assert
        challenge.Id.Should().Be(challengeId);

        challenge.UserId.Should().Be(userId);

        challenge.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);

        challenge.Purpose.Should()
            .Be(AuthenticationChallengePurpose.Login);

        challenge.ChallengeSecret.Should()
            .Be(secret);

        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Pending);

        challenge.CreatedAtUtc.Should()
            .Be(createdAt);

        challenge.ExpiresAtUtc.Should()
            .Be(expiresAt);

        challenge.CompletedAtUtc.Should()
            .BeNull();

        challenge.CancelledAtUtc.Should()
            .BeNull();

        challenge.LockedAtUtc.Should()
            .BeNull();

        challenge.CancellationReason.Should()
            .BeNull();

        challenge.FailedAttemptCount.Should()
            .Be(0);
    }

    /// <summary>
    /// Verifies that creating an authentication challenge
    /// raises the corresponding domain event.
    /// </summary>
    [Fact]
    public void Create_ShouldRaiseAuthenticationChallengeCreatedDomainEvent()
    {
        // Arrange
        var challengeId =
            Guid.NewGuid();

        var userId =
            Guid.NewGuid();

        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            createdAt.AddMinutes(5);

        // Act
        var challenge =
            AuthenticationChallenge.Create(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                expiresAt);

        // Assert
        challenge.DomainEvents.Should()
            .ContainSingle();

        var domainEvent =
            challenge.DomainEvents.Single()
                .Should()
                .BeOfType<AuthenticationChallengeCreatedDomainEvent>()
                .Subject;

        domainEvent.AggregateId.Should()
            .Be(challengeId);

        domainEvent.UserId.Should()
            .Be(userId);

        domainEvent.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);

        domainEvent.Purpose.Should()
            .Be(AuthenticationChallengePurpose.Login);

        domainEvent.OccurredOn.Should()
            .Be(createdAt);
    }

    /// <summary>
    /// Verifies that an empty user identifier
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenUserIdIsEmpty()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.Empty,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an undefined challenge type
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenChallengeTypeIsUndefined()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (AuthenticationChallengeType)999,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an undefined challenge purpose
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenPurposeIsUndefined()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                (AuthenticationChallengePurpose)999,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a null challenge secret
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenChallengeSecretIsNull()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                null!,
                createdAt,
                createdAt.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that a non-UTC creation timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenCreatedAtIsNotUtc()
    {
        // Arrange
        var createdAt =
            DateTime.Now;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC expiration timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenExpiresAtIsNotUtc()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            DateTime.Now;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                expiresAt);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an expiration timestamp
    /// earlier than the creation timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenExpirationIsEarlierThanCreation()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(-1));

        // Assert
        var exception =
            action.Should()
                .Throw<DomainException>()
                .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidChallengeExpiration);
    }

    /// <summary>
    /// Verifies that an expiration timestamp
    /// equal to the creation timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Create_ShouldThrow_WhenExpirationEqualsCreation()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        // Act
        var action = () =>
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt);

        // Assert
        var exception =
            action.Should()
                .Throw<DomainException>()
                .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidChallengeExpiration);
    }

    #endregion

    #region Test Helpers

    /// <summary>
    /// Creates a valid pending authentication challenge.
    /// </summary>
    private static AuthenticationChallenge CreatePendingChallenge()
    {
        var createdAt =
            DateTime.UtcNow;

        return AuthenticationChallenge.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            AuthenticationChallengeType.EmailOtp,
            AuthenticationChallengePurpose.Login,
            new ChallengeSecret("ENCRYPTED_SECRET"),
            createdAt,
            createdAt.AddMinutes(5));
    }

    /// <summary>
    /// Returns only the domain events raised
    /// after the supplied event count.
    /// </summary>
    private static IReadOnlyList<DomainEvent> GetNewDomainEvents(
        AuthenticationChallenge challenge,
        int previousCount)
    {
        return challenge.DomainEvents
            .Skip(previousCount)
            .ToArray();
    }

    /// <summary>
    /// Creates a valid locked authentication challenge.
    /// </summary>
    /// <param name="lockedAt">
    /// Returns the UTC timestamp used to lock the challenge.
    /// </param>
    /// <returns>
    /// A locked authentication challenge.
    /// </returns>
    private static AuthenticationChallenge CreateLockedChallenge(
        out DateTime lockedAt)
    {
        var challenge =
            CreatePendingChallenge();

        lockedAt =
            DateTime.UtcNow;

        challenge.RegisterFailedAttempt(
            1,
            lockedAt);

        return challenge;
    }

    #endregion

    #region Complete Test
    /// <summary>
    /// Verifies that completing a pending challenge
    /// updates the aggregate state.
    /// </summary>
    [Fact]
    public void Complete_ShouldCompleteChallenge()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var completedAt =
            DateTime.UtcNow;

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Complete(
            completedAt);

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Completed);

        challenge.CompletedAtUtc.Should()
            .Be(completedAt);

        var events =
            GetNewDomainEvents(
                challenge,
                before);

        events.Should()
            .ContainSingle(e =>
                e is AuthenticationChallengeCompletedDomainEvent);
    }

    /// <summary>
    /// Verifies that completing a challenge
    /// raises the corresponding domain event.
    /// </summary>
    [Fact]
    public void Complete_ShouldRaiseAuthenticationChallengeCompletedDomainEvent()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var completedAt =
            DateTime.UtcNow;

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Complete(
            completedAt);

        // Assert
        var domainEvent =
            GetNewDomainEvents(
                challenge,
                before)
            .Should()
            .ContainSingle()
            .Subject
            .Should()
            .BeOfType<AuthenticationChallengeCompletedDomainEvent>()
            .Subject;

        domainEvent.AggregateId.Should()
            .Be(challenge.Id);

        domainEvent.UserId.Should()
            .Be(challenge.UserId);

        domainEvent.ChallengeType.Should()
            .Be(challenge.ChallengeType);

        domainEvent.Purpose.Should()
            .Be(challenge.Purpose);

        domainEvent.OccurredOn.Should()
            .Be(completedAt);
    }

    /// <summary>
    /// Verifies that a completed challenge
    /// cannot be completed again.
    /// </summary>
    [Fact]
    public void Complete_ShouldThrow_WhenAlreadyCompleted()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Complete(
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Complete(
                DateTime.UtcNow.AddSeconds(1));

        // Assert
        var exception =
            action.Should()
                .Throw<DomainException>()
                .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a cancelled challenge
    /// cannot be completed.
    /// </summary>
    [Fact]
    public void Complete_ShouldThrow_WhenCancelled()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Complete(
                DateTime.UtcNow.AddSeconds(1));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a locked challenge
    /// cannot be completed.
    /// </summary>
    [Fact]
    public void Complete_ShouldThrow_WhenLocked()
    {
        // Arrange
        var challenge =
            CreateLockedChallenge(
                out var lockedAt);

        // Act
        var action = () =>
            challenge.Complete(
                lockedAt.AddSeconds(1));

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Locked);

        challenge.CompletedAtUtc.Should()
            .BeNull();

        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that an expired challenge
    /// cannot be completed.
    /// </summary>
    [Fact]
    public void Complete_ShouldThrow_WhenExpired()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Act
        var action = () =>
            challenge.Complete(
                createdAt.AddMinutes(6));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Complete_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.Complete(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Cancel Test
    /// <summary>
    /// Verifies that cancelling a pending challenge
    /// updates the aggregate state.
    /// </summary>
    [Fact]
    public void Cancel_ShouldCancelChallenge()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var cancelledAt =
            DateTime.UtcNow;

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            cancelledAt);

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Cancelled);

        challenge.CancelledAtUtc.Should()
            .Be(cancelledAt);

        challenge.CancellationReason.Should()
            .Be(
                AuthenticationChallengeCancellationReason.UserCancelled);

        var events =
            GetNewDomainEvents(
                challenge,
                before);

        events.Should()
            .ContainSingle(e =>
                e is AuthenticationChallengeCancelledDomainEvent);
    }

    /// <summary>
    /// Verifies that cancelling a challenge
    /// raises the corresponding domain event.
    /// </summary>
    [Fact]
    public void Cancel_ShouldRaiseAuthenticationChallengeCancelledDomainEvent()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var cancelledAt =
            DateTime.UtcNow;

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            cancelledAt);

        // Assert
        var domainEvent =
            GetNewDomainEvents(
                challenge,
                before)
            .Should()
            .ContainSingle()
            .Subject
            .Should()
            .BeOfType<AuthenticationChallengeCancelledDomainEvent>()
            .Subject;

        domainEvent.AggregateId.Should()
            .Be(challenge.Id);

        domainEvent.UserId.Should()
            .Be(challenge.UserId);

        domainEvent.ChallengeType.Should()
            .Be(challenge.ChallengeType);

        domainEvent.Purpose.Should()
            .Be(challenge.Purpose);

        domainEvent.CancellationReason.Should()
            .Be(
                AuthenticationChallengeCancellationReason.UserCancelled);

        domainEvent.OccurredOn.Should()
            .Be(cancelledAt);
    }

    /// <summary>
    /// Verifies that a cancelled challenge
    /// cannot be cancelled again.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenAlreadyCancelled()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Cancel(
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.UtcNow.AddSeconds(1));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a completed challenge
    /// cannot be cancelled.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenCompleted()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Complete(
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Cancel(
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.UtcNow.AddSeconds(1));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a locked challenge
    /// cannot be cancelled.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenLocked()
    {
        // Arrange
        var challenge =
            CreateLockedChallenge(
                out var lockedAt);

        // Act
        var action = () =>
            challenge.Cancel(
                AuthenticationChallengeCancellationReason.UserCancelled,
                lockedAt.AddSeconds(1));

        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Locked);

        challenge.LockedAtUtc.Should()
            .Be(lockedAt);

        challenge.FailedAttemptCount.Should()
            .Be(1);

        challenge.CancelledAtUtc.Should()
            .BeNull();

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that an expired challenge
    /// cannot be cancelled.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenExpired()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                createdAt.AddMinutes(5));

        // Act
        var action = () =>
            challenge.Cancel(
                AuthenticationChallengeCancellationReason.UserCancelled,
                createdAt.AddMinutes(6));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that an undefined cancellation reason
    /// is rejected.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenReasonIsUndefined()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.Cancel(
                (AuthenticationChallengeCancellationReason)999,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Cancel_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.Cancel(
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Expire Test
    /// <summary>
    /// Verifies that expiring a pending challenge
    /// updates the aggregate state.
    /// </summary>
    [Fact]
    public void Expire_ShouldExpireChallenge()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            createdAt.AddMinutes(5);

        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                expiresAt);

        var expiredAt =
            expiresAt;

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Expire(
            expiredAt);

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Expired);

        var events =
            GetNewDomainEvents(
                challenge,
                before);

        events.Should()
            .ContainSingle(e =>
                e is AuthenticationChallengeExpiredDomainEvent);
    }

    /// <summary>
    /// Verifies that expiring a challenge
    /// raises the corresponding domain event.
    /// </summary>
    [Fact]
    public void Expire_ShouldRaiseAuthenticationChallengeExpiredDomainEvent()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            createdAt.AddMinutes(5);

        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                expiresAt);

        var before =
            challenge.DomainEvents.Count;

        // Act
        challenge.Expire(
            expiresAt);

        // Assert
        var domainEvent =
            GetNewDomainEvents(
                challenge,
                before)
            .Should()
            .ContainSingle()
            .Subject
            .Should()
            .BeOfType<AuthenticationChallengeExpiredDomainEvent>()
            .Subject;

        domainEvent.AggregateId.Should()
            .Be(challenge.Id);

        domainEvent.UserId.Should()
            .Be(challenge.UserId);

        domainEvent.ChallengeType.Should()
            .Be(challenge.ChallengeType);

        domainEvent.Purpose.Should()
            .Be(challenge.Purpose);

        domainEvent.OccurredOn.Should()
            .Be(expiresAt);
    }

    /// <summary>
    /// Verifies that a challenge cannot expire
    /// before its configured expiration time.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenExpirationHasNotBeenReached()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.Expire(
                DateTime.UtcNow);

        // Assert
        var exception =
            action.Should()
                .Throw<DomainException>()
                .Which;

        exception.ErrorCode.Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that an expired challenge
    /// cannot expire again.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenAlreadyExpired()
    {
        // Arrange
        var createdAt =
            DateTime.UtcNow;

        var expiresAt =
            createdAt.AddMinutes(5);

        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("ENCRYPTED_SECRET"),
                createdAt,
                expiresAt);

        challenge.Expire(
            expiresAt);

        // Act
        var action = () =>
            challenge.Expire(
                expiresAt.AddSeconds(1));

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a completed challenge
    /// cannot expire.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenCompleted()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Complete(
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Expire(
                challenge.ExpiresAtUtc);

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a cancelled challenge
    /// cannot expire.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenCancelled()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.Expire(
                challenge.ExpiresAtUtc);

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a locked challenge
    /// cannot expire.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenLocked()
    {
        // Arrange
        var challenge =
            CreateLockedChallenge(
                out var lockedAt);

        // Act
        var action = () =>
            challenge.Expire(
                lockedAt.AddMinutes(10));

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Locked);

        challenge.LockedAtUtc.Should()
            .Be(lockedAt);

        challenge.FailedAttemptCount.Should()
            .Be(1);

        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void Expire_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.Expire(
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }
    #endregion

    #region RegisterFailedAttempt Tests
    /// <summary>
    /// Verifies that registering a failed attempt
    /// increments the failed attempt count.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldIncrementFailedAttemptCount()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();
        var now =
            DateTime.UtcNow;

        // Act
        challenge.RegisterFailedAttempt(
            5,
            now);

        // Assert
        challenge.FailedAttemptCount.Should()
            .Be(1);

        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Pending);
    }

    /// <summary>
    /// Verifies that the challenge remains pending
    /// while the maximum failed attempts
    /// has not been reached.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldRemainPending_WhenThresholdHasNotBeenReached()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var now =
            DateTime.UtcNow;

        // Act
        challenge.RegisterFailedAttempt(
            3,
            now);

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Pending);

        challenge.LockedAtUtc.Should()
            .BeNull();
    }

    /// <summary>
    /// Verifies that reaching the maximum
    /// failed attempts locks the challenge.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldLockChallenge_WhenThresholdIsReached()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var lockedAt =
            DateTime.UtcNow;

        // Act
        challenge.RegisterFailedAttempt(
            1,
            lockedAt);

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Locked);

        challenge.LockedAtUtc.Should()
            .Be(lockedAt);

        challenge.FailedAttemptCount.Should()
            .Be(1);
    }

    /// <summary>
    /// Verifies that locking the challenge
    /// raises the corresponding domain event.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldRaiseLockedDomainEvent_WhenThresholdIsReached()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        var before =
            challenge.DomainEvents.Count;

        var lockedAt =
            DateTime.UtcNow;

        // Act
        challenge.RegisterFailedAttempt(
            1,
            lockedAt);

        // Assert
        var domainEvent =
            GetNewDomainEvents(
                challenge,
                before)
            .Should()
            .ContainSingle()
            .Subject
            .Should()
            .BeOfType<AuthenticationChallengeLockedDomainEvent>()
            .Subject;

        domainEvent.AggregateId.Should()
            .Be(challenge.Id);

        domainEvent.UserId.Should()
            .Be(challenge.UserId);

        domainEvent.ChallengeType.Should()
            .Be(challenge.ChallengeType);

        domainEvent.Purpose.Should()
            .Be(challenge.Purpose);

        domainEvent.OccurredOn.Should()
            .Be(lockedAt);

        domainEvent.FailedAttemptCount.Should()
            .Be(1);
    }

    /// <summary>
    /// Verifies that zero maximum attempts
    /// is rejected.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenMaximumAttemptsIsZero()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                0,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that a negative maximum
    /// attempts value is rejected.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenMaximumAttemptsIsNegative()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                -1,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that a completed challenge
    /// cannot register failed attempts.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenChallengeIsCompleted()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Complete(
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                5,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a cancelled challenge
    /// cannot register failed attempts.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenChallengeIsCancelled()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        challenge.Cancel(
            AuthenticationChallengeCancellationReason.UserCancelled,
            DateTime.UtcNow);

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                5,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a locked challenge
    /// cannot register additional failed attempts.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenChallengeIsLocked()
    {
        // Arrange
        var challenge =
            CreateLockedChallenge(
                out var lockedAt);

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                5,
                lockedAt.AddSeconds(1));

        // Assert
        challenge.Status.Should()
            .Be(AuthenticationChallengeStatus.Locked);
            
        challenge.FailedAttemptCount.Should()
            .Be(1);

        challenge.LockedAtUtc.Should()
            .Be(lockedAt);

        action.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be(
                IdentityDomainErrorCodes.InvalidState);
    }

    /// <summary>
    /// Verifies that a non-UTC timestamp
    /// is rejected.
    /// </summary>
    [Fact]
    public void RegisterFailedAttempt_ShouldThrow_WhenTimestampIsNotUtc()
    {
        // Arrange
        var challenge =
            CreatePendingChallenge();

        // Act
        var action = () =>
            challenge.RegisterFailedAttempt(
                5,
                DateTime.Now);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

}