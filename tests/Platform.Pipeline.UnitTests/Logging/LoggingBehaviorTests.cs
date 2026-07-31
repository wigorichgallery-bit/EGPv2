// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Logging/LoggingBehaviorTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Logging;
using Platform.Pipeline.Models;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Logging;

/// <summary>
/// Dummy request used by the unit tests.
/// </summary>
public sealed record TestRequest(string Value);

/// <summary>
/// Contains unit tests for <see cref="LoggingBehavior{TRequest}"/>.
/// </summary>
public sealed class LoggingBehaviorTests
{
    /// <summary>
    /// Verifies that the constructor throws when logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenLoggerIsNull()
    {
        // Arrange
        IExecutionLogger? logger = null;

        // Act
        Action act =
            () => new LoggingBehavior<TestRequest>(logger!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies the pipeline order.
    /// </summary>
    [Fact]
    public void Order_Should_Be400()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Assert
        behavior.Order.Should().Be(400);
    }

    /// <summary>
    /// Verifies HandleAsync throws when request is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenRequestIsNull()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

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
            .ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies HandleAsync throws when next delegate is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenNextIsNull()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies successful execution is logged.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_LogSuccess()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();

        logger.Verify(
            x => x.LogAsync(
                It.Is<ExecutionLogEntry>(e =>
                    e.RequestName == nameof(TestRequest)
                    && e.Success
                    && e.ErrorCode == null
                    && e.ExceptionType == null
                    && e.DurationMs >= 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies failed result is logged.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_LogFailure()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var failure =
            Result.Failure(
                new Error(
                    "ERROR",
                    "Failure"));

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(failure);

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsFailure.Should().BeTrue();

        logger.Verify(
            x => x.LogAsync(
                It.Is<ExecutionLogEntry>(e =>
                    e.RequestName == nameof(TestRequest)
                    && !e.Success
                    && e.ErrorCode == "ERROR"
                    && e.ExceptionType == null
                    && e.DurationMs >= 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies exceptions are logged then rethrown.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_LogException()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ThrowsAsync(
                new InvalidOperationException("Boom"));

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>();

        logger.Verify(
            x => x.LogAsync(
                It.Is<ExecutionLogEntry>(e =>
                    e.RequestName == nameof(TestRequest)
                    && !e.Success
                    && e.ErrorCode == null
                    && e.ExceptionType == nameof(InvalidOperationException)
                    && e.DurationMs >= 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies logger failures never affect successful execution.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_IgnoreLoggerException_OnSuccess()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        logger.Setup(x =>
                x.LogAsync(
                    It.IsAny<ExecutionLogEntry>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Verifies logger failures never affect failed results.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_IgnoreLoggerException_OnFailure()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        logger.Setup(x =>
                x.LogAsync(
                    It.IsAny<ExecutionLogEntry>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var failure =
            Result.Failure(
                new Error(
                    "ERROR",
                    "Failure"));

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(failure);

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ERROR");
    }

    /// <summary>
    /// Verifies the original exception is rethrown
    /// even if logging also fails.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_RethrowOriginalException_WhenLoggerAlsoThrows()
    {
        // Arrange
        var logger =
            new Mock<IExecutionLogger>();

        logger.Setup(x =>
                x.LogAsync(
                    It.IsAny<ExecutionLogEntry>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Logger"));

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ThrowsAsync(
                new InvalidOperationException("Original"));

        var behavior =
            new LoggingBehavior<TestRequest>(
                logger.Object);

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Original");
    }
}