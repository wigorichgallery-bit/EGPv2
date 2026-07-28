using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Defines the contract for communicating with the SendGrid Email API.
/// </summary>
internal interface ISendGridClient
{
    /// <summary>
    /// Sends an email through the SendGrid API.
    /// </summary>
    /// <param name="message">
    /// The email message to send.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="VendorDeliveryResult"/> containing the
    /// vendor delivery information returned by SendGrid.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="message"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled.
    /// </exception>
    Task<VendorDeliveryResult> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}