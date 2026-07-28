using System.Threading;
using System.Threading.Tasks;

using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Sender;

/// <summary>
/// Defines an email sender.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="message">
    /// Email message.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Delivery result.
    /// </returns>
    Task<DeliveryResult> SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}