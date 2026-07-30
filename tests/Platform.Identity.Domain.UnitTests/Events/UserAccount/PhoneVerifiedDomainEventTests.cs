// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/PhoneVerifiedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="PhoneVerifiedDomainEvent"/>.
/// </summary>
public sealed class PhoneVerifiedDomainEventTests
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
        const string phoneNumber = "+6281234567890";

        // Act
        var domainEvent =
            new PhoneVerifiedDomainEvent(
                userId,
                occurredOn,
                phoneNumber);

        // Assert
        domainEvent.AggregateId.Should().Be(userId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.PhoneNumber.Should().Be(phoneNumber);
    }

    #endregion

    #region Phone Number Validation Tests

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="ArgumentNullException"/>
    /// when the phone number is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenPhoneNumberIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () =>
            new PhoneVerifiedDomainEvent(
                userId,
                occurredOn,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("phoneNumber");
    }

    /// <summary>
    /// Verifies that an empty phone number
    /// is preserved because the production
    /// implementation only rejects null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAccept_EmptyPhoneNumber()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent =
            new PhoneVerifiedDomainEvent(
                userId,
                occurredOn,
                string.Empty);

        // Assert
        domainEvent.PhoneNumber.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that a whitespace-only phone
    /// number is preserved because the production
    /// implementation only rejects null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAccept_WhitespacePhoneNumber()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        const string phoneNumber = "   ";

        // Act
        var domainEvent =
            new PhoneVerifiedDomainEvent(
                userId,
                occurredOn,
                phoneNumber);

        // Assert
        domainEvent.PhoneNumber.Should().Be(phoneNumber);
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
        typeof(PhoneVerifiedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}