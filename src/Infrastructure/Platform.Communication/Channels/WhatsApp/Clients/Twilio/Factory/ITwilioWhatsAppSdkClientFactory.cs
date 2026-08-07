namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Creates instances of
/// <see cref="ITwilioWhatsAppSdkClient"/>.
/// </summary>
internal interface ITwilioWhatsAppSdkClientFactory
{
    /// <summary>
    /// Creates a new Twilio WhatsApp SDK client.
    /// </summary>
    /// <param name="accountSid">
    /// The Twilio account SID.
    /// </param>
    /// <param name="authToken">
    /// The Twilio authentication token.
    /// </param>
    /// <returns>
    /// A configured
    /// <see cref="ITwilioWhatsAppSdkClient"/>.
    /// </returns>
    ITwilioWhatsAppSdkClient Create(
        string accountSid,
        string authToken);
}