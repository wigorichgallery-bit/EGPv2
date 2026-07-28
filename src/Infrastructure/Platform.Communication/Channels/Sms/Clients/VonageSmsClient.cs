using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides communication with the Vonage SMS API.
/// </summary>
internal sealed class VonageSmsClient : IVonageSmsClient
{
    private readonly ILogger<VonageSmsClient> _logger;

    private readonly VonageSmsConfiguration _configuration;

    private readonly VonageClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VonageSmsClient"/> class.
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
    /// Thrown when the Vonage configuration is invalid.
    /// </exception>
    public VonageSmsClient(
        IOptions<CommunicationOptions> options,
        ILogger<VonageSmsClient> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value.Sms.Vonage;

        ValidateConfiguration(_configuration);

        var credentials = Credentials.FromApiKeyAndSecret(
            _configuration.ApiKey,
            _configuration.ApiSecret);

        _client = new VonageClient(credentials);
    }

    /// <inheritdoc />
    public async Task<VendorDeliveryResult> SendMessageAsync(
        string recipient,
        string message,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Sending SMS through Vonage to {Recipient}.",
            recipient);

        try
        {
            var request = new SendSmsRequest
            {
                To = recipient,
                From = _configuration.From,
                Text = message
            };

            var response =
                await _client.SmsClient
                    .SendAnSmsAsync(request)
                    .ConfigureAwait(false);

            var firstMessage =
                response.Messages?
                    .FirstOrDefault();

            if (firstMessage is null)
            {
                throw new InvalidOperationException(
                    "Vonage returned an empty SMS response.");
            }

            _logger.LogDebug(
                "Vonage returned MessageId {MessageId} for recipient {Recipient}.",
                firstMessage.MessageId,
                recipient);

            return VendorDeliveryResult.Success(
                messageId: firstMessage.MessageId,
                providerReference: firstMessage.MessageId,
                status: firstMessage.Status.ToString());
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Vonage SMS request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Vonage returned an error while sending an SMS to {Recipient}.",
                recipient);

            throw;
        }
    }

    /// <summary>
    /// Validates the Vonage SMS configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to validate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configuration"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when one or more required configuration values
    /// are missing.
    /// </exception>
    private static void ValidateConfiguration(
        VonageSmsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.ApiKey))
        {
            throw new InvalidOperationException(
                "Vonage ApiKey is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.ApiSecret))
        {
            throw new InvalidOperationException(
                "Vonage ApiSecret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.From))
        {
            throw new InvalidOperationException(
                "Vonage From is not configured.");
        }
    }
}