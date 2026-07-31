// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Governance/GovernanceBehaviorTTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Governance;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Governance;

/// <summary>
/// Contains unit tests for <see cref="GovernanceBehaviorT{TRequest, TValue}"/>.
/// </summary>
public sealed class GovernanceBehaviorTTests
{
    /// <summary>
    /// Verifies the pipeline order.
    /// </summary>
    [Fact]
    public void Order_Should_Be200()
    {
        // Arrange
        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(null);

        // Assert
        behavior.Order.Should().Be(200);
    }

    /// <summary>
    /// Verifies HandleAsync throws when request is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Throw_WhenRequestIsNull()
    {
        // Arrange
        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(null);

        Func<Task<Result<int>>> next =
            () => Task.FromResult(Result<int>.Success(100));

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
        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(null);

        // Act
        Func<Task> act =
            async () => await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies non-governed requests continue the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Continue_WhenRequestIsNotGoverned()
    {
        // Arrange
        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(null);

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(123));

        // Act
        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(123);

        next.Verify(
            x => x(),
            Times.Once);
    }

    /// <summary>
    /// Verifies governed requests continue when evaluator is null.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Continue_WhenEvaluatorIsNull()
    {
        // Arrange
        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(null);

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(321));

        // Act
        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(321);

        next.Verify(
            x => x(),
            Times.Once);
    }

    /// <summary>
    /// Verifies failed governance evaluation stops the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_ReturnFailure_WhenEvaluationFails()
    {
        // Arrange
        var evaluator =
            new Mock<IGovernanceEvaluator<GovernanceRequest>>();

        evaluator
            .Setup(x => x.Evaluate(It.IsAny<GovernanceRequest>()))
            .Returns(
                Result.Failure(
                    new Error(
                        "GOVERNANCE",
                        "Rejected")));

        var next =
            new Mock<Func<Task<Result<int>>>>();

        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(
                evaluator.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("GOVERNANCE");
        result.Error.Message.Should().Be("Rejected");

        evaluator.Verify(
            x => x.Evaluate(It.IsAny<GovernanceRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Never);
    }

    /// <summary>
    /// Verifies successful governance evaluation continues the pipeline.
    /// </summary>
    [Fact]
    public async Task HandleAsync_Should_Continue_WhenEvaluationSucceeds()
    {
        // Arrange
        var evaluator =
            new Mock<IGovernanceEvaluator<GovernanceRequest>>();

        evaluator
            .Setup(x => x.Evaluate(It.IsAny<GovernanceRequest>()))
            .Returns(Result.Success());

        var next =
            new Mock<Func<Task<Result<int>>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result<int>.Success(999));

        var behavior =
            new GovernanceBehaviorT<GovernanceRequest, int>(
                evaluator.Object);

        // Act
        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(999);

        evaluator.Verify(
            x => x.Evaluate(It.IsAny<GovernanceRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Once);
    }
}