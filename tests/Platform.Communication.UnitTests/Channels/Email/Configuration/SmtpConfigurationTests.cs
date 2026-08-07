using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.UnitTests.Channels.Email.Configuration;

/// <summary>
/// Contains unit tests for <see cref="SmtpConfiguration"/>.
/// </summary>
public sealed class SmtpConfigurationTests
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
        SmtpConfiguration configuration = new();

        // Assert
        configuration.Host.Should().BeEmpty();
        configuration.Port.Should().Be(0);
        configuration.Username.Should().BeEmpty();
        configuration.Password.Should().BeEmpty();
        configuration.EnableSsl.Should().BeTrue();
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
        SmtpConfiguration configuration = new();

        // Act
        configuration.Host = "smtp.example.com";
        configuration.Port = 587;
        configuration.Username = "user";
        configuration.Password = "password";
        configuration.EnableSsl = false;
        configuration.SenderAddress = "sender@example.com";
        configuration.SenderName = "Sender";

        // Assert
        configuration.Host.Should().Be("smtp.example.com");
        configuration.Port.Should().Be(587);
        configuration.Username.Should().Be("user");
        configuration.Password.Should().Be("password");
        configuration.EnableSsl.Should().BeFalse();
        configuration.SenderAddress.Should().Be("sender@example.com");
        configuration.SenderName.Should().Be("Sender");
    }
}