using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.UnitTests.Channels.Email.Configuration;

/// <summary>
/// Contains unit tests for <see cref="MicrosoftGraphConfiguration"/>.
/// </summary>
public sealed class MicrosoftGraphConfigurationTests
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
        MicrosoftGraphConfiguration configuration = new();

        // Assert
        configuration.TenantId.Should().BeEmpty();
        configuration.ClientId.Should().BeEmpty();
        configuration.ClientSecret.Should().BeEmpty();
        configuration.UserId.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that all properties
    /// can be assigned.
    /// </summary>
    [Fact]
    public void Properties_Should_BeAssignable()
    {
        // Arrange
        MicrosoftGraphConfiguration configuration = new();

        // Act
        configuration.TenantId = "tenant";
        configuration.ClientId = "client";
        configuration.ClientSecret = "secret";
        configuration.UserId = "user";

        // Assert
        configuration.TenantId.Should().Be("tenant");
        configuration.ClientId.Should().Be("client");
        configuration.ClientSecret.Should().Be("secret");
        configuration.UserId.Should().Be("user");
    }
}