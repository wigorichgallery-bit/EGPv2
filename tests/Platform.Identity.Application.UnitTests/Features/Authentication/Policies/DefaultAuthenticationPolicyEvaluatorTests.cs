using FluentAssertions;
using Moq;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Application.Features.Authentication.Policies;
using Platform.Identity.Application.Features.Authentication.Policies.Contracts;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Platform.Identity.Application.UnitTests.Fixtures.Builders;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Policies;

/// <summary>
/// Unit tests for <see cref="DefaultAuthenticationPolicyEvaluator"/>.
/// </summary>
public sealed class DefaultAuthenticationPolicyEvaluatorTests
{
    private static AuthenticationContext CreateContext()
    {
        return new AuthenticationContext(
            UserAccountBuilder.Default.Build(),
            new LoginRequest("john", "password"),
            DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_Policies_Are_Null()
    {
        // Act
        Action act = () => new DefaultAuthenticationPolicyEvaluator(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("policies");
    }

    [Fact]
    public async Task EvaluateAsync_Should_ThrowArgumentNullException_When_Context_Is_Null()
    {
        // Arrange
        var sut = new DefaultAuthenticationPolicyEvaluator([]);

        // Act
        Func<Task> act = () => sut.EvaluateAsync(null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("context");
    }

    [Fact]
    public async Task EvaluateAsync_Should_ThrowOperationCanceledException_When_Cancellation_Is_Requested()
    {
        // Arrange
        var policy = new Mock<IAuthenticationPolicy>();

        var sut = new DefaultAuthenticationPolicyEvaluator(
            new[]
            {
            policy.Object
            });

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var context = CreateContext();

        // Act
        Func<Task> act =
            () => sut.EvaluateAsync(
                context,
                cts.Token);

        // Assert
        await act.Should()
            .ThrowAsync<OperationCanceledException>();

        policy.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task EvaluateAsync_Should_ReturnContinue_When_No_Policies_Are_Registered()
    {
        // Arrange
        var sut = new DefaultAuthenticationPolicyEvaluator([]);

        // Act
        var result = await sut.EvaluateAsync(CreateContext());

        // Assert
        result.Should().BeEquivalentTo(
            PolicyEvaluationResult.Continue());
    }

    [Fact]
    public async Task EvaluateAsync_Should_Invoke_All_Policies_When_All_Continue()
    {
        // Arrange
        var first = new Mock<IAuthenticationPolicy>();
        var second = new Mock<IAuthenticationPolicy>();

        first.Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Continue());

        second.Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Continue());

        var sut = new DefaultAuthenticationPolicyEvaluator(
            [first.Object, second.Object]);

        // Act
        var result = await sut.EvaluateAsync(CreateContext());

        // Assert
        result.Should().BeEquivalentTo(
            PolicyEvaluationResult.Continue());

        first.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        second.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task EvaluateAsync_Should_Stop_When_A_Policy_Returns_Stop()
    {
        // Arrange
        var stopResult =
            PolicyEvaluationResult.Stop(
                AuthenticationDecision.Deny("Denied"));

        var first = new Mock<IAuthenticationPolicy>();
        var second = new Mock<IAuthenticationPolicy>();

        first.Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(stopResult);

        var sut = new DefaultAuthenticationPolicyEvaluator(
            [first.Object, second.Object]);

        // Act
        var result = await sut.EvaluateAsync(CreateContext());

        // Assert
        result.Should().BeSameAs(stopResult);

        first.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        second.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task EvaluateAsync_Should_Stop_Executing_Remaining_Policies_After_Stop()
    {
        // Arrange
        var sequence = new MockSequence();

        var first = new Mock<IAuthenticationPolicy>();
        var second = new Mock<IAuthenticationPolicy>();
        var third = new Mock<IAuthenticationPolicy>();

        first.InSequence(sequence)
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(PolicyEvaluationResult.Continue());

        second.InSequence(sequence)
            .Setup(x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.Deny()));

        var sut = new DefaultAuthenticationPolicyEvaluator(
            [first.Object, second.Object, third.Object]);

        // Act
        await sut.EvaluateAsync(CreateContext());

        // Assert
        first.VerifyAll();
        second.VerifyAll();

        third.Verify(
            x => x.EvaluateAsync(
                It.IsAny<AuthenticationContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}