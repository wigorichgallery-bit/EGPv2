using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.UnitTests.Channels.Email.Configuration;

/// <summary>
/// Contains unit tests for <see cref="EmailConfiguration"/>.
/// </summary>
public sealed class EmailConfigurationTests
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
        EmailConfiguration configuration = new();

        // Assert
        configuration.Smtp.Should().NotBeNull();
        configuration.MicrosoftGraph.Should().NotBeNull();
        configuration.SendGrid.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that the SMTP configuration
    /// can be initialized.
    /// </summary>
    [Fact]
    public void Smtp_Should_BeInitializable()
    {
        // Arrange
        SmtpConfiguration smtp = new();

        // Act
        EmailConfiguration configuration = new()
        {
            Smtp = smtp
        };

        // Assert
        configuration.Smtp.Should().BeSameAs(smtp);
    }

    /// <summary>
    /// Verifies that the Microsoft Graph configuration
    /// can be initialized.
    /// </summary>
    [Fact]
    public void MicrosoftGraph_Should_BeInitializable()
    {
        // Arrange
        MicrosoftGraphConfiguration graph = new();

        // Act
        EmailConfiguration configuration = new()
        {
            MicrosoftGraph = graph
        };

        // Assert
        configuration.MicrosoftGraph.Should().BeSameAs(graph);
    }

    /// <summary>
    /// Verifies that the SendGrid configuration
    /// can be initialized.
    /// </summary>
    [Fact]
    public void SendGrid_Should_BeInitializable()
    {
        // Arrange
        SendGridConfiguration sendGrid = new();

        // Act
        EmailConfiguration configuration = new()
        {
            SendGrid = sendGrid
        };

        // Assert
        configuration.SendGrid.Should().BeSameAs(sendGrid);
    }
}