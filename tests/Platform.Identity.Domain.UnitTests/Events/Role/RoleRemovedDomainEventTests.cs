// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/RoleRemovedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="RoleRemovedDomainEvent"/>.
/// </summary>
public sealed class RoleRemovedDomainEventTests
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
        var roleId = Guid.NewGuid();

        // Act
        var domainEvent =
            new RoleRemovedDomainEvent(
                aggregateId,
                occurredOn,
                roleId);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.RoleId.Should().Be(roleId);
    }

    #endregion

    #region Role Identifier Tests

    /// <summary>
    /// Verifies that the supplied role identifier
    /// is preserved by the domain event.
    /// </summary>
    [Fact]
    public void Constructor_ShouldStoreRoleId()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        // Act
        var domainEvent =
            new RoleRemovedDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                roleId);

        // Assert
        domainEvent.RoleId.Should().Be(roleId);
    }

    /// <summary>
    /// Verifies that an empty role identifier
    /// is preserved because the production
    /// implementation performs no validation.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAcceptEmptyRoleId()
    {
        // Arrange
        var roleId = Guid.Empty;

        // Act
        var domainEvent =
            new RoleRemovedDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                roleId);

        // Assert
        domainEvent.RoleId.Should().Be(Guid.Empty);
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
        typeof(RoleRemovedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}