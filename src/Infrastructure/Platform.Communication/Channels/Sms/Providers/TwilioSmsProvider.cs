using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.Sms.Clients;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.Sms.Providers;

/// <summary>
/// Represents a Twilio-based SMS provider.
/// </summary>
internal sealed class TwilioSmsProvider : ISmsProvider
{
    private readonly ITwilioSmsClient _client;

    private readonly ILogger<TwilioSmsProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioSmsProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The Twilio SMS client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public TwilioSmsProvider(
        ITwilioSmsClient client,
        ILogger<TwilioSmsProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(logger);

        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<DeliveryResult> SendAsync(
        SmsMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.To.Count == 0)
        {
            return DeliveryResult.Failure(
                "No recipient was specified.");
        }

        _logger.LogInformation(
            "Sending SMS message via Twilio to {RecipientCount} recipient(s).",
            message.To.Count);

        cancellationToken.ThrowIfCancellationRequested();

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

                // _logger.LogInformation(
                //     "SMS message sent successfully via Twilio. Recipient: {Recipient}, MessageId: {MessageId}",
                //     recipient.Value,
                //     result.MessageId);
            }
            if (string.IsNullOrWhiteSpace(lastMessageId))
            {
                return DeliveryResult.Failure(
                    "The provider did not return a message identifier.");
            }

            _logger.LogInformation(
                "Successfully delivered SMS message to {RecipientCount} recipient(s).",
                message.To.Count);

            return DeliveryResult.Success(lastMessageId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Twilio SMS send operation was cancelled.");

            throw;
        }
        catch (CommunicationException exception)
        {
            _logger.LogError(
                exception,
                "Failed to send SMS message using Twilio.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}