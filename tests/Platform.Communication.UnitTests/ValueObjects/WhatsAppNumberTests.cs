using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for <see cref="WhatsAppNumber"/>.
/// </summary>
public sealed class WhatsAppNumberTests
{
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the supplied WhatsApp number is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ValueIsNull()
    {
        // Arrange
        string? value = null;

        // Act
        Action action = () => _ = new WhatsAppNumber(value!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the supplied WhatsApp number
    /// does not follow the E.164 format.
    /// </summary>
    /// <param name="value">
    /// Invalid WhatsApp number.
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
        Action action = () => _ = new WhatsAppNumber(value);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("value")
            .WithMessage("WhatsApp number must follow E.164 format.*");
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
        WhatsAppNumber whatsappNumber = new(input);

        // Assert
        whatsappNumber.Value.Should().Be("+628123456789");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// the supplied WhatsApp number.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetValue_When_ValueIsValid()
    {
        // Arrange
        const string value = "+628123456789";

        // Act
        WhatsAppNumber whatsappNumber = new(value);

        // Assert
        whatsappNumber.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies that
    /// <see cref="WhatsAppNumber.ToString"/>
    /// returns the encapsulated WhatsApp number.
    /// </summary>
    [Fact]
    public void ToString_Should_ReturnValue()
    {
        // Arrange
        WhatsAppNumber whatsappNumber = new("+628123456789");

        // Act
        string result = whatsappNumber.ToString();

        // Assert
        result.Should().Be("+628123456789");
    }

    /// <summary>
    /// Verifies that two WhatsApp numbers having
    /// the same value are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValueIsEqual()
    {
        // Arrange
        WhatsAppNumber left = new("+628123456789");
        WhatsAppNumber right = new("+628123456789");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two WhatsApp numbers having
    /// different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValueIsDifferent()
    {
        // Arrange
        WhatsAppNumber left = new("+628123456789");
        WhatsAppNumber right = new("+628987654321");

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
        WhatsAppNumber whatsappNumber = new("+628123456789");

        // Act
        bool result = whatsappNumber.Equals(null);

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
        WhatsAppNumber whatsappNumber = new("+628123456789");
        PhoneNumber phoneNumber = new("+628123456789");

        // Act
        bool result = whatsappNumber.Equals(phoneNumber);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that two equal WhatsApp numbers
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValueIsEqual()
    {
        // Arrange
        WhatsAppNumber left = new("+628123456789");
        WhatsAppNumber right = new("+628123456789");

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
        WhatsAppNumber left = new("+628123456789");
        WhatsAppNumber right = new("+628123456789");

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
        WhatsAppNumber left = new("+628123456789");
        WhatsAppNumber right = new("+628987654321");

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
        WhatsAppNumber? left = null;
        WhatsAppNumber? right = null;
        WhatsAppNumber value = new("+628123456789");

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