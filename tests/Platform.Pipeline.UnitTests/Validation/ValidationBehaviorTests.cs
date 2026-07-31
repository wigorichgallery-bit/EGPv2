// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Validation/ValidationBehaviorTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Validation;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Validation;

/// <summary>
/// Dummy request used by the unit tests.
/// </summary>
public sealed record TestRequest(string Value);

/// <summary>
/// Contains unit tests for <see cref="ValidationBehavior{TRequest}"/>.
/// </summary>
public sealed class ValidationBehaviorTests
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
        Action act = () => new ValidationBehavior<TestRequest>(validators!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that the pipeline order is fixed.
    /// </summary>
    [Fact]
    public void Order_Should_Be100()
    {
        // Arrange
        var behavior =
            new ValidationBehavior<TestRequest>(
                Array.Empty<IValidator<TestRequest>>());

        // Act

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
            new ValidationBehavior<TestRequest>(
                Array.Empty<IValidator<TestRequest>>());

        Func<Task<Result>> next =
            () => Task.FromResult(Result.Success());

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                null!,
                CancellationToken.None,
                next);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that HandleAsync throws when next delegate is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenNextIsNull()
    {
        // Arrange
        var behavior =
            new ValidationBehavior<TestRequest>(
                Array.Empty<IValidator<TestRequest>>());

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new TestRequest("A"),
                CancellationToken.None,
                null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that next delegate is invoked when
    /// no validators are registered.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_InvokeNext_WhenNoValidatorsExist()
    {
        // Arrange
        var behavior =
            new ValidationBehavior<TestRequest>(
                Array.Empty<IValidator<TestRequest>>());

        var next = new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("A"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();

        next.Verify(
            x => x(),
            Times.Once);
    }

    /// <summary>
    /// Verifies that next delegate is invoked
    /// when validation succeeds.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_InvokeNext_WhenValidationSucceeds()
    {
        // Arrange
        var validator =
            new Mock<IValidator<TestRequest>>();

        validator.Setup(v =>
                v.Validate(It.IsAny<TestRequest>()))
            .Returns(ValidationResult.Success());

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new ValidationBehavior<TestRequest>(
                [validator.Object]);

        // Act
        var result =
            await behavior.HandleAsync(
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
    /// Verifies that validation failure
    /// stops the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_ReturnFailure_WhenValidatorFails()
    {
        // Arrange
        var validator =
            new Mock<IValidator<TestRequest>>();

        validator.Setup(v =>
                v.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError(
                        "VALIDATION.REQUIRED",
                        "Name required.")
                ]));

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new ValidationBehavior<TestRequest>(
                [validator.Object]);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("ABC"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Name required.");

        validator.Verify(
            x => x.Validate(It.IsAny<TestRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Never);
    }

    /// <summary>
    /// Verifies that multiple validation failures
    /// are aggregated.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_AggregateValidationErrors()
    {
        // Arrange
        var validator1 =
            new Mock<IValidator<TestRequest>>();

        validator1.Setup(v =>
                v.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError(
                        "CODE1",
                        "Error 1")
                ]));

        var validator2 =
            new Mock<IValidator<TestRequest>>();

        validator2.Setup(v =>
                v.Validate(It.IsAny<TestRequest>()))
            .Returns(
                ValidationResult.Failure(
                [
                    new ValidationError(
                        "CODE2",
                        "Error 2")
                ]));

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new ValidationBehavior<TestRequest>(
            [
                validator1.Object,
                validator2.Object
            ]);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("ABC"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("VALIDATION.FAILED");
        result.Error.Message.Should().Be("Error 1; Error 2");

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