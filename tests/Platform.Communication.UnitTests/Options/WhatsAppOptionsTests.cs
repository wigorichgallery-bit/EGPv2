using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.Options;

/// <summary>
/// Contains unit tests for <see cref="WhatsAppOptions"/>.
/// </summary>
public sealed class WhatsAppOptionsTests
{
    /// <summary>
    /// Verifies that the constructor initializes
    /// all nested configurations.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeConfigurations()
    {
        // Arrange

        // Act
        WhatsAppOptions options = new();

        // Assert
        options.MetaCloud.Should().NotBeNull();
        options.Twilio.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the provider property
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Provider_Should_BeAssignable()
    {
        // Arrange
        WhatsAppOptions options = new();

        // Act
        options.Provider = WhatsAppProviderType.Twilio;

        // Assert
        options.Provider.Should().Be(WhatsAppProviderType.Twilio);
    }

    /// <summary>
    /// Verifies that the Meta Cloud configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void MetaCloud_Should_BeAssignable()
    {
        // Arrange
        WhatsAppOptions options = new();
        MetaCloudWhatsAppConfiguration configuration = new();

        // Act
        options.MetaCloud = configuration;

        // Assert
        options.MetaCloud.Should().BeSameAs(configuration);
    }

    /// <summary>
    /// Verifies that the Twilio configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Twilio_Should_BeAssignable()
    {
        // Arrange
        WhatsAppOptions options = new();
        TwilioWhatsAppConfiguration configuration = new();

        // Act
        options.Twilio = configuration;

        // Assert
        options.Twilio.Should().BeSameAs(configuration);
    }
}