using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Enums;

namespace Platform.Communication.Options;

/// <summary>
/// Represents WhatsApp communication configuration.
/// </summary>
public sealed class WhatsAppOptions
{
    /// <summary>
    /// Gets or sets the WhatsApp provider.
    /// </summary>
    public WhatsAppProviderType Provider { get; set; }

    /// <summary>
    /// Gets or sets the Meta WhatsApp Cloud API configuration.
    /// </summary>
    public MetaCloudWhatsAppConfiguration MetaCloud { get; set; } = new();

    /// <summary>
    /// Gets or sets the Twilio WhatsApp configuration.
    /// </summary>
    public TwilioWhatsAppConfiguration Twilio { get; set; } = new();
}