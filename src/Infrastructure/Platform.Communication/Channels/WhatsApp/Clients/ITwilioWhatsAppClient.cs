using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Defines the contract for communicating with the Twilio WhatsApp API.
/// </summary>
internal interface ITwilioWhatsAppClient
{
    /// <summary>
    /// Sends a WhatsApp message through the Twilio API.
    /// </summary>
    /// <param name="recipient">
    /// The recipient WhatsApp number in E.164 format.
    /// </param>
    /// <param name="message">
    /// The message text.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="VendorDeliveryResult"/> containing the
    /// vendor delivery information.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more required arguments are empty
    /// or whitespace.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled.
    /// </exception>
    Task<VendorDeliveryResult> SendMessageAsync(
        string recipient,
        string message,
        CancellationToken cancellationToken = default);
}