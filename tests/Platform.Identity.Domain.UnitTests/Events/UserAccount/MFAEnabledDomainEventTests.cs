// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/MFAEnabledDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="MFAEnabledDomainEvent"/>.
/// </summary>
public sealed class MFAEnabledDomainEventTests
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
        var method = MFAMethod.TOTP;

        // Act
        var domainEvent =
            new MFAEnabledDomainEvent(
                aggregateId,
                occurredOn,
                method);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.Method.Should().Be(method);
    }

    #endregion

    #region Method Tests

    /// <summary>
    /// Verifies that every supported MFA method
    /// can be stored by the domain event.
    /// </summary>
    [Theory]
    [InlineData(MFAMethod.None)]
    [InlineData(MFAMethod.TOTP)]
    [InlineData(MFAMethod.Email)]
    [InlineData(MFAMethod.SMS)]
    [InlineData(MFAMethod.WhatsApp)]
    public void Constructor_ShouldStoreMethod(
        MFAMethod method)
    {
        // Arrange

        // Act
        var domainEvent =
            new MFAEnabledDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                method);

        // Assert
        domainEvent.Method.Should().Be(method);
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
        typeof(MFAEnabledDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}