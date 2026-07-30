// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Events/PasswordChangedDomainEventTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Events;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Events;

/// <summary>
/// Contains unit tests for
/// <see cref="PasswordChangedDomainEvent"/>.
/// </summary>
public sealed class PasswordChangedDomainEventTests
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
        const int passwordVersion = 5;

        // Act
        var domainEvent =
            new PasswordChangedDomainEvent(
                aggregateId,
                occurredOn,
                passwordVersion);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
        domainEvent.PasswordVersion.Should().Be(passwordVersion);
    }

    #endregion

    #region Password Version Tests

    /// <summary>
    /// Verifies that the password version
    /// supplied to the constructor is preserved.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Constructor_ShouldStorePasswordVersion(
        int passwordVersion)
    {
        // Arrange

        // Act
        var domainEvent =
            new PasswordChangedDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                passwordVersion);

        // Assert
        domainEvent.PasswordVersion.Should().Be(passwordVersion);
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
        typeof(PasswordChangedDomainEvent)
            .Should()
            .BeAssignableTo<DomainEvent>();
    }

    #endregion
}