using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Sender;

/// <summary>
/// Defines a WhatsApp sender.
/// </summary>
public interface IWhatsAppSender
{
    /// <summary>
    /// Sends a WhatsApp message asynchronously.
    /// </summary>
    /// <param name="message">
    /// WhatsApp message.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Delivery result.
    /// </returns>
    Task<DeliveryResult> SendAsync(
        WhatsAppMessage message,
        CancellationToken cancellationToken = default);
}