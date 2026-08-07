using Platform.Communication.Channels.WhatsApp.Models;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Models;

/// <summary>
/// Contains unit tests for <see cref="MetaCloudSendMessageResponse"/>.
/// </summary>
public sealed class MetaCloudSendMessageResponseTests
{
    /// <summary>
    /// Verifies that the messages
    /// collection can be initialized.
    /// </summary>
    [Fact]
    public void Messages_Should_BeInitializable()
    {
        // Arrange
        IReadOnlyCollection<MetaCloudMessage> messages =
        [
            new MetaCloudMessage
            {
                Id = "wamid.001"
            }
        ];

        // Act
        MetaCloudSendMessageResponse response = new()
        {
            Messages = messages
        };

        // Assert
        response.Messages.Should().BeSameAs(messages);
    }

    /// <summary>
    /// Verifies that the messages
    /// property defaults to null.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeDefaultValues()
    {
        // Arrange

        // Act
        MetaCloudSendMessageResponse response = new();

        // Assert
        response.Messages.Should().BeNull();
    }
}