using Platform.Communication.Channels.WhatsApp.Models;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Models;

/// <summary>
/// Contains unit tests for <see cref="MetaCloudTextMessage"/>.
/// </summary>
public sealed class MetaCloudTextMessageTests
{
    /// <summary>
    /// Verifies that all properties
    /// can be initialized.
    /// </summary>
    [Fact]
    public void Properties_Should_BeInitializable()
    {
        // Arrange

        // Act
        MetaCloudTextMessage message = new()
        {
            Body = "Hello World",
            PreviewUrl = true
        };

        // Assert
        message.Body.Should().Be("Hello World");
        message.PreviewUrl.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the preview flag
    /// defaults to false.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeDefaultValues()
    {
        // Arrange

        // Act
        MetaCloudTextMessage message = new()
        {
            Body = "Hello"
        };

        // Assert
        message.PreviewUrl.Should().BeFalse();
    }
}