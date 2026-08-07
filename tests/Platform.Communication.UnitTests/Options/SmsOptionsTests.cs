using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.UnitTests.Options;

/// <summary>
/// Contains unit tests for <see cref="SmsOptions"/>.
/// </summary>
public sealed class SmsOptionsTests
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
        SmsOptions options = new();

        // Assert
        options.Twilio.Should().NotBeNull();
        options.Vonage.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the provider property
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Provider_Should_BeAssignable()
    {
        // Arrange
        SmsOptions options = new();

        // Act
        options.Provider = SmsProviderType.Vonage;

        // Assert
        options.Provider.Should().Be(SmsProviderType.Vonage);
    }

    /// <summary>
    /// Verifies that the Twilio configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Twilio_Should_BeAssignable()
    {
        // Arrange
        SmsOptions options = new();
        TwilioSmsConfiguration configuration = new();

        // Act
        options.Twilio = configuration;

        // Assert
        options.Twilio.Should().BeSameAs(configuration);
    }

    /// <summary>
    /// Verifies that the Vonage configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Vonage_Should_BeAssignable()
    {
        // Arrange
        SmsOptions options = new();
        VonageSmsConfiguration configuration = new();

        // Act
        options.Vonage = configuration;

        // Assert
        options.Vonage.Should().BeSameAs(configuration);
    }
}