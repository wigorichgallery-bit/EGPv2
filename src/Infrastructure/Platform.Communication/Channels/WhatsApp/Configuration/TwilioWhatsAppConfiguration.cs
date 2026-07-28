namespace Platform.Communication.Channels.WhatsApp.Configuration;

/// <summary>
/// Represents the Twilio WhatsApp configuration.
/// </summary>
public sealed class TwilioWhatsAppConfiguration
{
    /// <summary>
    /// Gets or sets the account SID.
    /// </summary>
    public string AccountSid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    public string AuthToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the WhatsApp sender number.
    /// </summary>
    public string FromNumber { get; set; } = string.Empty;
}