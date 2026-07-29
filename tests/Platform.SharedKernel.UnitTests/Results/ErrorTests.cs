using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

/// <summary>
/// Contains unit tests for the <see cref="Error"/> class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies the behavior of the <see cref="Error"/> constructors,
/// validates constructor arguments, and confirms the contract of the
/// predefined <see cref="Error.None"/> instance.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify constructor behavior using the default error type.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify constructor behavior using an explicitly supplied
/// <see cref="ErrorType"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify validation of error code and message arguments.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify the predefined <see cref="Error.None"/> instance.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="Error"/> value object only.
/// </para>
/// </remarks>
/// </summary>
public sealed class ErrorTests
{
    #region Error Constructors

    /// <summary>
    /// Verifies that the two-parameter constructor creates an
    /// <see cref="Error"/> using <see cref="ErrorType.Internal"/>
    /// as the default error type.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The supplied code is preserved.</description></item>
    /// <item><description>The supplied message is preserved.</description></item>
    /// <item><description>The default error type is <see cref="ErrorType.Internal"/>.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithValidArguments_ShouldCreateError()
    {
        // Arrange
        const string code = "USER_NOT_FOUND";
        const string message = "User not found.";

        // Act
        var error = new Error(code, message);

        // Assert
        error.Code.Should().Be(code);
        error.Message.Should().Be(message);
        error.Type.Should().Be(ErrorType.Internal);
    }

    /// <summary>
    /// Verifies that the three-parameter constructor preserves the supplied
    /// <see cref="ErrorType"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>The supplied code is preserved.</description></item>
    /// <item><description>The supplied message is preserved.</description></item>
    /// <item><description>The supplied error type is preserved.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithExplicitErrorType_ShouldCreateError()
    {
        // Arrange
        const string code = "VALIDATION";
        const string message = "Validation failed.";

        // Act
        var error = new Error(
            code,
            message,
            ErrorType.Validation);

        // Assert
        error.Code.Should().Be(code);
        error.Message.Should().Be(message);
        error.Type.Should().Be(ErrorType.Validation);
    }

    #endregion

    #region Invalid Code

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error code
    /// is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>code</c> parameter.</description></item>
    /// <item><description>The validation message indicates that an error code is required.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithNullCode_ShouldThrowArgumentException()
    {
        // Arrange
        string? code = null;

        // Act
        var action = () => new Error(code!, "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("code")
            .Which;

        exception.Message.Should().Contain("Error code required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error code
    /// is an empty string.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>code</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithEmptyCode_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new Error(string.Empty, "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("code")
            .Which;

        exception.Message.Should().Contain("Error code required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error code
    /// consists only of white-space characters.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item><description>An <see cref="ArgumentException"/> is thrown.</description></item>
    /// <item><description>The exception identifies the <c>code</c> parameter.</description></item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithWhitespaceCode_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new Error("   ", "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("code")
            .Which;

        exception.Message.Should().Contain("Error code required.");
    }

    #endregion

    #region Invalid Message
    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error message
    /// is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception identifies the <c>message</c> parameter.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The validation message indicates that an error message is required.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithNullMessage_ShouldThrowArgumentException()
    {
        // Arrange
        string? message = null;

        // Act
        var action = () => new Error("CODE", message!);

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("message")
            .Which;

        exception.Message.Should().Contain("Error message required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error message
    /// is an empty string.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception identifies the <c>message</c> parameter.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithEmptyMessage_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new Error("CODE", string.Empty);

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("message")
            .Which;

        exception.Message.Should().Contain("Error message required.");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentException"/> when the supplied error message
    /// contains only white-space characters.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception identifies the <c>message</c> parameter.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Constructor_WithWhitespaceMessage_ShouldThrowArgumentException()
    {
        // Arrange

        // Act
        var action = () => new Error("CODE", "   ");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("message")
            .Which;

        exception.Message.Should().Contain("Error message required.");
    }

    #endregion

    #region Error.None

    /// <summary>
    /// Verifies that the predefined <see cref="Error.None"/> instance
    /// exposes the expected default values.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="Error.None"/> has the code <c>"NONE"</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Error.None"/> has the message <c>"No error"</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Error.None"/> has the type <see cref="ErrorType.None"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Fact]
    public void None_ShouldContainExpectedValues()
    {
        // Arrange

        // Act

        // Assert
        Error.None.Code.Should().Be("NONE");
        Error.None.Message.Should().Be("No error");
        Error.None.Type.Should().Be(ErrorType.None);
    }

    #endregion
}