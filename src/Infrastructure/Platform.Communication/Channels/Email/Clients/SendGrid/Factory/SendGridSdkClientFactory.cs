namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates SendGrid SDK client instances.
/// </summary>
internal sealed class SendGridSdkClientFactory
    : ISendGridSdkClientFactory
{
    public ISendGridSdkClient Create(
        string apiKey)
    {
        return new SendGridSdkClient(
            apiKey);
    }
}