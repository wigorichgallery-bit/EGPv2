using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

/// <summary>
/// Contains unit tests for the <see cref="BaseEntity"/> base class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the identity-based equality semantics implemented by
/// <see cref="BaseEntity"/>, including constructor validation,
/// equality comparison, hash code generation, and equality operators.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify constructor initialization and validation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify equality based on entity identity.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify hash code consistency.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify equality and inequality operators.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="BaseEntity"/> base class only.
/// </para>
/// </remarks>
/// </summary>
public sealed class BaseEntityTests
{
    #region BaseEntity Constructor

    /// <summary>
    /// Verifies that the constructor initializes a
    /// <see cref="BaseEntity"/> using a valid identifier.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="BaseEntity.Id"/> equals the supplied identifier.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithValidId_ShouldCreateEntity()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new TestEntity(id);

        // Assert
        entity.Id.Should().Be(id);
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied identifier
    /// is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>id</c> parameter.</description></item>
    /// <item><description>The validation message indicates that the GUID cannot be empty.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new TestEntity(Guid.Empty);

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("id")
            .Which;

        exception.Message.Should().Contain("Guid cannot be empty.");
    }

    #endregion

    #region BaseEntity.Equals()

    /// <summary>
    /// Verifies that two entities of the same runtime type having the same
    /// identifier are considered equal.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="object.Equals(object?)"/> returns <see langword="true"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithSameTypeAndSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        // Act

        // Assert
        left.Equals(right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two entities of the same runtime type having different
    /// identifiers are not considered equal.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="object.Equals(object?)"/> returns <see langword="false"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithSameTypeAndDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        // Act

        // Assert
        left.Equals(right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that entities of different runtime types are never considered
    /// equal, even when their identifiers are identical.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="object.Equals(object?)"/> returns <see langword="false"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new OtherEntity(id);

        // Act

        // Assert
        left.Equals(right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing an entity with
    /// <see langword="null"/> returns
    /// <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>Equality comparison returns <see langword="false"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act

        // Assert
        entity.Equals(null).Should().BeFalse();
    }

    #endregion

    #region BaseEntity.GetHashCode()
    /// <summary>
    /// Verifies that two equal entities produce identical hash codes.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Both entities produce the same hash code.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void GetHashCode_WithEqualEntities_ShouldReturnSameHashCode()
    {
        // Arrange
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        // Act

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that two entities having different identifiers produce
    /// different hash codes.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The generated hash codes are different.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void GetHashCode_WithDifferentEntities_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        // Act

        // Assert
        left.GetHashCode().Should().NotBe(right.GetHashCode());
    }

    #endregion

    #region BaseEntity.operator ==

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="true"/> when two entities have the same
    /// runtime type and identifier.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The equality operator evaluates to
    /// <see langword="true"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void EqualityOperator_WithEqualEntities_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        // Act

        // Assert
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="false"/> when two entities have different
    /// identifiers.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The equality operator evaluates to
    /// <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void EqualityOperator_WithDifferentEntities_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        // Act

        // Assert
        (left == right).Should().BeFalse();
    }

    #endregion

    #region BaseEntity.operator !=

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="false"/> when two entities have the same
    /// runtime type and identifier.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The inequality operator evaluates to
    /// <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void InequalityOperator_WithEqualEntities_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        // Act

        // Assert
        (left != right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="true"/> when two entities have different
    /// identifiers.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The inequality operator evaluates to
    /// <see langword="true"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void InequalityOperator_WithDifferentEntities_ShouldReturnTrue()
    {
        // Arrange
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        // Act

        // Assert
        (left != right).Should().BeTrue();
    }

    #endregion

    #region Test Infrastructure

    /// <summary>
    /// Exposes the protected constructor of <see cref="BaseEntity"/>
    /// for unit testing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Purpose:
    /// Provides a concrete implementation of <see cref="BaseEntity"/>
    /// so that its identity semantics can be verified.
    /// </para>
    ///
    /// <para>
    /// Scope:
    /// Test infrastructure only. This type must never be referenced by
    /// production code.
    /// </para>
    /// </remarks>
    private sealed class TestEntity : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntity"/> class.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the entity.
        /// </param>
        public TestEntity(Guid id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Provides an alternative concrete implementation of
    /// <see cref="BaseEntity"/> for runtime type comparison tests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Purpose:
    /// Verifies that entity equality requires both the same identifier
    /// and the same runtime type.
    /// </para>
    ///
    /// <para>
    /// Scope:
    /// Test infrastructure only. This type must never be referenced by
    /// production code.
    /// </para>
    /// </remarks>
    private sealed class OtherEntity : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OtherEntity"/> class.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the entity.
        /// </param>
        public OtherEntity(Guid id)
            : base(id)
        {
        }
    }

    #endregion
}