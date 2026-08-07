using Platform.Communication.Models;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Defines the contract for communicating
/// with the Twilio SMS API.
/// </summary>
internal interface ITwilioSmsClient
{
    /// <summary>
    /// Sends an SMS message through Twilio.
    /// </summary>
    /// <param name="recipient">
    /// The destination phone number
    /// in E.164 format.
    /// </param>
    /// <param name="message">
    /// The SMS message body.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="VendorDeliveryResult"/>
    /// returned by Twilio.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when
    /// <paramref name="recipient"/>
    /// or
    /// <paramref name="message"/>
    /// is empty or whitespace.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled.
    /// </exception>
    Task<VendorDeliveryResult> SendMessageAsync(
        string recipient,
        string message,
        CancellationToken cancellationToken = default);
}