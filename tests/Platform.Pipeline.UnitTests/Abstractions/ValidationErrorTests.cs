// ===========================================
// File Location:
// tests/Platform.Pipeline.UnitTests/
// Abstractions/ValidationErrorTests.cs
// ===========================================

using FluentAssertions;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Pipeline.UnitTests.Abstractions;

/// <summary>
/// Contains unit tests for <see cref="ValidationError"/>.
/// </summary>
public sealed class ValidationErrorTests
{
    /// <summary>
    /// Verifies that the constructor initializes all properties
    /// when valid arguments are provided.
    /// </summary>
    [Fact]
    public void Constructor_Should_SetProperties_WhenArgumentsAreValid()
    {
        // Arrange
        const string code = "VALIDATION.REQUIRED";
        const string message = "Validation failed.";

        // Act
        var validationError = new ValidationError(
            code,
            message);

        // Assert
        validationError.Code.Should().Be(code);
        validationError.Message.Should().Be(message);
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="code"/> is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenCodeIsNull()
    {
        // Arrange
        string? code = null;
        const string message = "Validation failed.";

        // Act
        Action act = () => new ValidationError(
            code!,
            message);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="code"/> is an empty string.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenCodeIsEmpty()
    {
        // Arrange
        const string code = "";
        const string message = "Validation failed.";

        // Act
        Action act = () => new ValidationError(
            code,
            message);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="code"/> contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenCodeIsWhiteSpace()
    {
        // Arrange
        const string code = "   ";
        const string message = "Validation failed.";

        // Act
        Action act = () => new ValidationError(
            code,
            message);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="message"/> is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenMessageIsNull()
    {
        // Arrange
        const string code = "VALIDATION.REQUIRED";
        string? message = null;

        // Act
        Action act = () => new ValidationError(
            code,
            message!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="message"/> is an empty string.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenMessageIsEmpty()
    {
        // Arrange
        const string code = "VALIDATION.REQUIRED";
        const string message = "";

        // Act
        Action act = () => new ValidationError(
            code,
            message);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the constructor throws an exception
    /// when <paramref name="message"/> contains only whitespace.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenMessageIsWhiteSpace()
    {
        // Arrange
        const string code = "VALIDATION.REQUIRED";
        const string message = "   ";

        // Act
        Action act = () => new ValidationError(
            code,
            message);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }
}