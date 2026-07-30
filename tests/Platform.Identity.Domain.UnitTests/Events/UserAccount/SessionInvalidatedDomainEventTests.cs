// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/SessionInvalidatedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="SessionInvalidatedDomainEvent"/>.
/// </summary>
public sealed class SessionInvalidatedDomainEventTests
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
        const string reason = "Password changed";

        // Act
        var domainEvent =
            new SessionInvalidatedDomainEvent(
                aggregateId,
                occurredOn,
                reason);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.Reason.Should().Be(reason);
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the invalidation reason
    /// is preserved.
    /// </summary>
    [Fact]
    public void Reason_ShouldMatchConstructorValue()
    {
        // Arrange
        const string expected = "Role modified";

        // Act
        var domainEvent =
            new SessionInvalidatedDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                expected);

        // Assert
        domainEvent.Reason.Should().Be(expected);
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
        typeof(SessionInvalidatedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}