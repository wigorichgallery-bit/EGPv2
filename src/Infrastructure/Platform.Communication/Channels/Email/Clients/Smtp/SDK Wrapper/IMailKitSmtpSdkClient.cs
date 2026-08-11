using MailKit.Security;

using MimeKit;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides an abstraction over the MailKit SMTP client.
/// </summary>
internal interface IMailKitSmtpSdkClient
    : IAsyncDisposable
{
    /// <summary>
    /// Connects to the SMTP server.
    /// </summary>
    Task ConnectAsync(
        string host,
        int port,
        SecureSocketOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates against the SMTP server.
    /// </summary>
    Task AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends the specified email message.
    /// </summary>
    Task<string> SendAsync(
        MimeMessage message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the SMTP server.
    /// </summary>
    Task DisconnectAsync(
        bool quit,
        CancellationToken cancellationToken = default);
}