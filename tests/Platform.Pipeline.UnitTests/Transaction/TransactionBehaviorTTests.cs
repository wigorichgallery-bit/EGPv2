// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Transaction/TransactionBehaviorTTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Transaction;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Transaction;

/// <summary>
/// Contains unit tests for <see cref="TransactionBehaviorT{TRequest, TValue}"/>.
/// </summary>
public sealed class TransactionBehaviorTTests
{
    /// <summary>
    /// Verifies that the constructor throws when unit of work is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_WhenUnitOfWorkIsNull()
    {
        // Arrange
        IUnitOfWork? unitOfWork = null;

        // Act
        Action act =
            () => new TransactionBehaviorT<TestRequest, int>(unitOfWork!);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies the pipeline order.
    /// </summary>
    [Fact]
    public void Order_Should_Be300()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

        // Assert
        behavior.Order.Should().Be(300);
    }

    /// <summary>
    /// Verifies that HandleAsync throws when request is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenRequestIsNull()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

        Func<Task<Result<int>>> next =
            () => Task.FromResult(Result<int>.Success(123));

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
    /// Verifies that HandleAsync throws when next delegate is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenNextIsNull()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

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
    /// Verifies that successful execution commits the transaction.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Commit_WhenNextSucceeds()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(100));

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(100);

        next.Verify(
            x => x(),
            Times.Once);

        unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.RollbackAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies that failed execution rolls back the transaction.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Rollback_WhenResultFails()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var failure =
            Result<int>.Failure(
                new Error(
                    "ERROR",
                    "Failure"));

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(failure);

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ERROR");
        result.Error.Message.Should().Be("Failure");

        next.Verify(
            x => x(),
            Times.Once);

        unitOfWork.Verify(
            x => x.RollbackAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies that exceptions roll back the transaction and are rethrown.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Rollback_WhenNextThrows()
    {
        // Arrange
        var unitOfWork =
            new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ThrowsAsync(new InvalidOperationException("Boom"));

        var behavior =
            new TransactionBehaviorT<TestRequest, int>(
                unitOfWork.Object);

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new TestRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Boom");

        next.Verify(
            x => x(),
            Times.Once);

        unitOfWork.Verify(
            x => x.RollbackAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}