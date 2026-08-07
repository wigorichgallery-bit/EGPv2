using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Policies.Models;

/// <summary>
/// Unit tests for <see cref="PolicyEvaluationResult"/>.
/// </summary>
public sealed class PolicyEvaluationResultTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var decision = AuthenticationDecision.Allow();

        // Act
        var result = new PolicyEvaluationResult(
            true,
            true,
            decision);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.ShouldContinue.Should().BeTrue();
        result.Decision.Should().BeSameAs(decision);
    }

    /// <summary>
    /// Verifies Continue returns the expected result.
    /// </summary>
    [Fact]
    public void Continue_Should_Return_Expected_Result()
    {
        // Act
        var result = PolicyEvaluationResult.Continue();

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.ShouldContinue.Should().BeTrue();

        result.Decision.Decision
            .Should()
            .Be(AuthenticationDecisionType.Allow);

        result.Decision.Reason.Should().BeNull();
    }

    /// <summary>
    /// Verifies Stop returns the expected result.
    /// </summary>
    [Fact]
    public void Stop_Should_Return_Expected_Result()
    {
        // Arrange
        var decision =
            AuthenticationDecision.Deny("Denied");

        // Act
        var result =
            PolicyEvaluationResult.Stop(decision);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.ShouldContinue.Should().BeFalse();
        result.Decision.Should().BeSameAs(decision);
    }

    /// <summary>
    /// Verifies Stop throws when the decision is null.
    /// </summary>
    [Fact]
    public void Stop_Should_ThrowArgumentNullException_When_Decision_Is_Null()
    {
        // Act
        var action =
            () => PolicyEvaluationResult.Stop(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("decision");
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var left = new PolicyEvaluationResult(
            true,
            true,
            AuthenticationDecision.Allow());

        var right = new PolicyEvaluationResult(
            true,
            true,
            AuthenticationDecision.Allow());

        // Assert
        left.Should().Be(right);
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies different records are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var left = PolicyEvaluationResult.Continue();

        var right =
            PolicyEvaluationResult.Stop(
                AuthenticationDecision.Deny());

        // Assert
        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }
}