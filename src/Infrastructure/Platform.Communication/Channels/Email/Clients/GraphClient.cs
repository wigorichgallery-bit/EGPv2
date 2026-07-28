using Azure.Identity;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Platform.Communication.Channels.Email.Configuration;
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

    private readonly GraphServiceClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GraphClient"/> class.
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
    /// Thrown when the Microsoft Graph configuration
    /// is invalid.
    /// </exception>
    public GraphClient(
        IOptions<CommunicationOptions> options,
        ILogger<GraphClient> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value.Email.Configuration.MicrosoftGraph;

        ValidateConfiguration(_configuration);

        var credential =
            new ClientSecretCredential(
                _configuration.TenantId,
                _configuration.ClientId,
                _configuration.ClientSecret);

        _client = new GraphServiceClient(
            credential);
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
            var graphMessage = new Message
            {
                Subject = message.Subject,

                Body = new ItemBody
                {
                    Content = message.Body,
                    ContentType = message.IsHtml
                        ? BodyType.Html
                        : BodyType.Text
                },

                ToRecipients =
                [
                    .. message.To.Select(
                        recipient => new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = recipient.Value
                            }
                        })
                ]
            };

            if (message.Cc is not null)
            {
                graphMessage.CcRecipients =
                [
                    .. message.Cc.Select(
                        recipient => new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = recipient.Value
                            }
                        })
                ];
            }

            if (message.Bcc is not null)
            {
                graphMessage.BccRecipients =
                [
                    .. message.Bcc.Select(
                        recipient => new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = recipient.Value
                            }
                        })
                ];
            }

            if (message.Attachments is not null &&
                message.Attachments.Count > 0)
            {
                graphMessage.Attachments =
                [
                    .. message.Attachments.Select(
                        attachment => new FileAttachment
                        {
                            Name = attachment.FileName,
                            ContentType = attachment.ContentType,
                            ContentBytes = attachment.Content
                        })
                ];
            }

            var request = new SendMailPostRequestBody
            {
                Message = graphMessage,
                SaveToSentItems = true
            };

            await _client
                .Users[_configuration.UserId]
                .SendMail
                .PostAsync(
                    request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            _logger.LogDebug(
                "Email successfully sent through Microsoft Graph.");

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

            throw;
        }
    }

    /// <summary>
    /// Validates the Microsoft Graph configuration.
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
        MicrosoftGraphConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.TenantId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph TenantId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.ClientId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph ClientId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.ClientSecret))
        {
            throw new InvalidOperationException(
                "Microsoft Graph ClientSecret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.UserId))
        {
            throw new InvalidOperationException(
                "Microsoft Graph UserId is not configured.");
        }
    }
}