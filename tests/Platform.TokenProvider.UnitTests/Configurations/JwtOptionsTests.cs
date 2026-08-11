// ===========================================
// File Location :
// tests/Platform.TokenProvider.UnitTests/Configuration/JwtOptionsTests.cs
// ===========================================

using Platform.TokenProvider.Configuration;

namespace Platform.TokenProvider.UnitTests.Configuration;

/// <summary>
/// Contains unit tests for <see cref="JwtOptions"/>.
/// </summary>
/// <remarks>
/// These tests verify the default state, configuration section name,
/// and property assignment behavior of the JWT configuration model.
/// </remarks>
public sealed class JwtOptionsTests
{
    /// <summary>
    /// Verifies that <see cref="JwtOptions.SectionName"/> returns
    /// the expected JWT configuration section name.
    /// </summary>
    [Fact]
    public void SectionName_Should_Return_Jwt()
    {
        // Arrange
        const string expected = "Jwt";

        // Act
        var actual = JwtOptions.SectionName;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that a newly created <see cref="JwtOptions"/> instance
    /// initializes all properties with their expected default values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_DefaultValues_When_Created()
    {
        // Arrange & Act
        var options = new JwtOptions();

        // Assert
        options.Issuer.Should().BeEmpty();
        options.Audience.Should().BeEmpty();
        options.SecretKey.Should().BeEmpty();
        options.AccessTokenLifetimeMinutes.Should().Be(0);
        options.RefreshTokenLifetimeDays.Should().Be(0);
    }

    /// <summary>
    /// Verifies that all configurable properties preserve the values
    /// assigned to them.
    /// </summary>
    [Fact]
    public void Properties_Should_Store_AssignedValues_When_Values_Are_Set()
    {
        // Arrange
        const string expectedIssuer = "https://issuer.example.com";
        const string expectedAudience = "egpv2-api";
        const string expectedSecretKey = "test-secret-key";
        const int expectedAccessTokenLifetimeMinutes = 30;
        const int expectedRefreshTokenLifetimeDays = 7;

        var options = new JwtOptions();

        // Act
        options.Issuer = expectedIssuer;
        options.Audience = expectedAudience;
        options.SecretKey = expectedSecretKey;
        options.AccessTokenLifetimeMinutes = expectedAccessTokenLifetimeMinutes;
        options.RefreshTokenLifetimeDays = expectedRefreshTokenLifetimeDays;

        // Assert
        options.Issuer.Should().Be(expectedIssuer);
        options.Audience.Should().Be(expectedAudience);
        options.SecretKey.Should().Be(expectedSecretKey);
        options.AccessTokenLifetimeMinutes.Should()
            .Be(expectedAccessTokenLifetimeMinutes);
        options.RefreshTokenLifetimeDays.Should()
            .Be(expectedRefreshTokenLifetimeDays);
    }
}