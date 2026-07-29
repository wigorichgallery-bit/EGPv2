using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

public sealed class AggregateRootTests
{
    [Fact]
    public void Constructor_ShouldInitializeEmptyDomainEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Assert
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AddDomainEvent_WithValidEvent_ShouldAddEvent()
    {
        // Arrange
        var id = Guid.NewGuid();

        var aggregate = new TestAggregateRoot(id);

        var domainEvent = new TestDomainEvent(
            id,
            DateTime.UtcNow);

        // Act
        aggregate.Add(domainEvent);

        // Assert
        aggregate.DomainEvents.Should().ContainSingle();
        aggregate.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void AddDomainEvent_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Act
        var action = () => aggregate.Add(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("domainEvent");
    }

    [Fact]
    public void AddDomainEvent_WithDifferentAggregateId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        var domainEvent = new TestDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow);

        // Act
        var action = () => aggregate.Add(domainEvent);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("DomainEvent AggregateId must match the AggregateRoot identifier.");
    }

    [Fact]
    public void AddDomainEvent_WithMultipleEvents_ShouldContainAllEvents()
    {
        // Arrange
        var id = Guid.NewGuid();

        var aggregate = new TestAggregateRoot(id);

        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));
        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));

        // Assert
        aggregate.DomainEvents.Should().HaveCount(2);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var id = Guid.NewGuid();

        var aggregate = new TestAggregateRoot(id);

        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));
        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));

        // Act
        aggregate.Clear();

        // Assert
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Assert
        aggregate.DomainEvents.Should().BeAssignableTo<IReadOnlyCollection<DomainEvent>>();
    }

    [Fact]
    public void DomainEvents_ShouldExposeAddedEvents()
    {
        // Arrange
        var id = Guid.NewGuid();

        var aggregate = new TestAggregateRoot(id);

        var domainEvent = new TestDomainEvent(
            id,
            DateTime.UtcNow);

        // Act
        aggregate.Add(domainEvent);

        // Assert
        aggregate.DomainEvents.First().Should().BeSameAs(domainEvent);
    }

    private sealed class TestAggregateRoot : AggregateRoot
    {
        public TestAggregateRoot(Guid id)
            : base(id)
        {
        }

        public void Add(DomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }

        public void Clear()
        {
            ClearDomainEvents();
        }
    }

    private sealed class TestDomainEvent : DomainEvent
    {
        public TestDomainEvent(
            Guid aggregateId,
            DateTime occurredOn)
            : base(aggregateId, occurredOn)
        {
        }
    }
}