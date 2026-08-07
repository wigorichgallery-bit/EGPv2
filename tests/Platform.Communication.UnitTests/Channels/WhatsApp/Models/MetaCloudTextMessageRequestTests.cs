using Platform.Communication.Channels.WhatsApp.Models;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Models;

/// <summary>
/// Contains unit tests for <see cref="MetaCloudTextMessageRequest"/>.
/// </summary>
public sealed class MetaCloudTextMessageRequestTests
{
    /// <summary>
    /// Verifies that all properties
    /// can be initialized.
    /// </summary>
    [Fact]
    public void Properties_Should_BeInitializable()
    {
        // Arrange
        MetaCloudTextMessage text = new()
        {
            Body = "Hello World"
        };

        // Act
        MetaCloudTextMessageRequest request = new()
        {
            To = "+628123456789",
            Text = text
        };

        // Assert
        request.To.Should().Be("+628123456789");
        request.Text.Should().BeSameAs(text);

        request.MessagingProduct.Should().Be("whatsapp");
        request.RecipientType.Should().Be("individual");
        request.Type.Should().Be("text");
    }

    /// <summary>
    /// Verifies that default property values
    /// are initialized.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeDefaultValues()
    {
        // Arrange

        // Act
        MetaCloudTextMessageRequest request = new()
        {
            To = "+628123456789",
            Text = new MetaCloudTextMessage
            {
                Body = "Hello"
            }
        };

        // Assert
        request.MessagingProduct.Should().Be("whatsapp");
        request.RecipientType.Should().Be("individual");
        request.Type.Should().Be("text");
    }
}