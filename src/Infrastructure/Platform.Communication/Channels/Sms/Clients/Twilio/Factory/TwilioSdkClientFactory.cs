namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Creates instances of
/// <see cref="ITwilioSdkClient"/>.
/// </summary>
internal sealed class TwilioSdkClientFactory
    : ITwilioSdkClientFactory
{
    /// <inheritdoc />
    public ITwilioSdkClient Create(
        string accountSid,
        string authToken)
    {
        return new TwilioSdkClient(
            accountSid,
            authToken);
    }
}