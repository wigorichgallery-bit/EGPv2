using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

public sealed class ResultOfTTests
{
    [Fact]
    public void Success_WithValidValue_ShouldReturnSuccessfulResult()
    {
        // Arrange
        const string value = "Hello";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeSameAs(Error.None);
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Success_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () => Result<string>.Success(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    [Fact]
    public void Failure_WithValidError_ShouldReturnFailureResult()
    {
        // Arrange
        var error = new Error(
            "VALIDATION",
            "Validation failed.",
            ErrorType.Validation);

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeSameAs(error);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Failure_WithNullError_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () => Result<string>.Failure(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    [Fact]
    public void Failure_ForValueType_ShouldReturnDefaultValue()
    {
        // Arrange
        var error = new Error(
            "ERROR",
            "Operation failed.");

        // Act
        var result = Result<int>.Failure(error);

        // Assert
        result.Value.Should().Be(default(int));
    }

    [Fact]
    public void Success_ShouldPreserveValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        result.Value.Should().Be(42);
    }
}