using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.UnitTests.Fixtures;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengeSecretResult"/>.
/// </summary>
public sealed class AuthenticationChallengeSecretResultTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var secret = ChallengeSecretFixture.Create();

        const string plainTextSecret = "123456";

        // Act
        var result = new AuthenticationChallengeSecretResult(
            secret,
            plainTextSecret);

        // Assert
        result.Secret.Should().BeSameAs(secret);
        result.PlainTextSecret.Should().Be(plainTextSecret);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var secret = ChallengeSecretFixture.Create();

        var left = new AuthenticationChallengeSecretResult(
            secret,
            "123456");

        var right = new AuthenticationChallengeSecretResult(
            secret,
            "123456");

        // Assert
        left.Should().Be(right);
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies records with different values are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var secret = ChallengeSecretFixture.Create();

        var left = new AuthenticationChallengeSecretResult(
            secret,
            "111111");

        var right = new AuthenticationChallengeSecretResult(
            secret,
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
        var secret = ChallengeSecretFixture.Create();

        var result = new AuthenticationChallengeSecretResult(
            secret,
            "654321");

        // Act
        var (
            protectedSecret,
            plainTextSecret) = result;

        // Assert
        protectedSecret.Should().BeSameAs(secret);
        plainTextSecret.Should().Be("654321");
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var result = new AuthenticationChallengeSecretResult(
            ChallengeSecretFixture.Create(),
            "123456");

        // Act
        var text = result.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationChallengeSecretResult.Secret));
        text.Should().Contain(nameof(AuthenticationChallengeSecretResult.PlainTextSecret));
    }
}