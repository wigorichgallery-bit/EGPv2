using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;

using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides communication with Microsoft Graph Mail.
/// </summary>
internal sealed class GraphClient : IGraphClient
{
    private readonly ILogger<GraphClient> _logger;

    private readonly MicrosoftGraphConfiguration _configuration;

    private readonly IGraphSdkClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GraphClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The Graph SDK client factory.
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
    /// Thrown when the Microsoft Graph configuration
    /// is invalid.
    /// </exception>
    public GraphClient(
        IGraphSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<GraphClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value
                .Email
                .Configuration
                .MicrosoftGraph;

        ValidateConfiguration(
            _configuration);

        _client =
            factory.Create(
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
            "Sending email through Microsoft Graph to {RecipientCount} recipient(s).",
            message.To.Count);

        try
        {
            Message graphMessage =
                CreateGraphMessage(message);

            SendMailPostRequestBody request =
                CreateRequest(graphMessage);

            await _client
                .SendMailAsync(
                    _configuration.UserId,
                    request,
                    cancellationToken)
                .ConfigureAwait(false);

            _logger.LogDebug(
                "Email successfully sent through Microsoft Graph.");

            // Microsoft Graph does not return the created message identifier.
            // Generate an internal identifier for tracking purposes.
            return VendorDeliveryResult.Success(
                messageId: Guid.NewGuid().ToString("N"),
                providerReference: _configuration.UserId,
                status: "Accepted");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Microsoft Graph email request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Microsoft Graph returned an error while sending email.");

            throw new CommunicationException(
                "Failed to send email using Microsoft Graph.",
                exception);
        }
    }

    /// <summary>
    /// Validates the Microsoft Graph configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to validate.
    /// </param>
    private static void ValidateConfiguration(
        MicrosoftGraphConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.TenantId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph TenantId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.ClientId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph ClientId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.ClientSecret))
        {
            throw new InvalidOperationException(
                "Microsoft Graph ClientSecret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.UserId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph UserId is not configured.");
        }
    }

    private static Message CreateGraphMessage(
    EmailMessage message)
    {
        Message graphMessage =
            new()
            {
                Subject = message.Subject,

                Body = new ItemBody
                {
                    Content = message.Body,

                    ContentType =
                        message.IsHtml
                            ? BodyType.Html
                            : BodyType.Text
                },

                ToRecipients =
                    CreateRecipients(
                        message.To)
            };

        if (message.Cc is not null)
        {
            graphMessage.CcRecipients =
                CreateRecipients(
                    message.Cc);
        }

        if (message.Bcc is not null)
        {
            graphMessage.BccRecipients =
                CreateRecipients(
                    message.Bcc);
        }

        if (message.Attachments is not null &&
            message.Attachments.Count > 0)
        {
            graphMessage.Attachments =
                CreateAttachments(
                    message.Attachments);
        }

        return graphMessage;
    }

    private static List<Recipient> CreateRecipients(
    IReadOnlyCollection<Platform.Communication.ValueObjects.EmailAddress> recipients)
    {
        return
        [
            .. recipients.Select(
            recipient =>
                new Recipient
                {
                    EmailAddress =
                        new Microsoft.Graph.Models.EmailAddress
                        {
                            Address =
                                recipient.Value
                        }
                })
        ];
    }

    private static List<Attachment> CreateAttachments(
    IReadOnlyCollection<EmailAttachment> attachments)
    {
        return
        [
            .. attachments.Select(
            attachment =>
                new FileAttachment
                {
                    Name = attachment.FileName,
                    ContentType = attachment.ContentType,
                    ContentBytes = attachment.Content
                })
        ];
    }

    private static SendMailPostRequestBody CreateRequest(
    Message message)
    {
        return new SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true
        };
    }
}