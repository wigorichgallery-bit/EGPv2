using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides a wrapper around the Twilio WhatsApp SDK.
/// </summary>
internal sealed class TwilioWhatsAppSdkClient
    : ITwilioWhatsAppSdkClient
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioWhatsAppSdkClient"/> class.
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
    public TwilioWhatsAppSdkClient(
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
        ArgumentNullException.ThrowIfNull(
            from);

        ArgumentNullException.ThrowIfNull(
            to);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            body);

        cancellationToken.ThrowIfCancellationRequested();

        return MessageResource.CreateAsync(
            from: from,
            to: to,
            body: body);
    }
}