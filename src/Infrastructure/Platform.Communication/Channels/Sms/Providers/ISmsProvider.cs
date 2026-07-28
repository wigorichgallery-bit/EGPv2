using Platform.Communication.Models;

namespace Platform.Communication.Channels.Sms.Providers;

/// <summary>
/// Defines the contract for sending SMS messages through an SMS provider.
/// </summary>
internal interface ISmsProvider
{
    /// <summary>
    /// Sends the specified SMS message asynchronously.
    /// </summary>
    /// <param name="message">
    /// The SMS message to send.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the delivery result.
    /// </returns>
    Task<DeliveryResult> SendAsync(
        SmsMessage message,
        CancellationToken cancellationToken = default);
}