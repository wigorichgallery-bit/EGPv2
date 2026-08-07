using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Providers;

/// <summary>
/// Represents a Microsoft Graph email provider.
/// </summary>
internal sealed class MicrosoftGraphEmailProvider : IEmailProvider
{
    private readonly IGraphClient _client;

    private readonly ILogger<MicrosoftGraphEmailProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MicrosoftGraphEmailProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The Microsoft Graph client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public MicrosoftGraphEmailProvider(
        IGraphClient client,
        ILogger<MicrosoftGraphEmailProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(logger);

        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<DeliveryResult> SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.To.Count == 0)
        {
            return DeliveryResult.Failure(
                "No recipient was specified.");
        }

        _logger.LogInformation(
            "Sending email via Microsoft Graph to {RecipientCount} recipient(s).",
            message.To.Count);

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var result = await _client
                .SendEmailAsync(
                    message,
                    cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(result.MessageId))
            {
                return DeliveryResult.Failure(
                    "The provider did not return a message identifier.");
            }

            _logger.LogInformation(
                "Email successfully sent via Microsoft Graph. MessageId: {MessageId}",
                result.MessageId);

            return DeliveryResult.Success(
                result.MessageId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Microsoft Graph email send operation was cancelled.");

            throw;
        }
        catch (CommunicationException exception)
        {
            _logger.LogError(
                exception,
                "Failed to send email using Microsoft Graph.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}