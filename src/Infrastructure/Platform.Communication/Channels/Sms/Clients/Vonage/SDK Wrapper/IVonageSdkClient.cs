using Vonage.Messages.Sms;
using Vonage.Messaging;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides an abstraction over the Vonage SMS SDK.
/// </summary>
internal interface IVonageSdkClient
{
    /// <summary>
    /// Sends an SMS message using the Vonage SDK.
    /// </summary>
    /// <param name="from">
    /// The sender identifier.
    /// </param>
    /// <param name="to">
    /// The destination phone number.
    /// </param>
    /// <param name="text">
    /// The SMS message body.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="SendSmsResponse"/> returned by the
    /// Vonage SDK.
    /// </returns>
    Task<SendSmsResponse> SendMessageAsync(
        string from,
        string to,
        string text,
        CancellationToken cancellationToken = default);
}