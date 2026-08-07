using MailKit.Security;

using MimeKit;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Wraps the MailKit SMTP client.
/// </summary>
internal sealed class MailKitSmtpClient
    : IMailKitSmtpClient
{
    private readonly MailKit.Net.Smtp.SmtpClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MailKitSmtpClient"/> class.
    /// </summary>
    public MailKitSmtpClient()
    {
        _client = new MailKit.Net.Smtp.SmtpClient();
    }

    /// <inheritdoc />
    public Task ConnectAsync(
        string host,
        int port,
        SecureSocketOptions options,
        CancellationToken cancellationToken = default)
    {
        return _client.ConnectAsync(
            host,
            port,
            options,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        return _client.AuthenticateAsync(
            username,
            password,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<string> SendAsync(
        MimeMessage message,
        CancellationToken cancellationToken = default)
    {
        return _client.SendAsync(
            message,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task DisconnectAsync(
        bool quit,
        CancellationToken cancellationToken = default)
    {
        return _client.DisconnectAsync(
            quit,
            cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        _client.Dispose();

        return ValueTask.CompletedTask;
    }
}