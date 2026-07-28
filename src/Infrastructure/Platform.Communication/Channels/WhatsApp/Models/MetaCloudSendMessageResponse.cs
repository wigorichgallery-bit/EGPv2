using System.Text.Json.Serialization;

namespace Platform.Communication.Channels.WhatsApp.Models;

/// <summary>
/// Represents the response returned by the Meta Cloud API after sending
/// a WhatsApp message.
/// </summary>
internal sealed record MetaCloudSendMessageResponse
{
    /// <summary>
    /// Gets the messages returned by the API.
    /// </summary>
    [JsonPropertyName("messages")]
    public IReadOnlyCollection<MetaCloudMessage>? Messages { get; init; }
}