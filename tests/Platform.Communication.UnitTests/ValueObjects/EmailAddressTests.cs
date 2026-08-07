using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.ValueObjects;

/// <summary>
/// Contains unit tests for <see cref="EmailAddress"/>.
/// </summary>
public sealed class EmailAddressTests
{
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the specified email address is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ValueIsNull()
    {
        // Arrange
        string? value = null;

        // Act
        Action action = () => _ = new EmailAddress(value!);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the specified email address is invalid.
    /// </summary>
    /// <param name="value">
    /// Invalid email address.
    /// </param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("plainaddress")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@domain")]
    [InlineData("user@@domain.com")]
    public void Constructor_Should_ThrowArgumentException_When_EmailIsInvalid(
        string value)
    {
        // Arrange

        // Act
        Action action = () => _ = new EmailAddress(value);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithParameterName("value")
            .WithMessage("Email address format is invalid.*");
    }

    /// <summary>
    /// Verifies that the constructor trims
    /// leading and trailing whitespace.
    /// </summary>
    [Fact]
    public void Constructor_Should_TrimValue_When_EmailContainsWhitespace()
    {
        // Arrange
        const string input = "  user@example.com  ";

        // Act
        EmailAddress email = new(input);

        // Assert
        email.Value.Should().Be("user@example.com");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// the supplied email address.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetValue_When_EmailIsValid()
    {
        // Arrange
        const string value = "user@example.com";

        // Act
        EmailAddress email = new(value);

        // Assert
        email.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies that <see cref="EmailAddress.ToString"/>
    /// returns the encapsulated email address.
    /// </summary>
    [Fact]
    public void ToString_Should_ReturnValue()
    {
        // Arrange
        EmailAddress email = new("user@example.com");

        // Act
        string result = email.ToString();

        // Assert
        result.Should().Be("user@example.com");
    }

    /// <summary>
    /// Verifies that two email addresses having
    /// the same value are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValueIsEqual()
    {
        // Arrange
        EmailAddress left = new("user@example.com");
        EmailAddress right = new("user@example.com");

        // Act

        // Assert
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that two email addresses having
    /// different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValueIsDifferent()
    {
        // Arrange
        EmailAddress left = new("user1@example.com");
        EmailAddress right = new("user2@example.com");

        // Act

        // Assert
        left.Equals(right).Should().BeFalse();
        (left == right).Should().BeFalse();
        (left != right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that equality returns false
    /// when compared with null.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_OtherIsNull()
    {
        // Arrange
        EmailAddress email = new("user@example.com");

        // Act

        // Assert
        email.Equals(null).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that two equal email addresses
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValueIsEqual()
    {
        // Arrange
        EmailAddress left = new("user@example.com");
        EmailAddress right = new("user@example.com");

        // Act

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that equality returns false
    /// when compared with another value object type.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_TypeIsDifferent()
    {
        // Arrange
        EmailAddress email = new("user@example.com");
        PhoneNumber phone = new("+628123456789");

        // Act

        // Assert
        email.Equals(phone).Should().BeFalse();
        (email == phone).Should().BeFalse();
        (email != phone).Should().BeTrue();
    }

    /// <summary>
    /// Verifies equality operators when one
    /// or both operands are null.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange
        EmailAddress? left = null;
        EmailAddress? right = null;
        EmailAddress value = new("user@example.com");

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