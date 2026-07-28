using System.Text.Json.Serialization;

namespace Platform.Communication.Channels.WhatsApp.Models;

/// <summary>
/// Represents a text message request sent to the Meta Cloud API.
/// </summary>
internal sealed record MetaCloudTextMessageRequest
{
    /// <summary>
    /// Gets the messaging product.
    /// </summary>
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; init; } = "whatsapp";

    /// <summary>
    /// Gets the recipient type.
    /// </summary>
    [JsonPropertyName("recipient_type")]
    public string RecipientType { get; init; } = "individual";

    /// <summary>
    /// Gets the recipient phone number.
    /// </summary>
    [JsonPropertyName("to")]
    public required string To { get; init; }

    /// <summary>
    /// Gets the message type.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "text";

    /// <summary>
    /// Gets the text payload.
    /// </summary>
    [JsonPropertyName("text")]
    public required MetaCloudTextMessage Text { get; init; }
}