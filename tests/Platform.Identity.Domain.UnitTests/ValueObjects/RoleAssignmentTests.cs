// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/RoleAssignmentTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="RoleAssignment"/>
/// value object.
/// </summary>
public sealed class RoleAssignmentTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that a valid role identifier creates a new
    /// <see cref="RoleAssignment"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenRoleIdIsValid_ShouldCreateInstance()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        // Act
        var assignment = new RoleAssignment(roleId);

        // Assert
        assignment.RoleId.Should().Be(roleId);
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the role identifier is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenRoleIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var roleId = Guid.Empty;

        // Act
        Action act = () => new RoleAssignment(roleId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("roleId");
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the assigned role identifier is preserved.
    /// </summary>
    [Fact]
    public void RoleId_WhenConstructed_ShouldReturnOriginalValue()
    {
        // Arrange
        var roleId = Guid.NewGuid();

        // Act
        var assignment = new RoleAssignment(roleId);

        // Assert
        assignment.RoleId.Should().Be(roleId);
    }

    /// <summary>
    /// Verifies that the <see cref="RoleAssignment.RoleId"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void RoleId_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(RoleAssignment)
            .GetProperty(nameof(RoleAssignment.RoleId));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same role identifier
    /// are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenRoleIdsAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var left = new RoleAssignment(roleId);
        var right = new RoleAssignment(roleId);

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different role identifiers
    /// are not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenRoleIdsAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new RoleAssignment(Guid.NewGuid());
        var right = new RoleAssignment(Guid.NewGuid());

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing with <see langword="null"/>
    /// returns <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Equals_WhenComparedWithNull_ShouldReturnFalse()
    {
        // Arrange
        var assignment = new RoleAssignment(Guid.NewGuid());

        // Act
        var result = assignment.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing with an object of a different type
    /// returns <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Equals_WhenComparedWithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var assignment = new RoleAssignment(Guid.NewGuid());

        // Act
        var result = assignment.Equals(new object());

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="true"/> for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_WhenRoleIdsAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var left = new RoleAssignment(roleId);
        var right = new RoleAssignment(roleId);

        // Act
        var result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="false"/> for different values.
    /// </summary>
    [Fact]
    public void EqualityOperator_WhenRoleIdsAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new RoleAssignment(Guid.NewGuid());
        var right = new RoleAssignment(Guid.NewGuid());

        // Act
        var result = left == right;

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="true"/> for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_WhenRoleIdsAreDifferent_ShouldReturnTrue()
    {
        // Arrange
        var left = new RoleAssignment(Guid.NewGuid());
        var right = new RoleAssignment(Guid.NewGuid());

        // Act
        var result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="false"/> for equal values.
    /// </summary>
    [Fact]
    public void InequalityOperator_WhenRoleIdsAreEqual_ShouldReturnFalse()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var left = new RoleAssignment(roleId);
        var right = new RoleAssignment(roleId);

        // Act
        var result = left != right;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal role assignments produce the same
    /// hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenRoleIdsAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var left = new RoleAssignment(roleId);
        var right = new RoleAssignment(roleId);

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion
}