    namespace Platform.Communication.Channels.WhatsApp.Configuration;
    /// <summary>
    /// Represents the WhatsApp provider configurations.
    /// </summary>
    public sealed class WhatsAppConfiguration
    {
        /// <summary>
        /// Gets or sets the Meta Cloud configuration.
        /// </summary>
        public MetaCloudWhatsAppConfiguration MetaCloud { get; set; } = new();

        /// <summary>
        /// Gets or sets the Twilio WhatsApp configuration.
        /// </summary>
        public TwilioWhatsAppConfiguration Twilio { get; set; } = new();
    }