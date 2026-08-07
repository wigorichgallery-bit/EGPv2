using Platform.Communication.Channels.WhatsApp.Models;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Models;

/// <summary>
/// Contains unit tests for <see cref="MetaCloudMessage"/>.
/// </summary>
public sealed class MetaCloudMessageTests
{
    /// <summary>
    /// Verifies that the identifier
    /// can be initialized.
    /// </summary>
    [Fact]
    public void Id_Should_BeInitializable()
    {
        // Arrange

        // Act
        MetaCloudMessage message = new()
        {
            Id = "wamid.123"
        };

        // Assert
        message.Id.Should().Be("wamid.123");
    }

    /// <summary>
    /// Verifies that the identifier
    /// defaults to null.
    /// </summary>
    [Fact]
    public void Constructor_Should_InitializeDefaultValues()
    {
        // Arrange

        // Act
        MetaCloudMessage message = new();

        // Assert
        message.Id.Should().BeNull();
    }
}