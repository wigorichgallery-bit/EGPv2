using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.WhatsApp.Clients;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.WhatsApp.Providers;

/// <summary>
/// Represents a Twilio-based WhatsApp provider.
/// </summary>
internal sealed class TwilioWhatsAppProvider
    : IWhatsAppProvider
{
    private readonly ITwilioWhatsAppClient _client;

    private readonly ILogger<TwilioWhatsAppProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioWhatsAppProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The Twilio WhatsApp client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public TwilioWhatsAppProvider(
        ITwilioWhatsAppClient client,
        ILogger<TwilioWhatsAppProvider> logger)
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

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Sending WhatsApp message via Twilio to {RecipientCount} recipient(s).",
            message.To.Count);

        try
        {
            string? lastMessageId = null;

            foreach (var recipient in message.To)
            {
                cancellationToken.ThrowIfCancellationRequested();

                VendorDeliveryResult result =
                    await _client
                        .SendMessageAsync(
                            recipient.Value,
                            message.Message,
                            cancellationToken)
                        .ConfigureAwait(false);

                lastMessageId = result.MessageId;

                _logger.LogInformation(
                    "WhatsApp message successfully sent via Twilio. Recipient: {Recipient}, MessageId: {MessageId}",
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

            return DeliveryResult.Success(
                lastMessageId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Twilio WhatsApp send operation was cancelled.");

            throw;
        }
        catch (CommunicationException exception)
        {
            _logger.LogError(
                exception,
                "Failed to send WhatsApp message using Twilio.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}