// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/UserLockedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="UserLockedDomainEvent"/>.
/// </summary>
public sealed class UserLockedDomainEventTests
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
        var lockoutUntil = occurredOn.AddMinutes(30);

        // Act
        var domainEvent =
            new UserLockedDomainEvent(
                aggregateId,
                occurredOn,
                lockoutUntil);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.LockoutUntil.Should().Be(lockoutUntil);
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the lockout expiration
    /// timestamp is preserved.
    /// </summary>
    [Fact]
    public void LockoutUntil_ShouldMatchConstructorValue()
    {
        // Arrange
        var expected = DateTime.UtcNow.AddHours(1);

        // Act
        var domainEvent =
            new UserLockedDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                expected);

        // Assert
        domainEvent.LockoutUntil.Should().Be(expected);
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
        typeof(UserLockedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}