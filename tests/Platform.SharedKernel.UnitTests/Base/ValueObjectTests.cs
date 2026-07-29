using FluentAssertions;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.UnitTests.TestHelpers;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

/// <summary>
/// Contains unit tests for the <see cref="ValueObject"/> base class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the equality semantics implemented by
/// <see cref="ValueObject"/>, including value equality,
/// hash code generation, and equality operators.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify equality based on atomic values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify hash code consistency for equal and different instances.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify the equality and inequality operators.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="ValueObject"/> base class only.
/// </para>
/// </remarks>
/// </summary>
public sealed class ValueObjectTests
{
    #region ValueObject.Equals()

    /// <summary>
    /// Verifies that two value objects containing identical atomic values
    /// are considered equal.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="object.Equals(object?)"/> returns
    /// <see langword="true"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("ABC", 10);

        // Act

        // Assert
        left.Equals(right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two value objects containing different atomic values
    /// are not considered equal.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="object.Equals(object?)"/> returns
    /// <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("XYZ", 10);

        // Act

        // Assert
        left.Equals(right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that value objects of different runtime types are never
    /// considered equal, even when their contained values are identical.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Equality comparison returns
    /// <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new OtherValueObject("ABC");

        // Act

        // Assert
        left.Equals(right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing a value object with
    /// <see langword="null"/> returns
    /// <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Equality comparison returns
    /// <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);

        // Act

        // Assert
        left.Equals(null).Should().BeFalse();
    }

    #endregion

    #region ValueObject.GetHashCode()
    /// <summary>
    /// Verifies that two equal value objects produce identical hash codes.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Both value objects produce the same hash code.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void GetHashCode_WithEqualObjects_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("ABC", 10);

        // Act

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that value objects containing different atomic values
    /// produce different hash codes.
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
    public void GetHashCode_WithDifferentObjects_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("XYZ", 20);

        // Act

        // Assert
        left.GetHashCode().Should().NotBe(right.GetHashCode());
    }

    #endregion

    #region ValueObject.operator ==

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="true"/> when two value objects contain identical
    /// atomic values.
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
    public void EqualityOperator_WithEqualObjects_ShouldReturnTrue()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("ABC", 10);

        // Act

        // Assert
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="false"/> when two value objects contain different
    /// atomic values.
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
    public void EqualityOperator_WithDifferentObjects_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("XYZ", 10);

        // Act

        // Assert
        (left == right).Should().BeFalse();
    }

    #endregion

    #region ValueObject.operator !=

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="false"/> when two value objects contain identical
    /// atomic values.
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
    public void InequalityOperator_WithEqualObjects_ShouldReturnFalse()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("ABC", 10);

        // Act

        // Assert
        (left != right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="true"/> when two value objects contain different
    /// atomic values.
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
    public void InequalityOperator_WithDifferentObjects_ShouldReturnTrue()
    {
        // Arrange
        var left = new TestValueObject("ABC", 10);
        var right = new TestValueObject("XYZ", 10);

        // Act

        // Assert
        (left != right).Should().BeTrue();
    }

    #endregion
}