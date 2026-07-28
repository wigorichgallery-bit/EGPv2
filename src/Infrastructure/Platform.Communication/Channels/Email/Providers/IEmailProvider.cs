using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Providers;

/// <summary>
/// Represents an internal email provider implementation.
/// </summary>
internal interface IEmailProvider
{
    /// <summary>
    /// Sends an email message using the configured provider.
    /// </summary>
    /// <param name="message">The email message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The delivery result.</returns>
    Task<DeliveryResult> SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}