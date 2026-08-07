using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides an abstraction over the Twilio SDK.
/// </summary>
internal interface ITwilioSdkClient
{
    /// <summary>
    /// Sends an SMS message using the Twilio SDK.
    /// </summary>
    /// <param name="from">
    /// The sender phone number.
    /// </param>
    /// <param name="to">
    /// The recipient phone number.
    /// </param>
    /// <param name="body">
    /// The SMS message body.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The Twilio <see cref="MessageResource"/> returned by the SDK.
    /// </returns>
    Task<MessageResource> SendMessageAsync(
        PhoneNumber from,
        PhoneNumber to,
        string body,
        CancellationToken cancellationToken = default);
}