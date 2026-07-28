using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.WhatsApp.Clients;
using Platform.Communication.Channels.WhatsApp.Models;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Providers;

/// <summary>
/// Represents a Meta Cloud based WhatsApp provider.
/// </summary>
internal sealed class MetaCloudWhatsAppProvider : IWhatsAppProvider
{
    private readonly IMetaCloudClient _client;

    private readonly ILogger<MetaCloudWhatsAppProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MetaCloudWhatsAppProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The Meta Cloud client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public MetaCloudWhatsAppProvider(
        IMetaCloudClient client,
        ILogger<MetaCloudWhatsAppProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(logger);

        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<DeliveryResult> SendAsync(
        WhatsAppMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.To.Count == 0)
        {
            return DeliveryResult.Failure(
                "No recipient was specified.");
        }

        _logger.LogInformation(
            "Sending WhatsApp message via Meta Cloud to {RecipientCount} recipient(s).",
            message.To.Count);

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            string? lastMessageId = null;

            foreach (var recipient in message.To)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _client.SendMessageAsync(
                        recipient.Value,
                        message.Message,
                        cancellationToken)
                    .ConfigureAwait(false);

                lastMessageId = result.MessageId;

                _logger.LogInformation(
                    "WhatsApp message sent successfully via Meta Cloud. Recipient: {Recipient}, MessageId: {MessageId}",
                    recipient.Value,
                    result.MessageId);
            }

            if (string.IsNullOrWhiteSpace(lastMessageId))
            {
                return DeliveryResult.Failure(
                    "The provider did not return a message identifier.");
            }

            _logger.LogInformation(
                "Successfully delivered WhatsApp message to {RecipientCount} recipient(s).",
                message.To.Count);

            return DeliveryResult.Success(lastMessageId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Meta Cloud WhatsApp send operation was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to send WhatsApp message using Meta Cloud.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}