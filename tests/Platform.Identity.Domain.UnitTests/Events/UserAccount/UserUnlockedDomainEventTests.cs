// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/UserUnlockedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="UserUnlockedDomainEvent"/>.
/// </summary>
public sealed class UserUnlockedDomainEventTests
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
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent =
            new UserUnlockedDomainEvent(
                aggregateId,
                occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
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
        typeof(UserUnlockedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}