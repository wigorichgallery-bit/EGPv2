using Platform.Communication.Channels.Sms.Configuration;

namespace Platform.Communication.UnitTests.Channels.Sms.Configuration;

/// <summary>
/// Contains unit tests for <see cref="SmsConfiguration"/>.
/// </summary>
public sealed class SmsConfigurationTests
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
        SmsConfiguration configuration = new();

        // Assert
        configuration.Twilio.Should().NotBeNull();
        configuration.Vonage.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the Twilio configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Twilio_Should_BeAssignable()
    {
        // Arrange
        SmsConfiguration configuration = new();
        TwilioSmsConfiguration twilio = new();

        // Act
        configuration.Twilio = twilio;

        // Assert
        configuration.Twilio.Should().BeSameAs(twilio);
    }

    /// <summary>
    /// Verifies that the Vonage configuration
    /// property can be assigned.
    /// </summary>
    [Fact]
    public void Vonage_Should_BeAssignable()
    {
        // Arrange
        SmsConfiguration configuration = new();
        VonageSmsConfiguration vonage = new();

        // Act
        configuration.Vonage = vonage;

        // Assert
        configuration.Vonage.Should().BeSameAs(vonage);
    }
}