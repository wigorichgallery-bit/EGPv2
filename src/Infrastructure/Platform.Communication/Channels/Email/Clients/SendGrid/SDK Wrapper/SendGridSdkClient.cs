using SendGrid;
using SendGrid.Helpers.Mail;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Wraps the SendGrid SDK client.
/// </summary>
internal sealed class SendGridSdkClient
    : ISendGridSdkClient
{
    private readonly SendGrid.SendGridClient _client;

    public SendGridSdkClient(
        string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _client =
            new SendGrid.SendGridClient(
                apiKey);
    }

    public Task<Response> SendEmailAsync(
        SendGridMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        return _client.SendEmailAsync(
            message,
            cancellationToken);
    }
}