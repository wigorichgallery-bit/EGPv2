namespace Platform.Communication.Channels.Sms.Configuration;

/// <summary>
/// Represents the SMS provider configurations.
/// </summary>
public sealed class SmsConfiguration
{
    /// <summary>
    /// Gets or sets the Twilio configuration.
    /// </summary>
    public TwilioSmsConfiguration Twilio { get; set; } = new();

    /// <summary>
    /// Gets or sets the Vonage configuration.
    /// </summary>
    public VonageSmsConfiguration Vonage { get; set; } = new();

}