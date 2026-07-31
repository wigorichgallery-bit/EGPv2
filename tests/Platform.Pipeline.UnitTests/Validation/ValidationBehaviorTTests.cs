// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Validation/ValidationBehaviorTTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Validation;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Validation;

/// <summary>
/// Contains unit tests for <see cref="ValidationBehaviorT{TRequest, TValue}"/>.
/// </summary>
public sealed class ValidationBehaviorTTests
{


    /// <summary>
    /// Verifies that the constructor throws when validators are null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenValidatorsAreNull()
    {
        // Arrange
        IEnumerable<IValidator<TestRequest>>? validators = null;

        // Act
        Action act = () =>
            new ValidationBehaviorT<TestRequest, int>(validators!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies the pipeline order.
    /// </summary>
    [Fact]
    public void Order_Should_Be100()
    {
        // Arrange
        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                Array.Empty<IValidator<TestRequest>>());

        // Assert
        behavior.Order.Should().Be(100);
    }

    /// <summary>
    /// Verifies that HandleAsync throws when request is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenRequestIsNull()
    {
        // Arrange
        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                Array.Empty<IValidator<TestRequest>>());

        Func<Task<Result<int>>> next =
            () => Task.FromResult(Result<int>.Success(123));

        // Act
        Func<Task> act = async () =>
            await behavior.HandleAsync(
                null!,
                CancellationToken.None,
                next);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies that HandleAsync throws when next delegate is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenNextIsNull()
    {
        // Arrange
        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                Array.Empty<IValidator<TestRequest>>());

        // Act
        Func<Task> act = async () =>
            await behavior.HandleAsync(
                new TestRequest("ABC"),
                CancellationToken.None,
                null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies that next is invoked when no validators exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_InvokeNext_WhenNoValidatorsExist()
    {
        // Arrange
        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                Array.Empty<IValidator<TestRequest>>());

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(10));

        // Act
        var result = await behavior.HandleAsync(
            new TestRequest("ABC"),
            CancellationToken.None,
            next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();

        next.Verify(x => x(), Times.Once);
    }

    /// <summary>
    /// Verifies that validation success continues the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_InvokeNext_WhenValidationSucceeds()
    {
        // Arrange
        var validator =
            new Mock<IValidator<TestRequest>>();

        validator.Setup(x =>
                x.Validate(It.IsAny<TestRequest>()))
            .Returns(ValidationResult.Success());

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(100));

        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                new[] { validator.Object });

        // Act
        var result = await behavior.HandleAsync(
            new TestRequest("ABC"),
            CancellationToken.None,
            next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();

        validator.Verify(
            x => x.Validate(It.IsAny<TestRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Once);
    }

    /// <summary>
    /// Verifies that validation failure stops the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_ReturnFailure_WhenValidatorFails()
    {
        // Arrange
        var validator =
            new Mock<IValidator<TestRequest>>();

        validator.Setup(x =>
                x.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError(
                        "CODE",
                        "Validation failed.")
                ]));

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(1));

        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                new[] { validator.Object });

        // Act
        var result = await behavior.HandleAsync(
            new TestRequest("ABC"),
            CancellationToken.None,
            next.Object);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Validation failed.");

        validator.Verify(
            x => x.Validate(It.IsAny<TestRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Never);
    }

    /// <summary>
    /// Verifies that validation errors from multiple validators are aggregated.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_AggregateValidationErrors()
    {
        // Arrange
        var validator1 =
            new Mock<IValidator<TestRequest>>();

        validator1.Setup(x =>
                x.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError("A", "Error A")
                ]));

        var validator2 =
            new Mock<IValidator<TestRequest>>();

        validator2.Setup(x =>
                x.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError("B", "Error B")
                ]));

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(1));

        var behavior =
            new ValidationBehaviorT<TestRequest, int>(
                new[]
                {
                    validator1.Object,
                    validator2.Object
                });

        // Act
        var result = await behavior.HandleAsync(
            new TestRequest("ABC"),
            CancellationToken.None,
            next.Object);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Error A; Error B");

        validator1.Verify(
            x => x.Validate(It.IsAny<TestRequest>()),
            Times.Once);

        validator2.Verify(
            x => x.Validate(It.IsAny<TestRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Never);
    }
}