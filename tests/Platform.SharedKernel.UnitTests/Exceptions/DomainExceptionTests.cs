using FluentAssertions;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Exceptions;

/// <summary>
/// Contains unit tests for the <see cref="DomainException"/> class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies that <see cref="DomainException"/> correctly stores domain
/// error information and enforces constructor argument validation.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify successful construction using valid arguments.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify validation of the <c>errorCode</c> argument.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify that exception properties are initialized correctly.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for <see cref="DomainException"/> only.
/// </para>
/// </remarks>
/// </summary>
public sealed class DomainExceptionTests
{
    #region Constructor

    /// <summary>
    /// Verifies that the constructor creates a
    /// <see cref="DomainException"/> using valid arguments.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The supplied error code is preserved.</description></item>
    /// <item><description>The supplied message is preserved.</description></item>
    /// <item><description><see cref="Exception.InnerException"/> is <see langword="null"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateException()
    {
        // Arrange
        const string errorCode = "USER_ALREADY_EXISTS";
        const string message = "User already exists.";

        // Act
        var exception = new DomainException(errorCode, message);

        // Assert
        exception.ErrorCode.Should().Be(errorCode);
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied
    /// error code is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>errorCode</c> parameter.</description></item>
    /// <item><description>The validation message indicates that an error code is required.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithNullErrorCode_ShouldThrowArgumentException()
    {
        // Arrange
        string? errorCode = null;

        // Act
        var action = () => new DomainException(errorCode!, "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorCode")
            .Which;

        exception.Message.Should().Contain("ErrorCode required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied
    /// error code is an empty string.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>errorCode</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithEmptyErrorCode_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new DomainException(string.Empty, "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorCode")
            .Which;

        exception.Message.Should().Contain("ErrorCode required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied
    /// error code consists only of white-space characters.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>errorCode</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithWhitespaceErrorCode_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new DomainException("   ", "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorCode")
            .Which;

        exception.Message.Should().Contain("ErrorCode required.");
    }

    #endregion

    #region Exception Properties

    /// <summary>
    /// Verifies that the constructor preserves the supplied exception message.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="Exception.Message"/> equals the supplied message.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_ShouldStoreMessage()
    {
        // Arrange
        const string message = "Aggregate invariant violated.";

        // Act
        var exception = new DomainException("DOMAIN_ERROR", message);

        // Assert
        exception.Message.Should().Be(message);
    }

    /// <summary>
    /// Verifies that the constructor initializes
    /// <see cref="Exception.InnerException"/> to
    /// <see langword="null"/> when no inner exception is supplied.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description><see cref="Exception.InnerException"/> is <see langword="null"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_ShouldInitializeWithoutInnerException()
    {
        // Arrange

        // Act
        var exception = new DomainException(
            "DOMAIN_ERROR",
            "Invariant failed.");

        // Assert
        exception.InnerException.Should().BeNull();
    }

    #endregion
}