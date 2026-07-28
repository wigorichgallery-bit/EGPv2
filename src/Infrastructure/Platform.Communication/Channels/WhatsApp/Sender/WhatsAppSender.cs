using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Sender;

/// <summary>
/// Default implementation of <see cref="IWhatsAppSender"/>.
/// </summary>
internal sealed class WhatsAppSender : IWhatsAppSender
{
    private readonly IWhatsAppProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhatsAppSender"/> class.
    /// </summary>
    /// <param name="provider">The WhatsApp provider.</param>
    public WhatsAppSender(IWhatsAppProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public Task<DeliveryResult> SendAsync(
        WhatsAppMessage message,
        CancellationToken cancellationToken = default)
        => _provider.SendAsync(message, cancellationToken);
}