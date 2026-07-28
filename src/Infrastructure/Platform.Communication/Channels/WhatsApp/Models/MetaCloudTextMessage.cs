using System.Text.Json.Serialization;

namespace Platform.Communication.Channels.WhatsApp.Models;

/// <summary>
/// Represents the text payload of a Meta Cloud WhatsApp message.
/// </summary>
internal sealed record MetaCloudTextMessage
{
    /// <summary>
    /// Gets the message body.
    /// </summary>
    [JsonPropertyName("body")]
    public required string Body { get; init; }

    /// <summary>
    /// Gets a value indicating whether URL previews are enabled.
    /// </summary>
    [JsonPropertyName("preview_url")]
    public bool PreviewUrl { get; init; }
}