using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

/// <summary>
/// Contains unit tests for the <see cref="AggregateRoot"/> base class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the domain event management behavior implemented by
/// <see cref="AggregateRoot"/>, including initialization,
/// event registration, validation, and event collection exposure.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify domain event collection initialization.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify adding and clearing domain events.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify constructor and domain event validation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify read-only exposure of the domain event collection.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="AggregateRoot"/> base class only.
/// </para>
/// </remarks>
/// </summary>
public sealed class AggregateRootTests
{
    #region AggregateRoot Constructor

    /// <summary>
    /// Verifies that a newly created <see cref="AggregateRoot"/>
    /// initializes an empty domain event collection.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="AggregateRoot.DomainEvents"/> is initially empty.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_ShouldInitializeEmptyDomainEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Act

        // Assert
        aggregate.DomainEvents.Should().BeEmpty();
    }

    #endregion

    #region AggregateRoot.AddDomainEvent()

    /// <summary>
    /// Verifies that a valid domain event is added to the aggregate.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The domain event collection contains one event.</description></item>
    /// <item><description>The added event instance is preserved.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that adding a
    /// <see langword="null"/> domain event throws an
    /// <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentNullException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>domainEvent</c> parameter.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that adding a domain event belonging to a different
    /// aggregate throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="InvalidOperationException"/> is thrown.</description></item>
    /// <item><description>The exception explains that aggregate identifiers must match.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that multiple valid domain events are retained by the aggregate.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The domain event collection contains all added events.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void AddDomainEvent_WithMultipleEvents_ShouldContainAllEvents()
    {
        // Arrange
        var id = Guid.NewGuid();

        var aggregate = new TestAggregateRoot(id);

        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));
        aggregate.Add(new TestDomainEvent(id, DateTime.UtcNow));

        // Act

        // Assert
        aggregate.DomainEvents.Should().HaveCount(2);
    }

    #endregion

    #region AggregateRoot.ClearDomainEvents()
    /// <summary>
    /// Verifies that clearing the domain event collection removes all
    /// previously registered domain events.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="AggregateRoot.DomainEvents"/> becomes empty.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region AggregateRoot.DomainEvents

    /// <summary>
    /// Verifies that the exposed domain event collection is read-only.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="AggregateRoot.DomainEvents"/> is exposed as an
    /// <see cref="IReadOnlyCollection{T}"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid());

        // Act

        // Assert
        aggregate.DomainEvents.Should()
            .BeAssignableTo<IReadOnlyCollection<DomainEvent>>();
    }

    /// <summary>
    /// Verifies that the domain event collection exposes the exact event
    /// instances added to the aggregate.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The exposed event instance is the same object that was added.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region Test Infrastructure

    /// <summary>
    /// Exposes the protected members of <see cref="AggregateRoot"/>
    /// for unit testing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Purpose:
    /// Provides a concrete implementation that allows protected
    /// domain event APIs to be exercised during unit tests.
    /// </para>
    ///
    /// <para>
    /// Scope:
    /// Test infrastructure only. This type must never be referenced
    /// by production code.
    /// </para>
    /// </remarks>
    private sealed class TestAggregateRoot : AggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TestAggregateRoot"/> class.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the aggregate.
        /// </param>
        public TestAggregateRoot(Guid id)
            : base(id)
        {
        }

        /// <summary>
        /// Adds the specified domain event to the aggregate.
        /// </summary>
        /// <param name="domainEvent">
        /// The domain event to register.
        /// </param>
        public void Add(DomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }

        /// <summary>
        /// Removes all registered domain events from the aggregate.
        /// </summary>
        public void Clear()
        {
            ClearDomainEvents();
        }
    }

    /// <summary>
    /// Provides a concrete implementation of <see cref="DomainEvent"/>
    /// for AggregateRoot unit tests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Purpose:
    /// Allows verification of domain event registration and validation
    /// performed by <see cref="AggregateRoot"/>.
    /// </para>
    ///
    /// <para>
    /// Scope:
    /// Test infrastructure only. This type must never be referenced
    /// by production code.
    /// </para>
    /// </remarks>
    private sealed class TestDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TestDomainEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">
        /// The identifier of the aggregate that raised the event.
        /// </param>
        /// <param name="occurredOn">
        /// The UTC timestamp indicating when the event occurred.
        /// </param>
        public TestDomainEvent(
            Guid aggregateId,
            DateTime occurredOn)
            : base(
                aggregateId,
                occurredOn)
        {
        }
    }

    #endregion
}