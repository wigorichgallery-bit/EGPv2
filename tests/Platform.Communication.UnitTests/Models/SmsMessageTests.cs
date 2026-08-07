using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="SmsMessage"/>.
/// </summary>
public sealed class SmsMessageTests
{
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when recipients are null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ToIsNull()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber>? to = null;

        // Act
        Action action = () =>
            _ = new SmsMessage(
                to!,
                "Message");

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("to");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the recipient collection is empty.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_ToIsEmpty()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to = [];

        // Act
        Action action = () =>
            _ = new SmsMessage(
                to,
                "Message");

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("to")
            .WithMessage("At least one recipient is required.*");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the message is invalid.
    /// </summary>
    /// <param name="message">
    /// Invalid SMS message.
    /// </param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowArgumentException_When_MessageIsInvalid(
        string message)
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        // Act
        Action action = () =>
            _ = new SmsMessage(
                to,
                message);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("message");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// all supplied values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_ArgumentsAreValid()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        // Act
        SmsMessage message = new(
            to,
            "Test SMS");

        // Assert
        message.To.Should().BeSameAs(to);
        message.Message.Should().Be("Test SMS");
    }

    /// <summary>
    /// Verifies that two SMS messages
    /// having identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        SmsMessage left = new(to, "Message");
        SmsMessage right = new(to, "Message");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two SMS messages
    /// having different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        SmsMessage left = new(to, "Message");
        SmsMessage right = new(to, "Different");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equal SMS messages
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        SmsMessage left = new(to, "Message");
        SmsMessage right = new(to, "Message");

        // Assert
        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    /// <summary>
    /// Verifies that the equality operator
    /// returns true for equal values.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        SmsMessage left = new(to, "Message");
        SmsMessage right = new(to, "Message");

        // Act
        bool result = left == right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the inequality operator
    /// returns true for different values.
    /// </summary>
    [Fact]
    public void InequalityOperator_Should_ReturnTrue_When_ValuesAreDifferent()
    {
        // Arrange
        IReadOnlyCollection<PhoneNumber> to =
        [
            new PhoneNumber("+628123456789")
        ];

        SmsMessage left = new(to, "Message");
        SmsMessage right = new(to, "Another Message");

        // Act
        bool result = left != right;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that equality operators
    /// correctly handle null operands.
    /// </summary>
    [Fact]
    public void EqualityOperator_Should_HandleNullOperands()
    {
        // Arrange
        SmsMessage? left = null;
        SmsMessage? right = null;

        SmsMessage value = new(
        [
            new PhoneNumber("+628123456789")
        ],
        "Message");

        // Act / Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}