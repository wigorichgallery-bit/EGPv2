namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Creates instances of
/// <see cref="IVonageSdkClient"/>.
/// </summary>
internal sealed class VonageSdkClientFactory
    : IVonageSdkClientFactory
{
    /// <inheritdoc />
    public IVonageSdkClient Create(
        string apiKey,
        string apiSecret)
    {
        return new VonageSdkClient(
            apiKey,
            apiSecret);
    }
}