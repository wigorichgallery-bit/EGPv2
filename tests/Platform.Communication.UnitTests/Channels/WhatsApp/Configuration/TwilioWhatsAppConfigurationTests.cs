using Platform.Communication.Channels.WhatsApp.Configuration;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Configuration;

/// <summary>
/// Contains unit tests for <see cref="TwilioWhatsAppConfiguration"/>.
/// </summary>
public sealed class TwilioWhatsAppConfigurationTests
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
        TwilioWhatsAppConfiguration configuration = new();

        // Assert
        configuration.AccountSid.Should().BeEmpty();
        configuration.AuthToken.Should().BeEmpty();
        configuration.FromNumber.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that all properties
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Properties_Should_BeAssignable()
    {
        // Arrange
        TwilioWhatsAppConfiguration configuration = new();

        // Act
        configuration.AccountSid = "AC123456";
        configuration.AuthToken = "AuthToken";
        configuration.FromNumber = "whatsapp:+628123456789";

        // Assert
        configuration.AccountSid.Should().Be("AC123456");
        configuration.AuthToken.Should().Be("AuthToken");
        configuration.FromNumber.Should().Be("whatsapp:+628123456789");
    }
}