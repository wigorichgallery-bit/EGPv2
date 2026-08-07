using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.UnitTests.Channels.Email.Configuration;

/// <summary>
/// Contains unit tests for <see cref="SendGridConfiguration"/>.
/// </summary>
public sealed class SendGridConfigurationTests
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
        SendGridConfiguration configuration = new();

        // Assert
        configuration.ApiKey.Should().BeEmpty();
        configuration.SenderAddress.Should().BeEmpty();
        configuration.SenderName.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that all properties
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Properties_Should_BeAssignable()
    {
        // Arrange
        SendGridConfiguration configuration = new();

        // Act
        configuration.ApiKey = "apikey";
        configuration.SenderAddress = "sender@example.com";
        configuration.SenderName = "Sender";

        // Assert
        configuration.ApiKey.Should().Be("apikey");
        configuration.SenderAddress.Should().Be("sender@example.com");
        configuration.SenderName.Should().Be("Sender");
    }
}