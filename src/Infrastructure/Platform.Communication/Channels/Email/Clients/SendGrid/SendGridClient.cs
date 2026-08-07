using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides communication with the SendGrid Email API.
/// </summary>
internal sealed class SendGridClient : ISendGridClient
{
    private readonly ILogger<SendGridClient> _logger;

    private readonly SendGridConfiguration _configuration;

    private readonly ISendGridSdkClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SendGridClient"/> class.
    /// </summary>
    /// <param name="options">
    /// The communication options.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the SendGrid configuration
    /// is invalid.
    /// </exception>
    public SendGridClient(
        ISendGridSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<SendGridClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);
        
        _logger = logger;

        _configuration =
            options.Value
                .Email
                .Configuration
                .SendGrid;

        ValidateConfiguration(_configuration);

        _client = factory.Create(
            _configuration.ApiKey);
    }

    /// <inheritdoc />
    public async Task<VendorDeliveryResult> SendEmailAsync(
     EmailMessage message,
     CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Sending email through SendGrid to {RecipientCount} recipient(s).",
            message.To.Count);

        try
        {
            SendGridMessage mail =
                CreateSendGridMessage(
                    message);

            Response response =
                await _client
                    .SendEmailAsync(
                        mail,
                        cancellationToken)
                    .ConfigureAwait(false);

            ValidateResponse(
                response);

            string providerReference =
                GetProviderReference(
                    response);

            _logger.LogDebug(
                "SendGrid accepted email. MessageId: {MessageId}",
                providerReference);

            return VendorDeliveryResult.Success(
                messageId: providerReference,
                providerReference: providerReference,
                status: response.StatusCode.ToString(),
                rawResponse: response);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "SendGrid email request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "SendGrid returned an error while sending email.");

            throw new CommunicationException(
                "Failed to send email using SendGrid.",
                exception);
        }
    }

    /// <summary>
    /// Validates the SendGrid configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to validate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configuration"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when one or more required configuration
    /// values are missing.
    /// </exception>
    private static void ValidateConfiguration(
        SendGridConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.ApiKey))
        {
            throw new InvalidOperationException(
                "SendGrid ApiKey is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.SenderAddress))
        {
            throw new InvalidOperationException(
                "SendGrid SenderAddress is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.SenderName))
        {
            throw new InvalidOperationException(
                "SendGrid SenderName is not configured.");
        }
    }

    private SendGridMessage CreateSendGridMessage(
        EmailMessage message)
    {
        SendGridMessage mail =
            new()
            {
                From = new EmailAddress(
                    _configuration.SenderAddress,
                    _configuration.SenderName),

                Subject = message.Subject
            };

        if (message.IsHtml)
        {
            mail.HtmlContent = message.Body;
        }
        else
        {
            mail.PlainTextContent = message.Body;
        }

        AddRecipients(
            mail,
            message);

        if (message.Attachments is not null &&
            message.Attachments.Count > 0)
        {
            AddAttachments(
                mail,
                message.Attachments);
        }

        return mail;
    }

    private static void AddRecipients(
        SendGridMessage message,
        EmailMessage email)
    {
        foreach (var recipient in email.To)
        {
            message.AddTo(
                new EmailAddress(
                    recipient.Value));
        }

        if (email.Cc is not null)
        {
            foreach (var recipient in email.Cc)
            {
                message.AddCc(
                    new EmailAddress(
                        recipient.Value));
            }
        }

        if (email.Bcc is not null)
        {
            foreach (var recipient in email.Bcc)
            {
                message.AddBcc(
                    new EmailAddress(
                        recipient.Value));
            }
        }
    }

    private static void AddAttachments(
    SendGridMessage message,
    IReadOnlyCollection<EmailAttachment> attachments)
    {
        foreach (var attachment in attachments)
        {
            message.AddAttachment(
                attachment.FileName,
                Convert.ToBase64String(
                    attachment.Content),
                attachment.ContentType);
        }
    }

    private static void ValidateResponse(
    Response response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"SendGrid returned status code {response.StatusCode}.");
        }
    }

    private static string GetProviderReference(
    Response response)
    {
        if (response.Headers.TryGetValues(
            "X-Message-Id",
            out IEnumerable<string>? values))
        {
            return values.FirstOrDefault() ?? string.Empty;
        }

        return string.Empty;
    }
}