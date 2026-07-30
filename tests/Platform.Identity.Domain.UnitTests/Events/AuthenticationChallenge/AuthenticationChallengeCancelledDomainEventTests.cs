// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/
// AuthenticationChallengeCancelledDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeCancelledDomainEvent"/>.
/// </summary>
public sealed class AuthenticationChallengeCancelledDomainEventTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that the constructor initializes
    /// every property correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var challengeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent =
            new AuthenticationChallengeCancelledDomainEvent(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                AuthenticationChallengeCancellationReason.UserCancelled,
                occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(challengeId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.UserId.Should().Be(userId);
        domainEvent.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);
        domainEvent.Purpose.Should()
            .Be(AuthenticationChallengePurpose.EmailVerification);
        domainEvent.CancellationReason.Should()
            .Be(AuthenticationChallengeCancellationReason.UserCancelled);
    }

    #endregion

    #region Guard Clause Tests

    /// <summary>
    /// Verifies that an empty user identifier
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenUserIdIsEmpty()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeCancelledDomainEvent(
                Guid.NewGuid(),
                Guid.Empty,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an undefined challenge type
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenChallengeTypeIsUndefined()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeCancelledDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (AuthenticationChallengeType)999,
                AuthenticationChallengePurpose.EmailVerification,
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an undefined purpose
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenPurposeIsUndefined()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeCancelledDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                (AuthenticationChallengePurpose)999,
                AuthenticationChallengeCancellationReason.UserCancelled,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an undefined cancellation reason
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenCancellationReasonIsUndefined()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeCancelledDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                (AuthenticationChallengeCancellationReason)999,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Inheritance Tests

    /// <summary>
    /// Verifies that the domain event derives
    /// from <see cref="DomainEvent"/>.
    /// </summary>
    [Fact]
    public void ShouldInheritFromDomainEvent()
    {
        // Arrange

        // Act

        // Assert
        typeof(AuthenticationChallengeCancelledDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}