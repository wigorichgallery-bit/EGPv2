using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for <see cref="PhoneNumber"/>.
/// </summary>
public sealed class PhoneNumberTests
{
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the supplied phone number is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ValueIsNull()
    {
        // Arrange
        string? value = null;

        // Act
        Action action = () => _ = new PhoneNumber(value!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the supplied phone number does not
    /// follow the E.164 format.
    /// </summary>
    /// <param name="value">
    /// Invalid phone number.
    /// </param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("+")]
    [InlineData("+0")]
    [InlineData("+01")]
    [InlineData("628123456789")]
    [InlineData("+62 8123456789")]
    [InlineData("+62-8123456789")]
    [InlineData("++628123456789")]
    [InlineData("+1234567890123456")]
    public void Constructor_Should_ThrowArgumentException_When_ValueIsInvalid(
        string value)
    {
        // Arrange

        // Act
        Action action = () => _ = new PhoneNumber(value);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("value")
            .WithMessage("Phone number must follow E.164 format.*");
    }

    /// <summary>
    /// Verifies that the constructor trims
    /// leading and trailing whitespace.
    /// </summary>
    [Fact]
    public void Constructor_Should_TrimValue_When_ValueContainsWhitespace()
    {
        // Arrange
        const string input = "  +628123456789  ";

        // Act
        PhoneNumber phoneNumber = new(input);

        // Assert
        phoneNumber.Value.Should().Be("+628123456789");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// the supplied phone number.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetValue_When_ValueIsValid()
    {
        // Arrange
        const string value = "+628123456789";

        // Act
        PhoneNumber phoneNumber = new(value);

        // Assert
        phoneNumber.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies that <see cref="PhoneNumber.ToString"/>
    /// returns the encapsulated phone number.
    /// </summary>
    [Fact]
    public void ToString_Should_ReturnValue()
    {
        // Arrange
        PhoneNumber phoneNumber = new("+628123456789");

        // Act
        string result = phoneNumber.ToString();

        // Assert
        result.Should().Be("+628123456789");
    }

    /// <summary>
    /// Verifies that two phone numbers having
    /// the same value are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValueIsEqual()
    {
        // Arrange
        PhoneNumber left = new("+628123456789");
        PhoneNumber right = new("+628123456789");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two phone numbers having
    /// different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValueIsDifferent()
    {
        // Arrange
        PhoneNumber left = new("+628123456789");
        PhoneNumber right = new("+628987654321");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equality returns false
    /// when compared with null.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_OtherIsNull()
    {
        // Arrange
        PhoneNumber phoneNumber = new("+628123456789");

        // Act
        bool result = phoneNumber.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equality returns false
    /// when compared with another value object type.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_TypeIsDifferent()
    {
        // Arrange
        PhoneNumber phoneNumber = new("+628123456789");
        WhatsAppNumber whatsappNumber = new("+628123456789");

        // Act
        bool result = phoneNumber.Equals(whatsappNumber);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that two equal phone numbers
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValueIsEqual()
    {
        // Arrange
        PhoneNumber left = new("+628123456789");
        PhoneNumber right = new("+628123456789");

        // Act

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that the equality operator returns
    /// true when both operands have the same value.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_ReturnTrue_When_ValueIsEqual()
    {
        // Arrange
        PhoneNumber left = new("+628123456789");
        PhoneNumber right = new("+628123456789");

        // Act
        bool result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator returns
    /// true when operands have different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_Should_ReturnTrue_When_ValueIsDifferent()
    {
        // Arrange
        PhoneNumber left = new("+628123456789");
        PhoneNumber right = new("+628987654321");

        // Act
        bool result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that equality operators correctly
    /// handle null operands.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange
        PhoneNumber? left = null;
        PhoneNumber? right = null;
        PhoneNumber value = new("+628123456789");

        // Act

        // Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}