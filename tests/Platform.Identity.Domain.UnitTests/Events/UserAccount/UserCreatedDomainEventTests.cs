// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/UserCreatedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="UserCreatedDomainEvent"/>.
/// </summary>
public sealed class UserCreatedDomainEventTests
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
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;
        const string username = "john.doe";
        const string email = "john@example.com";

        // Act
        var domainEvent =
            new UserCreatedDomainEvent(
                aggregateId,
                occurredOn,
                username,
                email);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.Username.Should().Be(username);
        domainEvent.Email.Should().Be(email);
    }

    #endregion

    #region Username Validation Tests

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the username is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenUsernameIsNull()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new UserCreatedDomainEvent(
                aggregateId,
                occurredOn,
                null!,
                "user@example.com");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the username is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenUsernameIsInvalid(
        string username)
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new UserCreatedDomainEvent(
                aggregateId,
                occurredOn,
                username,
                "user@example.com");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Email Validation Tests

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the email is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenEmailIsNull()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new UserCreatedDomainEvent(
                aggregateId,
                occurredOn,
                "john.doe",
                null!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the email is empty or whitespace.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenEmailIsInvalid(
        string email)
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new UserCreatedDomainEvent(
                aggregateId,
                occurredOn,
                "john.doe",
                email);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Inheritance Tests

    /// <summary>
    /// Verifies that the domain event derives
    /// from the DomainEvent base class.
    /// </summary>
    [Fact]
    public void ShouldInheritFromDomainEvent()
    {
        // Arrange

        // Act

        // Assert
        typeof(UserCreatedDomainEvent)
            .BaseType!
            .Name
            .Should()
            .Be("DomainEvent");
    }

    #endregion
}