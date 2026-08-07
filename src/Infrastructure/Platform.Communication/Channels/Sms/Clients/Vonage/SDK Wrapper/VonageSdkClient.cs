using Vonage;
using Vonage.Messages.Sms;
using Vonage.Messaging;
using Vonage.Request;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides a wrapper around the Vonage SMS SDK.
/// </summary>
internal sealed class VonageSdkClient : IVonageSdkClient
{
    private readonly VonageClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VonageSdkClient"/> class.
    /// </summary>
    /// <param name="apiKey">
    /// The Vonage API key.
    /// </param>
    /// <param name="apiSecret">
    /// The Vonage API secret.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more arguments are invalid.
    /// </exception>
    public VonageSdkClient(
        string apiKey,
        string apiSecret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            apiKey);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            apiSecret);

        Credentials credentials =
            Credentials.FromApiKeyAndSecret(
                apiKey,
                apiSecret);

        _client =
            new VonageClient(
                credentials);
    }

    /// <inheritdoc />
    public Task<SendSmsResponse> SendMessageAsync(
        string from,
        string to,
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            from);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            to);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            text);

        cancellationToken.ThrowIfCancellationRequested();

        SendSmsRequest request =
            new()
            {
                From = from,
                To = to,
                Text = text
            };

        return _client.SmsClient.SendAnSmsAsync(
            request);
    }
}