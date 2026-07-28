using System.Text.Json.Serialization;

namespace Platform.Communication.Channels.WhatsApp.Models;

/// <summary>
/// Represents a WhatsApp message returned by the Meta Cloud API.
/// </summary>
internal sealed record MetaCloudMessage
{
    /// <summary>
    /// Gets the Meta Cloud message identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }
}