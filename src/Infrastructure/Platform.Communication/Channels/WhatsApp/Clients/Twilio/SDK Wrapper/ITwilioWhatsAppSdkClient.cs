using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides an abstraction over the Twilio WhatsApp SDK.
/// </summary>
internal interface ITwilioWhatsAppSdkClient
{
    /// <summary>
    /// Sends a WhatsApp message using the Twilio SDK.
    /// </summary>
    /// <param name="from">
    /// The sender WhatsApp phone number.
    /// </param>
    /// <param name="to">
    /// The recipient WhatsApp phone number.
    /// </param>
    /// <param name="body">
    /// The WhatsApp message body.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="MessageResource"/> returned by the
    /// Twilio SDK.
    /// </returns>
    Task<MessageResource> SendMessageAsync(
        PhoneNumber from,
        PhoneNumber to,
        string body,
        CancellationToken cancellationToken = default);
}