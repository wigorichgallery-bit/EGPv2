using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides a wrapper around the Twilio SDK.
/// </summary>
internal sealed class TwilioSdkClient : ITwilioSdkClient
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioSdkClient"/> class.
    /// </summary>
    /// <param name="accountSid">
    /// The Twilio account SID.
    /// </param>
    /// <param name="authToken">
    /// The Twilio authentication token.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more arguments are invalid.
    /// </exception>
    public TwilioSdkClient(
        string accountSid,
        string authToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            accountSid);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            authToken);

        TwilioClient.Init(
            accountSid,
            authToken);
    }

    /// <inheritdoc />
    public Task<MessageResource> SendMessageAsync(
        PhoneNumber from,
        PhoneNumber to,
        string body,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            body);

        cancellationToken.ThrowIfCancellationRequested();

        return MessageResource.CreateAsync(
            from: from,
            to: to,
            body: body);
    }
}