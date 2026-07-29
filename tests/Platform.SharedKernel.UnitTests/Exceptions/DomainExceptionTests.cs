using FluentAssertions;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Exceptions;

public sealed class DomainExceptionTests
{
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

    [Fact]
    public void Constructor_WithEmptyErrorCode_ShouldThrowArgumentException()
    {
        // Act
        var action = () => new DomainException(string.Empty, "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorCode")
            .Which;

        exception.Message.Should().Contain("ErrorCode required.");
    }

    [Fact]
    public void Constructor_WithWhitespaceErrorCode_ShouldThrowArgumentException()
    {
        // Act
        var action = () => new DomainException("   ", "message");

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("errorCode")
            .Which;

        exception.Message.Should().Contain("ErrorCode required.");
    }

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

    [Fact]
    public void Constructor_ShouldInitializeWithoutInnerException()
    {
        // Act
        var exception = new DomainException(
            "DOMAIN_ERROR",
            "Invariant failed.");

        // Assert
        exception.InnerException.Should().BeNull();
    }
}