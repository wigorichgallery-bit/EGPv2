using Platform.Communication.Models;

namespace Platform.Communication.UnitTests.Models;

/// <summary>
/// Contains unit tests for <see cref="EmailAttachment"/>.
/// </summary>
public sealed class EmailAttachmentTests
{
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the file name is invalid.
    /// </summary>
    /// <param name="fileName">
    /// Invalid file name.
    /// </param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowArgumentException_When_FileNameIsInvalid(
        string fileName)
    {
        // Arrange
        byte[] content = [1];
        const string contentType = "text/plain";

        // Act
        Action action = () =>
            _ = new EmailAttachment(
                fileName,
                content,
                contentType);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("fileName");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the content is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ContentIsNull()
    {
        // Arrange
        byte[]? content = null;

        // Act
        Action action = () =>
            _ = new EmailAttachment(
                "file.txt",
                content!,
                "text/plain");

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("content");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the attachment content is empty.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_ContentIsEmpty()
    {
        // Arrange
        byte[] content = [];

        // Act
        Action action = () =>
            _ = new EmailAttachment(
                "file.txt",
                content,
                "text/plain");

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("content")
            .WithMessage("Attachment content cannot be empty.*");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/>
    /// when the content type is invalid.
    /// </summary>
    /// <param name="contentType">
    /// Invalid content type.
    /// </param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowArgumentException_When_ContentTypeIsInvalid(
        string contentType)
    {
        // Arrange
        byte[] content = [1];

        // Act
        Action action = () =>
            _ = new EmailAttachment(
                "file.txt",
                content,
                contentType);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("contentType");
    }

    /// <summary>
    /// Verifies that the constructor stores
    /// the supplied values.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_When_ArgumentsAreValid()
    {
        // Arrange
        byte[] content = [1, 2, 3];

        // Act
        EmailAttachment attachment = new(
            "invoice.pdf",
            content,
            "application/pdf");

        // Assert
        attachment.FileName.Should().Be("invoice.pdf");
        attachment.Content.Should().BeSameAs(content);
        attachment.ContentType.Should().Be("application/pdf");
    }

    /// <summary>
    /// Verifies that two attachments having
    /// identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnTrue_When_ValuesAreEqual()
    {
        // Arrange
        byte[] content = [1, 2, 3];

        EmailAttachment left = new(
            "file.txt",
            content,
            "text/plain");

        EmailAttachment right = new(
            "file.txt",
            content,
            "text/plain");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that two attachments having
    /// different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        // Arrange
        byte[] content = [1];
        EmailAttachment left = new(
            "file1.txt",
            content,
            "text/plain");

        EmailAttachment right = new(
            "file2.txt",
            content,
            "text/plain");

        // Act
        bool result = left.Equals(right);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that equal attachments
    /// produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_When_ValuesAreEqual()
    {
        // Arrange
        byte[] content = [1,2];
        EmailAttachment left = new(
            "file.txt",
            content,
            "text/plain");

        EmailAttachment right = new(
            "file.txt",
            content,
            "text/plain");

        // Act

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
        byte[] content = [1];
        EmailAttachment left = new(
            "file.txt",
            content,
            "text/plain");

        EmailAttachment right = new(
            "file.txt",
            content,
            "text/plain");

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
        byte[] content = [1];
        EmailAttachment left = new(
            "file1.txt",
            content,
            "text/plain");

        EmailAttachment right = new(
            "file2.txt",
            content,
            "text/plain");

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
        byte[] content = [1];

        EmailAttachment? left = null;
        EmailAttachment? right = null;

        EmailAttachment value = new(
            "file.txt",
            content,
            "text/plain");

        // Act / Assert
        (left == right).Should().BeTrue();
        (left != right).Should().BeFalse();

        (left == value).Should().BeFalse();
        (left != value).Should().BeTrue();

        (value == right).Should().BeFalse();
        (value != right).Should().BeTrue();
    }
}