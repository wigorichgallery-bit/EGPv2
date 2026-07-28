using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Models;
namespace Platform.Communication.Channels.Sms.Sender;

/// <summary>
/// Default implementation of <see cref="ISmsSender"/>.
/// </summary>
internal sealed class SmsSender : ISmsSender
{
    private readonly ISmsProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmsSender"/> class.
    /// </summary>
    /// <param name="provider">The SMS provider.</param>
    public SmsSender(ISmsProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _provider = provider;
    }

    /// <inheritdoc />
    public Task<DeliveryResult> SendAsync(
        SmsMessage message,
        CancellationToken cancellationToken = default)
        => _provider.SendAsync(message, cancellationToken);
}