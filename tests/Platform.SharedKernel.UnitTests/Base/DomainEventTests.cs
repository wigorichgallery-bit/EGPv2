using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

/// <summary>
/// Contains unit tests for the <see cref="DomainEvent"/> base class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies that <see cref="DomainEvent"/> correctly initializes its
/// immutable state and enforces constructor invariants.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify constructor initialization.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify constructor validation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify property values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify UTC <see cref="DateTime"/> preservation.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="DomainEvent"/> base class only.
/// </para>
/// </remarks>
/// </summary>
public sealed class DomainEventTests
{
    #region DomainEvent Constructor

    /// <summary>
    /// Verifies that the constructor initializes a
    /// <see cref="DomainEvent"/> using valid arguments.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="DomainEvent.AggregateId"/> equals the supplied aggregate identifier.</description></item>
    /// <item><description><see cref="DomainEvent.OccurredOn"/> equals the supplied timestamp.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied aggregate
    /// identifier is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>aggregateId</c> parameter.</description></item>
    /// <item><description>The validation message indicates that the GUID cannot be empty.</description></item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region DomainEvent Properties

    /// <summary>
    /// Verifies that <see cref="DomainEvent.AggregateId"/>
    /// returns the value supplied to the constructor.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The aggregate identifier is preserved.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="DomainEvent.OccurredOn"/>
    /// returns the value supplied to the constructor.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The occurrence timestamp is preserved.</description></item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that a UTC timestamp supplied to the constructor
    /// preserves its <see cref="DateTimeKind.Utc"/> value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="DomainEvent.OccurredOn"/> retains <see cref="DateTimeKind.Utc"/>.</description></item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region Test Infrastructure

    /// <summary>
    /// Exposes the protected constructor of <see cref="DomainEvent"/>
    /// for unit testing.
    ///
    /// <remarks>
    /// <para>
    /// Purpose:
    /// Allows the abstract <see cref="DomainEvent"/> base class to be
    /// instantiated within unit tests.
    /// </para>
    ///
    /// <para>
    /// Scope:
    /// Test infrastructure only. This type must never be referenced by
    /// production code.
    /// </para>
    /// </remarks>
    /// </summary>
    private sealed class TestDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDomainEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">
        /// The identifier of the aggregate that raised the domain event.
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