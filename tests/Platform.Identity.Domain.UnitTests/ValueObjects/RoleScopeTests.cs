// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/RoleScopeTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="RoleScope"/>
/// value object.
/// </summary>
public sealed class RoleScopeTests
{
    #region Static Instance Tests

    /// <summary>
    /// Verifies that <see cref="RoleScope.Global"/>
    /// represents the GLOBAL scope.
    /// </summary>
    [Fact]
    public void Global_ShouldReturnGlobalScope()
    {
        // Arrange

        // Act
        var scope = RoleScope.Global;

        // Assert
        scope.Value.Should().Be("GLOBAL");
    }

    /// <summary>
    /// Verifies that <see cref="RoleScope.Tenant"/>
    /// represents the TENANT scope.
    /// </summary>
    [Fact]
    public void Tenant_ShouldReturnTenantScope()
    {
        // Arrange

        // Act
        var scope = RoleScope.Tenant;

        // Assert
        scope.Value.Should().Be("TENANT");
    }

    /// <summary>
    /// Verifies that <see cref="RoleScope.Organization"/>
    /// represents the ORGANIZATION scope.
    /// </summary>
    [Fact]
    public void Organization_ShouldReturnOrganizationScope()
    {
        // Arrange

        // Act
        var scope = RoleScope.Organization;

        // Assert
        scope.Value.Should().Be("ORGANIZATION");
    }

    /// <summary>
    /// Verifies that <see cref="RoleScope.BusinessUnit"/>
    /// represents the BUSINESS_UNIT scope.
    /// </summary>
    [Fact]
    public void BusinessUnit_ShouldReturnBusinessUnitScope()
    {
        // Arrange

        // Act
        var scope = RoleScope.BusinessUnit;

        // Assert
        scope.Value.Should().Be("BUSINESS_UNIT");
    }

    /// <summary>
    /// Verifies that <see cref="RoleScope.Department"/>
    /// represents the DEPARTMENT scope.
    /// </summary>
    [Fact]
    public void Department_ShouldReturnDepartmentScope()
    {
        // Arrange

        // Act
        var scope = RoleScope.Department;

        // Assert
        scope.Value.Should().Be("DEPARTMENT");
    }

    #endregion

    #region Constructor Tests

