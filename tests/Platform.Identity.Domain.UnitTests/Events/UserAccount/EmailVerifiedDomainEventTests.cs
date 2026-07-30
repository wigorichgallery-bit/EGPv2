// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/EmailVerifiedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="EmailVerifiedDomainEvent"/>.
/// </summary>
public sealed class EmailVerifiedDomainEventTests
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
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;
        const string email = "john.doe@example.com";

        // Act
        var domainEvent =
            new EmailVerifiedDomainEvent(
                userId,
                occurredOn,
                email);

        // Assert
        domainEvent.AggregateId.Should().Be(userId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.Email.Should().Be(email);
    }

    #endregion

    #region Email Validation Tests

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="ArgumentNullException"/>
    /// when the email is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenEmailIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new EmailVerifiedDomainEvent(
                userId,
                occurredOn,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("email");
    }

    /// <summary>
    /// Verifies that an empty email
    /// is accepted because the production
    /// implementation only rejects null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAccept_EmptyEmail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent =
            new EmailVerifiedDomainEvent(
                userId,
                occurredOn,
                string.Empty);

        // Assert
        domainEvent.Email.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that a whitespace-only email
    /// is accepted because the production
    /// implementation only rejects null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAccept_WhitespaceEmail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;
        const string email = "   ";

        // Act
        var domainEvent =
            new EmailVerifiedDomainEvent(
                userId,
                occurredOn,
                email);

        // Assert
        domainEvent.Email.Should().Be(email);
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
        typeof(EmailVerifiedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}