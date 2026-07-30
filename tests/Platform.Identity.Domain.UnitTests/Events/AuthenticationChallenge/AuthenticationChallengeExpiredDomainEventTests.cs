// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/
// AuthenticationChallengeExpiredDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeExpiredDomainEvent"/>.
/// </summary>
public sealed class AuthenticationChallengeExpiredDomainEventTests
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
            new AuthenticationChallengeExpiredDomainEvent(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(challengeId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.UserId.Should().Be(userId);
        domainEvent.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);
        domainEvent.Purpose.Should()
            .Be(AuthenticationChallengePurpose.EmailVerification);
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
            new AuthenticationChallengeExpiredDomainEvent(
                Guid.NewGuid(),
                Guid.Empty,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
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
            new AuthenticationChallengeExpiredDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (AuthenticationChallengeType)999,
                AuthenticationChallengePurpose.EmailVerification,
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
            new AuthenticationChallengeExpiredDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                (AuthenticationChallengePurpose)999,
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
        typeof(AuthenticationChallengeExpiredDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}