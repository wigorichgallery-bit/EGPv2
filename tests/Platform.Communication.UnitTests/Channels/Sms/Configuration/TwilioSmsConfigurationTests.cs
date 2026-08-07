using Platform.Communication.Channels.Sms.Configuration;

namespace Platform.Communication.UnitTests.Channels.Sms.Configuration;

/// <summary>
/// Contains unit tests for <see cref="TwilioSmsConfiguration"/>.
/// </summary>
public sealed class TwilioSmsConfigurationTests
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
        TwilioSmsConfiguration configuration = new();

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
        TwilioSmsConfiguration configuration = new();

        // Act
        configuration.AccountSid = "AC123456";
        configuration.AuthToken = "AuthToken";
        configuration.FromNumber = "+628123456789";

        // Assert
        configuration.AccountSid.Should().Be("AC123456");
        configuration.AuthToken.Should().Be("AuthToken");
        configuration.FromNumber.Should().Be("+628123456789");
    }
}