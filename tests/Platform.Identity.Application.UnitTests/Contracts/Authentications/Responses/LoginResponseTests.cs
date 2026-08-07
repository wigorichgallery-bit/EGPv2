using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Platform.Identity.Application.Contracts.Authentication.Enums;
using Platform.Identity.Application.Contracts.Authentication.Responses;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Responses;

/// <summary>
/// Contains unit tests for <see cref="LoginResponse"/>.
/// </summary>
public sealed class LoginResponseTests
{
    /// <summary>
    /// Verifies the constructor assigns every property correctly.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddMinutes(5);
        var challengeId = Guid.NewGuid();

        var token = new AuthenticationTokenDto(
            "access-token",
            "refresh-token",
            "Bearer",
            3600,
            expiresAt);

        // Act
        var response = new LoginResponse(
            AuthenticationStatus.ChallengeRequired,
            token,
            challengeId,
            AuthenticationChallengeType.EmailOtp,
            AuthenticationChallengePurpose.Login,
            expiresAt);

        // Assert
        response.Status.Should().Be(AuthenticationStatus.ChallengeRequired);
        response.Token.Should().Be(token);
        response.ChallengeId.Should().Be(challengeId);
        response.ChallengeType.Should().Be(AuthenticationChallengeType.EmailOtp);
        response.ChallengePurpose.Should().Be(AuthenticationChallengePurpose.Login);
        response.ChallengeExpiresAtUtc.Should().Be(expiresAt);
    }

    /// <summary>
    /// Verifies nullable properties accept null values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Accept_Null_Optional_Values()
    {
        // Act
        var response = new LoginResponse(
            AuthenticationStatus.Success,
            null,
            null,
            null,
            null,
            null);

        // Assert
        response.Status.Should().Be(AuthenticationStatus.Success);
        response.Token.Should().BeNull();
        response.ChallengeId.Should().BeNull();
        response.ChallengeType.Should().BeNull();
        response.ChallengePurpose.Should().BeNull();
        response.ChallengeExpiresAtUtc.Should().BeNull();
    }

    /// <summary>
    /// Verifies two records with identical values are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var expiresAt = DateTime.UnixEpoch;
        var challengeId = Guid.NewGuid();

        var token = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            expiresAt);

        var left = new LoginResponse(
            AuthenticationStatus.Success,
            token,
            challengeId,
            AuthenticationChallengeType.None,
            AuthenticationChallengePurpose.Login,
            expiresAt);

        var right = new LoginResponse(
            AuthenticationStatus.Success,
            token,
            challengeId,
            AuthenticationChallengeType.None,
            AuthenticationChallengePurpose.Login,
            expiresAt);

        // Assert
        left.Should().Be(right);
        (left == right).Should().BeTrue();
        left.Equals(right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies records with different values are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var left = new LoginResponse(
            AuthenticationStatus.Success,
            null,
            null,
            null,
            null,
            null);

        var right = new LoginResponse(
            AuthenticationStatus.Locked,
            null,
            null,
            null,
            null,
            null);

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
        var expiresAt = DateTime.UnixEpoch;
        var challengeId = Guid.NewGuid();

        var token = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            expiresAt);

        var response = new LoginResponse(
            AuthenticationStatus.Success,
            token,
            challengeId,
            AuthenticationChallengeType.None,
            AuthenticationChallengePurpose.Login,
            expiresAt);

        // Act
        var (
            status,
            responseToken,
            id,
            challengeType,
            challengePurpose,
            challengeExpiresAtUtc) = response;

        // Assert
        status.Should().Be(AuthenticationStatus.Success);
        responseToken.Should().Be(token);
        id.Should().Be(challengeId);
        challengeType.Should().Be(AuthenticationChallengeType.None);
        challengePurpose.Should().Be(AuthenticationChallengePurpose.Login);
        challengeExpiresAtUtc.Should().Be(expiresAt);
    }

    /// <summary>
    /// Verifies the generated string representation contains key values.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Values()
    {
        // Arrange
        var response = new LoginResponse(
            AuthenticationStatus.Locked,
            null,
            null,
            null,
            null,
            null);

        // Act
        var text = response.ToString();

        // Assert
        text.Should().Contain(nameof(LoginResponse.Status));
        text.Should().Contain(nameof(AuthenticationStatus.Locked));
    }
}