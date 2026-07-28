using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Enums;

namespace Platform.Communication.Options;

/// <summary>
/// Represents SMS communication configuration.
/// </summary>
public sealed class SmsOptions
{
    /// <summary>
    /// Gets or sets the active SMS provider.
    /// </summary>
    public SmsProviderType Provider { get; set; }

    /// <summary>
    /// Gets or sets the Twilio SMS configuration.
    /// </summary>
    public TwilioSmsConfiguration Twilio { get; set; } = new();

    /// <summary>
    /// Gets or sets the Vonage SMS configuration.
    /// </summary>
    public VonageSmsConfiguration Vonage { get; set; } = new();
}