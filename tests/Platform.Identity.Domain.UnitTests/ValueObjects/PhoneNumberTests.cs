// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// ValueObjects/PhoneNumberTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="PhoneNumber"/>
/// value object.
/// </summary>
public sealed class PhoneNumberTests
{
    #region Constructor Tests

    /// <summary>
    /// Verifies that a valid E.164 phone number creates a new
    /// <see cref="PhoneNumber"/> instance.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateInstance()
    {
        // Arrange
        const string phone = "+6281234567890";

        // Act
        var phoneNumber = new PhoneNumber(phone);

        // Assert
        phoneNumber.Value.Should().Be(phone);
    }

    /// <summary>
    /// Verifies that leading and trailing whitespace is removed
    /// before storing the phone number.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueContainsLeadingOrTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string phone = "  +6281234567890  ";

        // Act
        var phoneNumber = new PhoneNumber(phone);

        // Assert
        phoneNumber.Value.Should().Be("+6281234567890");
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the phone number is <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? phone = null;

        // Act
        Action act = () => new PhoneNumber(phone!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the phone number is empty.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        const string phone = "";

        // Act
        Action act = () => new PhoneNumber(phone);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentException"/> is thrown
    /// when the phone number contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_WhenValueIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        const string phone = " ";

        // Act
        Action act = () => new PhoneNumber(phone);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a <see cref="DomainException"/> is thrown
    /// when the phone number format is invalid.
    /// </summary>
    [Theory]
    [InlineData("6281234567890")]
    [InlineData("+")]
    [InlineData("+0")]
    [InlineData("+0123456789")]
    [InlineData("+1234567")]
    [InlineData("+1234567890123456")]
    [InlineData("+62-81234567890")]
    [InlineData("+62 81234567890")]
    [InlineData("+62(812)34567890")]
    [InlineData("+ABC12345678")]
    [InlineData("081234567890")]
    public void Constructor_WhenPhoneNumberFormatIsInvalid_ShouldThrowDomainException(
        string phone)
    {
        // Arrange

        // Act
        Action act = () => new PhoneNumber(phone);

        // Assert
        act.Should()
            .Throw<DomainException>()
            .Which.ErrorCode
            .Should()
            .Be("IDENTITY.INVALID_PHONE");
    }

    /// <summary>
    /// Verifies that the minimum valid E.164 phone number length
    /// is accepted.
    /// </summary>
    [Fact]
    public void Constructor_WhenPhoneNumberHasMinimumLength_ShouldCreateInstance()
    {
        // Arrange
        const string phone = "+12345678";

        // Act
        var phoneNumber = new PhoneNumber(phone);

        // Assert
        phoneNumber.Value.Should().Be(phone);
    }

    /// <summary>
    /// Verifies that the maximum valid E.164 phone number length
    /// is accepted.
    /// </summary>
    [Fact]
    public void Constructor_WhenPhoneNumberHasMaximumLength_ShouldCreateInstance()
    {
        // Arrange
        const string phone = "+123456789012345";

        // Act
        var phoneNumber = new PhoneNumber(phone);

        // Assert
        phoneNumber.Value.Should().Be(phone);
    }

    #endregion

    #region Property Tests

    /// <summary>
    /// Verifies that the normalized phone number value is preserved.
    /// </summary>
    [Fact]
    public void Value_WhenConstructed_ShouldReturnNormalizedValue()
    {
        // Arrange
        const string phone = "+6281234567890";

        // Act
        var phoneNumber = new PhoneNumber(phone);

        // Assert
        phoneNumber.Value.Should().Be(phone);
    }

    /// <summary>
    /// Verifies that the <see cref="PhoneNumber.Value"/>
    /// property is immutable.
    /// </summary>
    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var property = typeof(PhoneNumber)
            .GetProperty(nameof(PhoneNumber.Value));

        // Act

        // Assert
        property.Should().NotBeNull();
        property!.CanWrite.Should().BeFalse();
    }

    #endregion

    #region Equality Tests

    /// <summary>
    /// Verifies that two instances having the same phone number
    /// are structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6281234567890");

        // Act
        var result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two instances having different phone numbers
    /// are not structurally equal.
    /// </summary>
    [Fact]
    public void Equals_WhenValuesAreDifferent_ShouldReturnFalse()
    {
        // Arrange
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6289876543210");

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
        var phoneNumber = new PhoneNumber("+6281234567890");

        // Act
        var result = phoneNumber.Equals(null);

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
        var phoneNumber = new PhoneNumber("+6281234567890");

        // Act
        var result = phoneNumber.Equals(new object());

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
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6281234567890");

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
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6289876543210");

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
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6289876543210");

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
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6281234567890");

        // Act
        var result = left != right;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region HashCode Tests

    /// <summary>
    /// Verifies that equal phone numbers produce the same
    /// hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var left = new PhoneNumber("+6281234567890");
        var right = new PhoneNumber("+6281234567890");

        // Act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();

        // Assert
        leftHash.Should().Be(rightHash);
    }

    #endregion

    #region ToString Tests

    /// <summary>
    /// Verifies that <see cref="PhoneNumber.ToString"/>
    /// returns the normalized phone number.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnNormalizedValue()
    {
        // Arrange
        var phoneNumber = new PhoneNumber("+6281234567890");

        // Act
        var result = phoneNumber.ToString();

        // Assert
        result.Should().Be("+6281234567890");
    }

    #endregion
}