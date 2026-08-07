using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Policies.Models;

/// <summary>
/// Unit tests for <see cref="AuthenticationDecision"/>.
/// </summary>
public sealed class AuthenticationDecisionTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_Properties()
    {
        // Arrange
        const string reason = "Reason";

        // Act
        var decision = new AuthenticationDecision(
            AuthenticationDecisionType.Allow,
            reason);

        // Assert
        decision.Decision.Should().Be(AuthenticationDecisionType.Allow);
        decision.Reason.Should().Be(reason);
    }

    /// <summary>
    /// Verifies Allow creates the expected decision.
    /// </summary>
    [Fact]
    public void Allow_Should_Return_Allow_Decision()
    {
        var decision = AuthenticationDecision.Allow();

        decision.Decision.Should().Be(AuthenticationDecisionType.Allow);
        decision.Reason.Should().BeNull();
    }

    /// <summary>
    /// Verifies RequireVerification creates the expected decision.
    /// </summary>
    [Fact]
    public void RequireVerification_Should_Return_Expected_Decision()
    {
        var decision =
            AuthenticationDecision.RequireVerification("Verify");

        decision.Decision.Should()
            .Be(AuthenticationDecisionType.RequireVerification);

        decision.Reason.Should().Be("Verify");
    }

    /// <summary>
    /// Verifies RequireChallenge creates the expected decision.
    /// </summary>
    [Fact]
    public void RequireChallenge_Should_Return_Expected_Decision()
    {
        var decision =
            AuthenticationDecision.RequireChallenge("Challenge");

        decision.Decision.Should()
            .Be(AuthenticationDecisionType.RequireChallenge);

        decision.Reason.Should().Be("Challenge");
    }

    /// <summary>
    /// Verifies RequirePasswordReset creates the expected decision.
    /// </summary>
    [Fact]
    public void RequirePasswordReset_Should_Return_Expected_Decision()
    {
        var decision =
            AuthenticationDecision.RequirePasswordReset("Reset");

        decision.Decision.Should()
            .Be(AuthenticationDecisionType.RequirePasswordReset);

        decision.Reason.Should().Be("Reset");
    }

    /// <summary>
    /// Verifies Deny creates the expected decision.
    /// </summary>
    [Fact]
    public void Deny_Should_Return_Expected_Decision()
    {
        var decision =
            AuthenticationDecision.Deny("Denied");

        decision.Decision.Should()
            .Be(AuthenticationDecisionType.Deny);

        decision.Reason.Should().Be("Denied");
    }

    /// <summary>
    /// Verifies LockAccount creates the expected decision.
    /// </summary>
    [Fact]
    public void LockAccount_Should_Return_Expected_Decision()
    {
        var decision =
            AuthenticationDecision.LockAccount("Locked");

        decision.Decision.Should()
            .Be(AuthenticationDecisionType.LockAccount);

        decision.Reason.Should().Be("Locked");
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        var left = new AuthenticationDecision(
            AuthenticationDecisionType.Allow,
            "Reason");

        var right = new AuthenticationDecision(
            AuthenticationDecisionType.Allow,
            "Reason");

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
        var left = AuthenticationDecision.Allow();

        var right = AuthenticationDecision.Deny();

        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }
}