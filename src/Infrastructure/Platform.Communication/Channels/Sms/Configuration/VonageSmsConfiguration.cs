namespace Platform.Communication.Channels.Sms.Configuration;

/// <summary>
/// Represents the Vonage SMS configuration.
/// </summary>
public sealed class VonageSmsConfiguration
{
    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API secret.
    /// </summary>
    public string ApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sender name.
    /// </summary>
    public string From { get; set; } = string.Empty;
}   