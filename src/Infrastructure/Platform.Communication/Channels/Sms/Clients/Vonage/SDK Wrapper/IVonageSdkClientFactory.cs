namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Creates instances of
/// <see cref="IVonageSdkClient"/>.
/// </summary>
internal interface IVonageSdkClientFactory
{
    /// <summary>
    /// Creates a new Vonage SDK client.
    /// </summary>
    /// <param name="apiKey">
    /// The Vonage API key.
    /// </param>
    /// <param name="apiSecret">
    /// The Vonage API secret.
    /// </param>
    /// <returns>
    /// A configured
    /// <see cref="IVonageSdkClient"/>.
    /// </returns>
    IVonageSdkClient Create(
        string apiKey,
        string apiSecret);
}