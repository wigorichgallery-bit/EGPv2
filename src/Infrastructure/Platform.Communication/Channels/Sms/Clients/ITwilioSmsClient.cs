using Platform.Communication.Models;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Defines the contract for communicating with the Twilio SMS API.
/// </summary>
internal interface ITwilioSmsClient
{
    /// <summary>
    /// Sends an SMS message through the Twilio API.
    /// </summary>
    /// <param name="recipient">
    /// The recipient phone number in E.164 format.
    /// </param>
    /// <param name="message">
    /// The message text to send.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="VendorDeliveryResult"/> containing the
    /// vendor delivery information returned by Twilio.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="recipient"/> or
    /// <paramref name="message"/> is empty or whitespace.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled.
    /// </exception>
    Task<VendorDeliveryResult> SendMessageAsync(
        string recipient,
        string message,
        CancellationToken cancellationToken = default);
}