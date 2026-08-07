using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.ValueObjects;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengeBuildResult"/>.
/// </summary>
public sealed class AuthenticationChallengeBuildResultTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        ChallengeSecret challengeSecret = CreateChallengeSecret();

        var challenge =
            AuthenticationChallengeFixture.Create(challengeSecret);

        const string plainTextSecret = "123456";

        // Act
        var result = new AuthenticationChallengeBuildResult(
            challenge,
            plainTextSecret);

        // Assert
        result.Challenge.Should().BeSameAs(challenge);
        result.PlainTextSecret.Should().Be(plainTextSecret);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        ChallengeSecret challengeSecret = CreateChallengeSecret();

        var challenge =
            AuthenticationChallengeFixture.Create(challengeSecret);

        var left = new AuthenticationChallengeBuildResult(
            challenge,
            "123456");

        var right = new AuthenticationChallengeBuildResult(
            challenge,
            "123456");

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
        ChallengeSecret challengeSecret = CreateChallengeSecret();

        var challenge =
            AuthenticationChallengeFixture.Create(challengeSecret);

        var left = new AuthenticationChallengeBuildResult(
            challenge,
            "111111");

        var right = new AuthenticationChallengeBuildResult(
            challenge,
            "222222");

        // Assert
        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies deconstruction returns all property values.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_Return_All_Property_Values()
    {
        // Arrange
        ChallengeSecret challengeSecret = CreateChallengeSecret();

        var challenge =
            AuthenticationChallengeFixture.Create(challengeSecret);

        var result = new AuthenticationChallengeBuildResult(
            challenge,
            "654321");

        // Act
        var (
            aggregate,
            plainTextSecret) = result;

        // Assert
        aggregate.Should().BeSameAs(challenge);
        plainTextSecret.Should().Be("654321");
    }

    /// <summary>
    /// Verifies the string representation contains significant values.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        ChallengeSecret challengeSecret = CreateChallengeSecret();

        var challenge =
            AuthenticationChallengeFixture.Create(challengeSecret);

        var result = new AuthenticationChallengeBuildResult(
            challenge,
            "123456");

        // Act
        var text = result.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationChallengeBuildResult.Challenge));
        text.Should().Contain(nameof(AuthenticationChallengeBuildResult.PlainTextSecret));
    }

    /// <summary>
    /// Creates a valid <see cref="ChallengeSecret"/>.
    /// Replace this implementation with the shared fixture/helper
    /// used by the solution.
    /// </summary>
    private static ChallengeSecret CreateChallengeSecret()
    {
        return ChallengeSecretFixture.Create();
        // throw new NotImplementedException(
        //     "Replace with the shared ChallengeSecret fixture/helper.");
    }
}