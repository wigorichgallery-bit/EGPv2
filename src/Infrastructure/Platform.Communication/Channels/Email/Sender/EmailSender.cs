using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Sender;

/// <summary>
/// Default implementation of <see cref="IEmailSender"/>.
/// </summary>
internal sealed class EmailSender : IEmailSender
{
    private readonly IEmailProvider _provider;

    public EmailSender(
        IEmailProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _provider = provider;
    }

    public Task<DeliveryResult> SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
        => _provider.SendAsync(
            message,
            cancellationToken);
}