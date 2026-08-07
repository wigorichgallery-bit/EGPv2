namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates SendGrid SDK clients.
/// </summary>
internal interface ISendGridSdkClientFactory
{
    /// <summary>
    /// Creates a SendGrid SDK client.
    /// </summary>
    ISendGridSdkClient Create(
        string apiKey);
}