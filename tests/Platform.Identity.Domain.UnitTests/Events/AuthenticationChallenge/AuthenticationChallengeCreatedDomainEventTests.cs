// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/
// AuthenticationChallengeCreatedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ErrorCodes;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeCreatedDomainEvent"/>.
/// </summary>
public sealed class AuthenticationChallengeCreatedDomainEventTests
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
        var expiresAtUtc = occurredOn.AddMinutes(5);

        // Act
        var domainEvent =
            new AuthenticationChallengeCreatedDomainEvent(
                challengeId,
                userId,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                expiresAtUtc);

        // Assert
        domainEvent.AggregateId.Should().Be(challengeId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.UserId.Should().Be(userId);
        domainEvent.ChallengeType.Should()
            .Be(AuthenticationChallengeType.EmailOtp);
        domainEvent.Purpose.Should()
            .Be(AuthenticationChallengePurpose.EmailVerification);
        domainEvent.ExpiresAtUtc.Should().Be(expiresAtUtc);
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
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.Empty,
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                occurredOn.AddMinutes(5));

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
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (AuthenticationChallengeType)999,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                occurredOn.AddMinutes(5));

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
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                (AuthenticationChallengePurpose)999,
                occurredOn,
                occurredOn.AddMinutes(5));

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a non-UTC expiration time
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenExpirationIsNotUtc()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;
        var expiresAt = new DateTime(
            2030,
            1,
            1,
            12,
            0,
            0,
            DateTimeKind.Local);

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                expiresAt);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an expiration timestamp
    /// equal to the occurrence time is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenExpirationEqualsOccurredOn()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                occurredOn);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.InvalidChallengeExpiration);
    }

    /// <summary>
    /// Verifies that an expiration timestamp
    /// earlier than the occurrence time is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenExpirationIsEarlierThanOccurredOn()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new AuthenticationChallengeCreatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
                AuthenticationChallengePurpose.EmailVerification,
                occurredOn,
                occurredOn.AddMinutes(-1));

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be(IdentityDomainErrorCodes.InvalidChallengeExpiration);
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
        typeof(AuthenticationChallengeCreatedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}