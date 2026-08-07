using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="EmailMessage"/>.
/// </summary>
public sealed class EmailMessageTests
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
        IReadOnlyCollection<EmailAddress>? to = null;

        // Act
        Action action = () =>
            _ = new EmailMessage(
                to!,
                "Subject",
                "Body");

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
        IReadOnlyCollection<EmailAddress> to = [];

        // Act
        Action action = () =>
            _ = new EmailMessage(
                to,
                "Subject",
                "Body");

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("to")
            .WithMessage("At least one recipient is required.*");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the subject is invalid.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowArgumentException_When_SubjectIsInvalid(
        string subject)
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        // Act
        Action action = () =>
            _ = new EmailMessage(
                to,
                subject,
                "Body");

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("subject");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the body is invalid.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowArgumentException_When_BodyIsInvalid(
        string body)
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        // Act
        Action action = () =>
            _ = new EmailMessage(
                to,
                "Subject",
                body);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("body");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// all required properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_ArgumentsAreValid()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        // Act
        EmailMessage message = new(
            to,
            "Subject",
            "Body");

        // Assert
        message.To.Should().BeSameAs(to);
        message.Subject.Should().Be("Subject");
        message.Body.Should().Be("Body");
        message.IsHtml.Should().BeFalse();
        message.Cc.Should().BeNull();
        message.Bcc.Should().BeNull();
        message.Attachments.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// optional properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetOptionalProperties_When_Provided()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        IReadOnlyCollection<EmailAddress> cc =
        [
            new EmailAddress("cc@example.com")
        ];

        IReadOnlyCollection<EmailAddress> bcc =
        [
            new EmailAddress("bcc@example.com")
        ];

        IReadOnlyCollection<EmailAttachment> attachments =
        [
            new EmailAttachment(
                "invoice.pdf",
                [1, 2, 3],
                "application/pdf")
        ];

        // Act
        EmailMessage message = new(
            to,
            "Subject",
            "Body",
            isHtml: true,
            cc: cc,
            bcc: bcc,
            attachments: attachments);

        // Assert
        message.To.Should().BeSameAs(to);
        message.Cc.Should().BeSameAs(cc);
        message.Bcc.Should().BeSameAs(bcc);
        message.Attachments.Should().BeSameAs(attachments);
        message.IsHtml.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that optional collections
    /// may be null.
    /// </summary>
    [Fact]
    public void Constructor_Should_AllowOptionalCollectionsToBeNull()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        // Act
        EmailMessage message = new(
            to,
            "Subject",
            "Body",
            cc: null,
            bcc: null,
            attachments: null);

        // Assert
        message.Cc.Should().BeNull();
        message.Bcc.Should().BeNull();
        message.Attachments.Should().BeNull();
    }

    /// <summary>
    /// Verifies that two email messages
    /// having identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        EmailMessage left = new(
            to,
            "Subject",
            "Body");

        EmailMessage right = new(
            to,
            "Subject",
            "Body");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two email messages
    /// having different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        EmailMessage left = new(
            to,
            "Subject",
            "Body");

        EmailMessage right = new(
            to,
            "Different",
            "Body");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equal email messages
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        EmailMessage left = new(
            to,
            "Subject",
            "Body");

        EmailMessage right = new(
            to,
            "Subject",
            "Body");

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
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        EmailMessage left = new(
            to,
            "Subject",
            "Body");

        EmailMessage right = new(
            to,
            "Subject",
            "Body");

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
        IReadOnlyCollection<EmailAddress> to =
        [
            new EmailAddress("user@example.com")
        ];

        EmailMessage left = new(
            to,
            "Subject",
            "Body");

        EmailMessage right = new(
            to,
            "Another Subject",
            "Body");

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
        EmailMessage? left = null;
        EmailMessage? right = null;

        EmailMessage value = new(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body");

        // Act / Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}