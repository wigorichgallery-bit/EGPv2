// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/EmailAddressTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="EmailAddress"/>
/// value object.
/// </summary>
public sealed class EmailAddressTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that a valid email address creates a new
    /// <see cref="EmailAddress"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateInstance()
    {
        // Arrange
        const string email = "user@example.com";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        emailAddress.Value.Should().Be(email);
    }

    /// <summary>
    /// Verifies that an email address is normalized to lowercase.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsUppercaseCharacters_ShouldNormalizeToLowercase()
    {
        // Arrange
        const string email = "John.DOE@Example.COM";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        emailAddress.Value.Should().Be("john.doe@example.com");
    }

    /// <summary>
    /// Verifies that leading and trailing whitespace is removed
    /// before storing the email address.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsLeadingOrTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string email = "   User@Example.com   ";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        emailAddress.Value.Should().Be("user@example.com");
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the email value is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? email = null;

        // Act
        Action act = () => new EmailAddress(email!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the email value is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        const string email = "";

        // Act
        Action act = () => new EmailAddress(email);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the email value contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        const string email = " ";

        // Act
        Action act = () => new EmailAddress(email);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a <see cref="DomainException"/> is thrown
    /// when the email format is invalid.
    /// </summary>
    [Theory]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user@example")]
    [InlineData("user@@example.com")]
    [InlineData("user @example.com")]
    public void Constructor_WhenEmailFormatIsInvalid_ShouldThrowDomainException(
        string email)
    {
        // Arrange

        // Act
        Action act = () => new EmailAddress(email);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be("IDENTITY.INVALID_EMAIL");
    }

    /// <summary>
    /// Verifies that Unicode characters are accepted when they
    /// satisfy the validation pattern.
    /// </summary>
    [Fact]
    public void Constructor_WhenEmailContainsUnicodeCharacters_ShouldCreateInstance()
    {
        // Arrange
        const string email = "测试@example.com";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        emailAddress.Value.Should().Be("测试@example.com");
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the normalized email value is preserved.
    /// </summary>
    [Fact]
    public void Value_WhenConstructed_ShouldReturnNormalizedValue()
    {
        // Arrange
        const string email = "USER@Example.COM";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        emailAddress.Value.Should().Be("user@example.com");
    }

    /// <summary>
    /// Verifies that the <see cref="EmailAddress.Value"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(EmailAddress)
            .GetProperty(nameof(EmailAddress.Value));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same normalized
    /// value are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenNormalizedValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new EmailAddress("USER@Example.com");
        var right = new EmailAddress("user@example.com");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different values are
    /// not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new EmailAddress("user1@example.com");
        var right = new EmailAddress("user2@example.com");

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
        var emailAddress = new EmailAddress("user@example.com");

        // Act
        var result = emailAddress.Equals(null);

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
        var emailAddress = new EmailAddress("user@example.com");

        // Act
        var result = emailAddress.Equals(new object());

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
        var left = new EmailAddress("USER@Example.com");
        var right = new EmailAddress("user@example.com");

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
        var left = new EmailAddress("user1@example.com");
        var right = new EmailAddress("user2@example.com");

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
        var left = new EmailAddress("user1@example.com");
        var right = new EmailAddress("user2@example.com");

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
        var left = new EmailAddress("USER@Example.com");
        var right = new EmailAddress("user@example.com");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal email addresses produce the same
    /// hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenNormalizedValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new EmailAddress("USER@Example.com");
        var right = new EmailAddress("user@example.com");

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion

    #region ToString Tests

    /// <summary>
    /// Verifies that <see cref="EmailAddress.ToString"/>
    /// returns the normalized email address.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnNormalizedValue()
    {
        // Arrange
        var emailAddress = new EmailAddress("USER@Example.com");

        // Act
        var result = emailAddress.ToString();

        // Assert
        result.Should().Be("user@example.com");
    }

    #endregion
}