using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Dtos;

/// <summary>
/// Unit tests for <see cref="AuthenticationTokenDto"/>.
/// </summary>
public sealed class AuthenticationTokenDtoTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddHours(1);

        // Act
        var dto = new AuthenticationTokenDto(
            "access-token",
            "refresh-token",
            "Bearer",
            3600,
            expiresAt);

        // Assert
        dto.AccessToken.Should().Be("access-token");
        dto.RefreshToken.Should().Be("refresh-token");
        dto.TokenType.Should().Be("Bearer");
        dto.ExpiresIn.Should().Be(3600);
        dto.ExpiresAtUtc.Should().Be(expiresAt);
    }

    /// <summary>
    /// Verifies two identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var expiresAt = DateTime.UnixEpoch;

        var left = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            expiresAt);

        var right = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            expiresAt);

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
        var left = new AuthenticationTokenDto(
            "access-1",
            "refresh",
            "Bearer",
            3600,
            DateTime.UnixEpoch);

        var right = new AuthenticationTokenDto(
            "access-2",
            "refresh",
            "Bearer",
            3600,
            DateTime.UnixEpoch);

        // Assert
        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies deconstruction returns all values.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_Return_All_Property_Values()
    {
        // Arrange
        var expiresAt = DateTime.UnixEpoch;

        var dto = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            expiresAt);

        // Act
        var (
            accessToken,
            refreshToken,
            tokenType,
            expiresIn,
            expiresAtUtc) = dto;

        // Assert
        accessToken.Should().Be("access");
        refreshToken.Should().Be("refresh");
        tokenType.Should().Be("Bearer");
        expiresIn.Should().Be(3600);
        expiresAtUtc.Should().Be(expiresAt);
    }

    /// <summary>
    /// Verifies the string representation contains important property values.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Values()
    {
        // Arrange
        var dto = new AuthenticationTokenDto(
            "access",
            "refresh",
            "Bearer",
            3600,
            DateTime.UnixEpoch);

        // Act
        var text = dto.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationTokenDto.AccessToken));
        text.Should().Contain(nameof(AuthenticationTokenDto.RefreshToken));
        text.Should().Contain(nameof(AuthenticationTokenDto.TokenType));
        text.Should().Contain("Bearer");
    }
}