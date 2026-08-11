using Platform.Communication.Channels.WhatsApp.Configuration;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Configuration;

/// <summary>
/// Contains unit tests for <see cref="MetaCloudConfiguration"/>.
/// </summary>
public sealed class MetaCloudConfigurationTests
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
        MetaCloudWhatsAppConfiguration configuration = new();

        // Assert
        configuration.AccessToken.Should().BeEmpty();
        configuration.PhoneNumberId.Should().BeEmpty();
        configuration.BusinessAccountId.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that all properties
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Properties_Should_BeAssignable()
    {
        // Arrange
        MetaCloudWhatsAppConfiguration configuration = new();

        // Act
        configuration.AccessToken = "AccessToken";
        configuration.PhoneNumberId = "PhoneNumberId";
        configuration.BusinessAccountId = "BusinessAccountId";

        // Assert
        configuration.AccessToken.Should().Be("AccessToken");
        configuration.PhoneNumberId.Should().Be("PhoneNumberId");
        configuration.BusinessAccountId.Should().Be("BusinessAccountId");
    }
}