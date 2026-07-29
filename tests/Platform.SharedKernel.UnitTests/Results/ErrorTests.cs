using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

public sealed class ErrorTests
{

    #region Constructor (2 parameter)
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

    #region Invalid code
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

        [Fact]
        public void Constructor_WithEmptyCode_ShouldThrowArgumentException()
        {
            // Act
            var action = () => new Error(string.Empty, "message");

            // Assert
            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName("code")
                .Which;

            exception.Message.Should().Contain("Error code required.");
        }

        [Fact]
        public void Constructor_WithWhitespaceCode_ShouldThrowArgumentException()
        {
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

    #region Invalid message
        [Fact]
        public void Constructor_WithNullMessage_ShouldThrowArgumentException()
        {
            string? message = null;

            var action = () => new Error("CODE", message!);

            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName("message")
                .Which;

            exception.Message.Should().Contain("Error message required.");
        }

        [Fact]
        public void Constructor_WithEmptyMessage_ShouldThrowArgumentException()
        {
            var action = () => new Error("CODE", string.Empty);

            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName("message")
                .Which;

            exception.Message.Should().Contain("Error message required.");
        }

        [Fact]
        public void Constructor_WithWhitespaceMessage_ShouldThrowArgumentException()
        {
            var action = () => new Error("CODE", "   ");

            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName("message")
                .Which;

            exception.Message.Should().Contain("Error message required.");
        }
    #endregion

    #region Error.None
        [Fact]
        public void None_ShouldContainExpectedValues()
        {
            // Assert
            Error.None.Code.Should().Be("NONE");
            Error.None.Message.Should().Be("No error");
            Error.None.Type.Should().Be(ErrorType.None);
        }
    #endregion
}