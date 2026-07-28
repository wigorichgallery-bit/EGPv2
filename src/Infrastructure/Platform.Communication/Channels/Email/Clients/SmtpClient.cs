using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Models;
using Platform.Communication.Options;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides communication with an SMTP server.
/// </summary>
internal sealed class SmtpClient : ISmtpClient
{
    private readonly ILogger<SmtpClient> _logger;

    private readonly SmtpConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SmtpClient"/> class.
    /// </summary>
    /// <param name="options">
    /// The communication options.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the SMTP configuration is invalid.
    /// </exception>
    public SmtpClient(
        IOptions<CommunicationOptions> options,
        ILogger<SmtpClient> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value
                .Email
                .Configuration
                .Smtp;

        ValidateConfiguration(_configuration);
    }

    /// <inheritdoc />
    public async Task<VendorDeliveryResult> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Sending email through SMTP to {RecipientCount} recipient(s).",
            message.To.Count);

        try
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _configuration.SenderName,
                    _configuration.SenderAddress));

            foreach (var recipient in message.To)
            {
                email.To.Add(
                    MailboxAddress.Parse(recipient.Value));
            }

            if (message.Cc is not null)
            {
                foreach (var recipient in message.Cc)
                {
                    email.Cc.Add(
                        MailboxAddress.Parse(recipient.Value));
                }
            }

            if (message.Bcc is not null)
            {
                foreach (var recipient in message.Bcc)
                {
                    email.Bcc.Add(
                        MailboxAddress.Parse(recipient.Value));
                }
            }

            email.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();

            if (message.IsHtml)
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }

            if (message.Attachments is not null)
            {
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add(
                        attachment.FileName,
                        attachment.Content,
                        ContentType.Parse(
                            attachment.ContentType));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client
                .ConnectAsync(
                    _configuration.Host,
                    _configuration.Port,
                    _configuration.EnableSsl
                        ? SecureSocketOptions.SslOnConnect
                        : SecureSocketOptions.StartTlsWhenAvailable,
                    cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(
                _configuration.Username))
            {
                await client
                    .AuthenticateAsync(
                        _configuration.Username,
                        _configuration.Password,
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            var messageId = await client
                .SendAsync(
                    email,
                    cancellationToken)
                .ConfigureAwait(false);

            await client
                .DisconnectAsync(
                    true,
                    cancellationToken)
                .ConfigureAwait(false);

            _logger.LogDebug(
                "SMTP accepted email. MessageId: {MessageId}",
                messageId);

            return VendorDeliveryResult.Success(
                messageId: messageId ?? string.Empty,
                providerReference: messageId ?? string.Empty,
                status: "Accepted");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "SMTP email request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "SMTP returned an error while sending email.");

            throw;
        }
    }

    /// <summary>
    /// Validates the SMTP configuration.
    /// </summary>
    /// <param name="configuration">
    /// The SMTP configuration.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configuration"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when one or more required configuration values
    /// are missing.
    /// </exception>
    private static void ValidateConfiguration(
        SmtpConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.Host))
        {
            throw new InvalidOperationException(
                "SMTP Host is not configured.");
        }

        if (configuration.Port <= 0)
        {
            throw new InvalidOperationException(
                "SMTP Port is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.SenderAddress))
        {
            throw new InvalidOperationException(
                "SMTP SenderAddress is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.SenderName))
        {
            throw new InvalidOperationException(
                "SMTP SenderName is not configured.");
        }
    }
}