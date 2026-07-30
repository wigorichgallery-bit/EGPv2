// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/PermissionIdTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="PermissionId"/>
/// value object.
/// </summary>
public sealed class PermissionIdTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that a valid permission identifier creates a new
    /// <see cref="PermissionId"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateInstance()
    {
        // Arrange
        const string permission = "USER.CREATE";

        // Act
        var permissionId = new PermissionId(permission);

        // Assert
        permissionId.Value.Should().Be(permission);
    }

    /// <summary>
    /// Verifies that the permission identifier is normalized to
    /// uppercase.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsLowercaseCharacters_ShouldNormalizeToUppercase()
    {
        // Arrange
        const string permission = "user.create";

        // Act
        var permissionId = new PermissionId(permission);

        // Assert
        permissionId.Value.Should().Be("USER.CREATE");
    }

    /// <summary>
    /// Verifies that leading and trailing whitespace is removed
    /// before storing the permission identifier.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsLeadingOrTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string permission = "  user.create  ";

        // Act
        var permissionId = new PermissionId(permission);

        // Assert
        permissionId.Value.Should().Be("USER.CREATE");
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the permission identifier is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? permission = null;

        // Act
        Action act = () => new PermissionId(permission!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the permission identifier is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        const string permission = "";

        // Act
        Action act = () => new PermissionId(permission);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the permission identifier contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        const string permission = " ";

        // Act
        Action act = () => new PermissionId(permission);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the permission identifier format is invalid.
    /// </summary>
    [Theory]
    [InlineData("USER")]
    [InlineData("USER.")]
    [InlineData(".CREATE")]
    [InlineData("USER.CREATE.")]
    [InlineData("USER..CREATE")]
    [InlineData("USER_CREATE")]
    [InlineData("USER-CREATE")]
    [InlineData("USER.CREATE.TEST1")]
    [InlineData("user-create")]
    [InlineData("USER CREATE")]
    [InlineData("123.CREATE")]
    [InlineData("USER.123")]
    public void Constructor_WhenPermissionIdentifierIsInvalid_ShouldThrowArgumentException(
        string permission)
    {
        // Arrange

        // Act
        Action act = () => new PermissionId(permission);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("value");
    }

    /// <summary>
    /// Verifies that a permission identifier containing multiple
    /// hierarchical segments is accepted.
    /// </summary>
    [Theory]
    [InlineData("IDENTITY.USER.CREATE")]
    [InlineData("SYSTEM.SECURITY.PERMISSION.CREATE")]
    [InlineData("MODULE.SUBMODULE.ACTION")]
    public void Constructor_WhenPermissionIdentifierContainsMultipleSegments_ShouldCreateInstance(
        string permission)
    {
        // Arrange

        // Act
        var permissionId = new PermissionId(permission);

        // Assert
        permissionId.Value.Should().Be(permission);
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the normalized permission identifier value is
    /// preserved.
    /// </summary>
    [Fact]
    public void Value_WhenConstructed_ShouldReturnNormalizedValue()
    {
        // Arrange
        const string permission = "identity.user.create";

        // Act
        var permissionId = new PermissionId(permission);

        // Assert
        permissionId.Value.Should().Be("IDENTITY.USER.CREATE");
    }

    /// <summary>
    /// Verifies that the <see cref="PermissionId.Value"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(PermissionId)
            .GetProperty(nameof(PermissionId.Value));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same normalized
    /// permission identifier are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new PermissionId("user.create");
        var right = new PermissionId("USER.CREATE");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different permission
    /// identifiers are not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new PermissionId("USER.CREATE");
        var right = new PermissionId("USER.UPDATE");

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
        var permissionId = new PermissionId("USER.CREATE");

        // Act
        var result = permissionId.Equals(null);

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
        var permissionId = new PermissionId("USER.CREATE");

        // Act
        var result = permissionId.Equals(new object());

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// <see langword="true"/> for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new PermissionId("user.create");
        var right = new PermissionId("USER.CREATE");

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
    public void EqualityOperator_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new PermissionId("USER.CREATE");
        var right = new PermissionId("USER.UPDATE");

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
    public void InequalityOperator_WhenValuesAreDifferent_ShouldReturnTrue()
    {
        // Arrange
        var left = new PermissionId("USER.CREATE");
        var right = new PermissionId("USER.UPDATE");

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
    public void InequalityOperator_WhenValuesAreEqual_ShouldReturnFalse()
    {
        // Arrange
        var left = new PermissionId("user.create");
        var right = new PermissionId("USER.CREATE");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal permission identifiers produce the same
    /// hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new PermissionId("user.create");
        var right = new PermissionId("USER.CREATE");

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion

    #region ToString Tests

    /// <summary>
    /// Verifies that <see cref="PermissionId.ToString"/>
    /// returns the normalized permission identifier.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnNormalizedValue()
    {
        // Arrange
        var permissionId = new PermissionId("user.create");

        // Act
        var result = permissionId.ToString();

        // Assert
        result.Should().Be("USER.CREATE");
    }

    #endregion

    #region Implicit Conversion Tests

    /// <summary>
    /// Verifies that a string can be implicitly converted to a
    /// <see cref="PermissionId"/>.
    /// </summary>
    [Fact]
    public void ImplicitConversionFromString_ShouldCreatePermissionId()
    {
        // Arrange
        const string permission = "USER.CREATE";

        // Act
        PermissionId permissionId = permission;

        // Assert
        permissionId.Value.Should().Be(permission);
    }

    /// <summary>
    /// Verifies that a <see cref="PermissionId"/> can be implicitly
    /// converted to a string.
    /// </summary>
    [Fact]
    public void ImplicitConversionToString_ShouldReturnPermissionValue()
    {
        // Arrange
        var permissionId = new PermissionId("USER.CREATE");

        // Act
        string value = permissionId;

        // Assert
        value.Should().Be("USER.CREATE");
    }

    #endregion
}