namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Creates instances of
/// <see cref="ITwilioWhatsAppSdkClient"/>.
/// </summary>
internal sealed class TwilioWhatsAppSdkClientFactory
    : ITwilioWhatsAppSdkClientFactory
{
    /// <inheritdoc />
    public ITwilioWhatsAppSdkClient Create(
        string accountSid,
        string authToken)
    {
        return new TwilioWhatsAppSdkClient(
            accountSid,
            authToken);
    }
}