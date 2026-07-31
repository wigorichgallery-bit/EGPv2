// ===========================================
// File Location:
// tests/Application/Platform.Pipeline.UnitTests/
// Governance/GovernanceBehaviorTests.cs
// ===========================================

using FluentAssertions;
using Moq;
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Governance;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.Pipeline.UnitTests.Governance;

/// <summary>
/// Dummy governance request.
/// </summary>
/// <summary>
/// Dummy governance request used by unit tests.
/// </summary>
public sealed record GovernanceRequest(
    string Value)
    : IGovernanceRequest
{
    public string GovernancePolicy =>
        "IDENTITY.USER.CREATE";

    public string Resource =>
        "User";

    public string Action =>
        "Create";
}
/// <summary>
/// Contains unit tests for <see cref="GovernanceBehavior{TRequest}"/>.
/// </summary>
public sealed class GovernanceBehaviorTests
{
    [Fact]
    public void Order_Should_Be200()
    {
        var behavior =
            new GovernanceBehavior<GovernanceRequest>(null);

        behavior.Order.Should().Be(200);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_WhenRequestIsNull()
    {
        var behavior =
            new GovernanceBehavior<GovernanceRequest>(null);

        Func<Task<Result>> next =
            () => Task.FromResult(Result.Success());

        Func<Task> act =
            async () => await behavior.HandleAsync(
                null!,
                CancellationToken.None,
                next);

        await act.Should()
            .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_WhenNextIsNull()
    {
        var behavior =
            new GovernanceBehavior<GovernanceRequest>(null);

        Func<Task> act =
            async () => await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                null!);

        await act.Should()
            .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task HandleAsync_Should_Continue_WhenRequestIsNotGoverned()
    {
        var behavior =
            new GovernanceBehavior<GovernanceRequest>(null);

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        result.IsSuccess.Should().BeTrue();

        next.Verify(
            x => x(),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Continue_WhenEvaluatorIsNull()
    {
        var behavior =
            new GovernanceBehavior<GovernanceRequest>(null);

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        result.IsSuccess.Should().BeTrue();

        next.Verify(
            x => x(),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnFailure_WhenEvaluationFails()
    {
        var evaluator =
            new Mock<IGovernanceEvaluator<GovernanceRequest>>();

        var failure =
            Result.Failure(
                new Error(
                    "GOVERNANCE",
                    "Rejected"));

        evaluator
            .Setup(x => x.Evaluate(It.IsAny<GovernanceRequest>()))
            .Returns(failure);

        var next =
            new Mock<Func<Task<Result>>>();

        var behavior =
            new GovernanceBehavior<GovernanceRequest>(
                evaluator.Object);

        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("GOVERNANCE");

        next.Verify(
            x => x(),
            Times.Never);

        evaluator.Verify(
            x => x.Evaluate(It.IsAny<GovernanceRequest>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Continue_WhenEvaluationSucceeds()
    {
        var evaluator =
            new Mock<IGovernanceEvaluator<GovernanceRequest>>();

        evaluator
            .Setup(x => x.Evaluate(It.IsAny<GovernanceRequest>()))
            .Returns(Result.Success());

        var next =
            new Mock<Func<Task<Result>>>();

        next.Setup(x => x())
            .ReturnsAsync(Result.Success());

        var behavior =
            new GovernanceBehavior<GovernanceRequest>(
                evaluator.Object);

        var result =
            await behavior.HandleAsync(
                new GovernanceRequest("REQ"),
                CancellationToken.None,
                next.Object);

        result.IsSuccess.Should().BeTrue();

        evaluator.Verify(
            x => x.Evaluate(It.IsAny<GovernanceRequest>()),
            Times.Once);

        next.Verify(
            x => x(),
            Times.Once);
    }
}