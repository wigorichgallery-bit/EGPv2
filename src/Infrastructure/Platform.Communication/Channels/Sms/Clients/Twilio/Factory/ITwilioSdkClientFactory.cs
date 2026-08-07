namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Creates instances of
/// <see cref="ITwilioSdkClient"/>.
/// </summary>
internal interface ITwilioSdkClientFactory
{
    /// <summary>
    /// Creates a new Twilio SDK client.
    /// </summary>
    /// <param name="accountSid">
    /// The Twilio account SID.
    /// </param>
    /// <param name="authToken">
    /// The Twilio authentication token.
    /// </param>
    /// <returns>
    /// A configured
    /// <see cref="ITwilioSdkClient"/>.
    /// </returns>
    ITwilioSdkClient Create(
        string accountSid,
        string authToken);
}