    /// <summary>
    /// Verifies that a supported role scope creates a new
    /// <see cref="RoleScope"/> instance.
    /// </summary>
    [Theory]
    [InlineData("GLOBAL")]
    [InlineData("TENANT")]
    [InlineData("ORGANIZATION")]
    [InlineData("BUSINESS_UNIT")]
    [InlineData("DEPARTMENT")]
    public void Constructor_WhenScopeIsValid_ShouldCreateInstance(
        string value)
    {
        // Arrange

        // Act
        var scope = new RoleScope(value);

        // Assert
        scope.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies that lowercase scope values are normalized
    /// to uppercase.
    /// </summary>
    [Fact]
    public void Constructor_WhenScopeContainsLowercaseCharacters_ShouldNormalizeToUppercase()
    {
        // Arrange
        const string value = "tenant";

        // Act
        var scope = new RoleScope(value);

        // Assert
        scope.Value.Should().Be("TENANT");
    }

    /// <summary>
    /// Verifies that leading and trailing whitespace is removed.
    /// </summary>
    [Fact]
    public void Constructor_WhenScopeContainsLeadingOrTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string value = "  organization  ";

        // Act
        var scope = new RoleScope(value);

        // Assert
        scope.Value.Should().Be("ORGANIZATION");
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the scope value is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenScopeIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? value = null;

        // Act
        Action act = () => new RoleScope(value!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the scope value is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenScopeIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        Action act = () => new RoleScope(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the scope value contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_WhenScopeIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = " ";

        // Act
        Action act = () => new RoleScope(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the supplied scope is unsupported.
    /// </summary>
    [Theory]
    [InlineData("SYSTEM")]
    [InlineData("ROOT")]
    [InlineData("USER")]
    [InlineData("COMPANY")]
    public void Constructor_WhenScopeIsUnsupported_ShouldThrowArgumentException(
        string value)
    {
        // Arrange

        // Act
        Action act = () => new RoleScope(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("value");
    }

    #endregion

    #region Factory Method Tests

    /// <summary>
    /// Verifies that <see cref="RoleScope.From(string)"/>
    /// returns the predefined singleton instance.
    /// </summary>
    [Theory]
    [InlineData("GLOBAL")]
    [InlineData("TENANT")]
    [InlineData("ORGANIZATION")]
    [InlineData("BUSINESS_UNIT")]
    [InlineData("DEPARTMENT")]
    public void From_WhenScopeIsValid_ShouldReturnSingletonInstance(
        string value)
    {
        // Arrange

        // Act
        var scope = RoleScope.From(value);

        // Assert
        scope.Should().BeSameAs(new RoleScope(value) switch
        {
            { Value: "GLOBAL" } => RoleScope.Global,
            { Value: "TENANT" } => RoleScope.Tenant,
            { Value: "ORGANIZATION" } => RoleScope.Organization,
            { Value: "BUSINESS_UNIT" } => RoleScope.BusinessUnit,
            _ => RoleScope.Department
        });
    }

    /// <summary>
    /// Verifies that <see cref="RoleScope.From(string)"/>
    /// throws an exception for unsupported values.
    /// </summary>
    [Fact]
    public void From_WhenScopeIsUnsupported_ShouldThrowArgumentException()
    {
        // Arrange
        const string value = "INVALID";

        // Act
        Action act = () => RoleScope.From(value);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the scope value is preserved.
    /// </summary>
    [Fact]
    public void Value_WhenConstructed_ShouldReturnNormalizedValue()
    {
        // Arrange
        var scope = new RoleScope("tenant");

        // Act

        // Assert
        scope.Value.Should().Be("TENANT");
    }

    /// <summary>
    /// Verifies that the <see cref="RoleScope.Value"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(RoleScope)
            .GetProperty(nameof(RoleScope.Value));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same scope
    /// are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new RoleScope("GLOBAL");
        var right = new RoleScope("global");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different scopes
    /// are not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new RoleScope("GLOBAL");
        var right = new RoleScope("TENANT");

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
        var scope = RoleScope.Global;

        // Act
        var result = scope.Equals(null);

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
        var scope = RoleScope.Global;

        // Act
        var result = scope.Equals(new object());

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
        var left = new RoleScope("GLOBAL");
        var right = new RoleScope("global");

        // Act
        var result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// <see langword="true"/> for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_WhenValuesAreDifferent_ShouldReturnTrue()
    {
        // Arrange
        var left = new RoleScope("GLOBAL");
        var right = new RoleScope("TENANT");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal role scopes produce the same hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new RoleScope("GLOBAL");
        var right = new RoleScope("global");

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion

    #region ToString Tests

    /// <summary>
    /// Verifies that <see cref="RoleScope.ToString"/>
    /// returns the normalized scope value.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnNormalizedValue()
    {
        // Arrange
        var scope = new RoleScope("tenant");

        // Act
        var result = scope.ToString();

        // Assert
        result.Should().Be("TENANT");
    }

    #endregion

    #region Implicit Conversion Tests

    /// <summary>
    /// Verifies that a string is implicitly converted into
    /// a <see cref="RoleScope"/>.
    /// </summary>
    [Fact]
    public void ImplicitConversionFromString_ShouldCreateRoleScope()
    {
        // Arrange

        // Act
        RoleScope scope = "GLOBAL";

        // Assert
        scope.Should().BeSameAs(RoleScope.Global);
    }

    /// <summary>
    /// Verifies that a <see cref="RoleScope"/> is implicitly
    /// converted into its string representation.
    /// </summary>
    [Fact]
    public void ImplicitConversionToString_ShouldReturnScopeValue()
    {
        // Arrange
        RoleScope scope = RoleScope.Department;

        // Act
        string value = scope;

        // Assert
        value.Should().Be("DEPARTMENT");
    }

    #endregion
}