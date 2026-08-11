using Platform.Communication.Channels.WhatsApp.Configuration;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Configuration;

/// <summary>
/// Contains unit tests for <see cref="WhatsAppConfiguration"/>.
/// </summary>
public sealed class WhatsAppConfigurationTests
{
    /// <summary>
    /// Verifies that the constructor initializes
    /// all nested configurations.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeNestedConfigurations()
    {
        // Arrange

        // Act
        WhatsAppConfiguration configuration = new();

        // Assert
        configuration.MetaCloud.Should().NotBeNull();
        configuration.Twilio.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the Meta Cloud configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void MetaCloud_Should_BeAssignable()
    {
        // Arrange
        WhatsAppConfiguration configuration = new();
        MetaCloudWhatsAppConfiguration metaCloud = new();

        // Act
        configuration.MetaCloud = metaCloud;

        // Assert
        configuration.MetaCloud.Should().BeSameAs(metaCloud);
    }

    /// <summary>
    /// Verifies that the Twilio configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Twilio_Should_BeAssignable()
    {
        // Arrange
        WhatsAppConfiguration configuration = new();
        TwilioWhatsAppConfiguration twilio = new();

        // Act
        configuration.Twilio = twilio;

        // Assert
        configuration.Twilio.Should().BeSameAs(twilio);
    }
}