using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Providers;

/// <summary>
/// Represents an internal WhatsApp provider implementation.
/// </summary>
internal interface IWhatsAppProvider
{
    /// <summary>
    /// Sends a WhatsApp message using the configured provider.
    /// </summary>
    /// <param name="message">The WhatsApp message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The delivery result.</returns>
    Task<DeliveryResult> SendAsync(
        WhatsAppMessage message,
        CancellationToken cancellationToken = default);
}