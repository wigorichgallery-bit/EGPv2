using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides communication with an SMTP server.
/// </summary>
internal sealed class MailKitSmtpClient
    : IMailKitSmtpClient
{
    private readonly ILogger<MailKitSmtpClient> _logger;

    private readonly SmtpConfiguration _configuration;

    private readonly IMailKitSmtpSdkClientFactory _factory;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MailKitSmtpClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The MailKit SMTP SDK client factory.
    /// </param>
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
    public MailKitSmtpClient(
        IMailKitSmtpSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<MailKitSmtpClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _factory = factory;
        _logger = logger;

        _configuration =
            options.Value
                .Email
                .Configuration
                .Smtp;

        ValidateConfiguration(
            _configuration);
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
            MimeMessage email =
                CreateMimeMessage(message);

            await using IMailKitSmtpSdkClient client =
                _factory.Create();

            await client
                .ConnectAsync(
                    _configuration.Host,
                    _configuration.Port,
                    GetSecureSocketOptions(),
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

            string messageId =
                await client
                    .SendAsync(
                        email,
                        cancellationToken)
                    .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new InvalidOperationException(
                    "The SMTP server did not return a message identifier.");
            }

            await client
                .DisconnectAsync(
                    quit: true,
                    cancellationToken)
                .ConfigureAwait(false);

            _logger.LogDebug(
                "SMTP accepted email. MessageId: {MessageId}",
                messageId);

            return VendorDeliveryResult.Success(
                messageId: messageId,
                providerReference: messageId,
                status: "Accepted");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "SMTP email request was cancelled.");

            throw;
        }
        catch (CommunicationException)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "SMTP returned an error while sending email.");

            throw new CommunicationException(
                "Failed to send email using SMTP.",
                exception);
        }
    }

    /// <summary>
    /// Creates the MIME email message.
    /// </summary>
    /// <param name="message">
    /// The communication email message.
    /// </param>
    /// <returns>
    /// A configured MIME email message.
    /// </returns>
    private MimeMessage CreateMimeMessage(
        EmailMessage message)
    {
        MimeMessage email =
            new();

        email.From.Add(
            new MailboxAddress(
                _configuration.SenderName,
                _configuration.SenderAddress));

        foreach (var recipient in message.To)
        {
            email.To.Add(
                MailboxAddress.Parse(
                    recipient.Value));
        }

        if (message.Cc is not null)
        {
            foreach (var recipient in message.Cc)
            {
                email.Cc.Add(
                    MailboxAddress.Parse(
                        recipient.Value));
            }
        }

        if (message.Bcc is not null)
        {
            foreach (var recipient in message.Bcc)
            {
                email.Bcc.Add(
                    MailboxAddress.Parse(
                        recipient.Value));
            }
        }

        email.Subject =
            message.Subject;

        BodyBuilder bodyBuilder =
            new();

        if (message.IsHtml)
        {
            bodyBuilder.HtmlBody =
                message.Body;
        }
        else
        {
            bodyBuilder.TextBody =
                message.Body;
        }

        if (message.Attachments is not null &&
            message.Attachments.Count > 0)
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

        email.Body =
            bodyBuilder.ToMessageBody();

        return email;
    }

    /// <summary>
    /// Gets the configured MailKit secure socket option.
    /// </summary>
    /// <returns>
    /// The secure socket option used by MailKit.
    /// </returns>
    private SecureSocketOptions GetSecureSocketOptions()
    {
        return _configuration.EnableSsl
            ? SecureSocketOptions.SslOnConnect
            : SecureSocketOptions.StartTlsWhenAvailable;
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
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.Host))
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