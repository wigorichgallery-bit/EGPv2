using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.UnitTests.Fixtures.Builders;
using Platform.Identity.Application.UnitTests.Fixtures;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Unit tests for <see cref="AuthenticationChallengeDeliveryRequest"/>.
/// </summary>
public sealed class AuthenticationChallengeDeliveryRequestTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var challenge = AuthenticationChallengeFixture.Create(
            ChallengeSecretFixture.Create());

        var user = UserAccountBuilder.Default.Build();

        const string plainTextSecret = "123456";

        // Act
        var request = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            plainTextSecret);

        // Assert
        request.Challenge.Should().BeSameAs(challenge);
        request.User.Should().BeSameAs(user);
        request.PlainTextSecret.Should().Be(plainTextSecret);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var challenge = AuthenticationChallengeFixture.Create(
            ChallengeSecretFixture.Create());

        var user = UserAccountBuilder.Default.Build();

        var left = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            "123456");

        var right = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
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
        var challenge = AuthenticationChallengeFixture.Create(
            ChallengeSecretFixture.Create());

        var user = UserAccountBuilder.Default.Build();

        var left = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            "111111");

        var right = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
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
        var challenge = AuthenticationChallengeFixture.Create(
            ChallengeSecretFixture.Create());

        var user = UserAccountBuilder.Default.Build();

        var request = new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            "654321");

        // Act
        var (
            aggregate,
            account,
            secret) = request;

        // Assert
        aggregate.Should().BeSameAs(challenge);
        account.Should().BeSameAs(user);
        secret.Should().Be("654321");
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var request = new AuthenticationChallengeDeliveryRequest(
            AuthenticationChallengeFixture.Create(
                ChallengeSecretFixture.Create()),
            UserAccountBuilder.Default.Build(),
            "123456");

        // Act
        var text = request.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationChallengeDeliveryRequest.Challenge));
        text.Should().Contain(nameof(AuthenticationChallengeDeliveryRequest.User));
        text.Should().Contain(nameof(AuthenticationChallengeDeliveryRequest.PlainTextSecret));
    }
}