using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Defines the contract for communicating with the Meta Cloud
/// WhatsApp Business API.
/// </summary>
internal interface IMetaCloudWhatsAppClient
{
    /// <summary>
    /// Sends a WhatsApp text message through the Meta Cloud API.
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
    /// vendor delivery information returned by the Meta Cloud API.
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