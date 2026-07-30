// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/TotpSecretSetDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="TotpSecretSetDomainEvent"/>.
/// </summary>
public sealed class TotpSecretSetDomainEventTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that the constructor initializes
    /// the inherited properties correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent =
            new TotpSecretSetDomainEvent(
                userId,
                occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(userId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
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
        typeof(TotpSecretSetDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}