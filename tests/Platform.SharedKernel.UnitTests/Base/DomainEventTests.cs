using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

public sealed class DomainEventTests
{
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateDomainEvent()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent = new TestDomainEvent(
            aggregateId,
            occurredOn);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
        domainEvent.OccurredOn.Should().Be(occurredOn);
    }

    [Fact]
    public void Constructor_WithEmptyAggregateId_ShouldThrowArgumentException()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;

        // Act
        var action = () => new TestDomainEvent(
            Guid.Empty,
            occurredOn);

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("aggregateId")
            .Which;

        exception.Message.Should().Contain("Guid cannot be empty.");
    }

    [Fact]
    public void AggregateId_ShouldReturnConstructorValue()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent(
            aggregateId,
            DateTime.UtcNow);

        // Assert
        domainEvent.AggregateId.Should().Be(aggregateId);
    }

    [Fact]
    public void OccurredOn_ShouldReturnConstructorValue()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent = new TestDomainEvent(
            Guid.NewGuid(),
            occurredOn);

        // Assert
        domainEvent.OccurredOn.Should().Be(occurredOn);
    }

    [Fact]
    public void Constructor_WithUtcDateTime_ShouldPreserveUtcKind()
    {
        // Arrange
        var occurredOn = DateTime.UtcNow;

        // Act
        var domainEvent = new TestDomainEvent(
            Guid.NewGuid(),
            occurredOn);

        // Assert
        domainEvent.OccurredOn.Kind.Should().Be(DateTimeKind.Utc);
    }

    private sealed class TestDomainEvent : DomainEvent
    {
        public TestDomainEvent(
            Guid aggregateId,
            DateTime occurredOn)
            : base(
                aggregateId,
                occurredOn)
        {
        }
    }
}