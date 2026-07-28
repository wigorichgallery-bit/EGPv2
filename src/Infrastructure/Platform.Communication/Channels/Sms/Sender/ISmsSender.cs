using System.Threading;
using System.Threading.Tasks;

using Platform.Communication.Models;

namespace Platform.Communication.Channels.Sms.Sender;

/// <summary>
/// Defines an SMS sender.
/// </summary>
public interface ISmsSender
{
    /// <summary>
    /// Sends an SMS message asynchronously.
    /// </summary>
    /// <param name="message">
    /// SMS message.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Delivery result.
    /// </returns>
    Task<DeliveryResult> SendAsync(
        SmsMessage message,
        CancellationToken cancellationToken = default);
}