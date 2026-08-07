using Platform.Communication.Channels.Sms.Configuration;

namespace Platform.Communication.UnitTests.Channels.Sms.Configuration;

/// <summary>
/// Contains unit tests for <see cref="VonageSmsConfiguration"/>.
/// </summary>
public sealed class VonageSmsConfigurationTests
{
    /// <summary>
    /// Verifies that the constructor initializes
    /// default values.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeDefaultValues()
    {
        // Arrange

        // Act
        VonageSmsConfiguration configuration = new();

        // Assert
        configuration.ApiKey.Should().BeEmpty();
        configuration.ApiSecret.Should().BeEmpty();
        configuration.From.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that all properties
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Properties_Should_BeAssignable()
    {
        // Arrange
        VonageSmsConfiguration configuration = new();

        // Act
        configuration.ApiKey = "ApiKey";
        configuration.ApiSecret = "ApiSecret";
        configuration.From = "Platform";

        // Assert
        configuration.ApiKey.Should().Be("ApiKey");
        configuration.ApiSecret.Should().Be("ApiSecret");
        configuration.From.Should().Be("Platform");
    }
}