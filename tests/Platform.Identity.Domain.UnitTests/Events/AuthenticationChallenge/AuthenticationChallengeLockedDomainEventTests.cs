// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/
// AuthenticationChallengeLockedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeLockedDomainEvent"/>.
/// </summary>
public sealed class AuthenticationChallengeLockedDomainEventTests
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

        const int failedAttempts = 5;

        // Act
        var domainEvent =
            new AuthenticationChallengeLockedDomainEvent(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                failedAttempts,
                occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(challengeId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.UserId.Should().Be(userId);
        domainEvent.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);
        domainEvent.Purpose.Should()
            .Be(AuthenticationChallengePurpose.EmailVerification);
        domainEvent.FailedAttemptCount.Should().Be(failedAttempts);
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
            new AuthenticationChallengeLockedDomainEvent(
                Guid.NewGuid(),
                Guid.Empty,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                3,
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
            new AuthenticationChallengeLockedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (AuthenticationChallengeType)999,
                AuthenticationChallengePurpose.EmailVerification,
                3,
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
            new AuthenticationChallengeLockedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                (AuthenticationChallengePurpose)999,
                3,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a failed attempt count
    /// of zero is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenFailedAttemptCountIsZero()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeLockedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                0,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that a negative failed attempt
    /// count is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenFailedAttemptCountIsNegative()
    {
        // Arrange

        // Act
        var action = () =>
            new AuthenticationChallengeLockedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                -1,
                DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>();
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
        typeof(AuthenticationChallengeLockedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}