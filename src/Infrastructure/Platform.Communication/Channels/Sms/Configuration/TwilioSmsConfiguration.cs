namespace Platform.Communication.Channels.Sms.Configuration;

/// <summary>
/// Represents the Twilio SMS configuration.
/// </summary>
public sealed class TwilioSmsConfiguration
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
    /// Gets or sets the sender phone number.
    /// </summary>
    public string FromNumber { get; set; } = string.Empty;
}