using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Vonage.Messages.Sms;
using Vonage.Messaging;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides communication with the Vonage SMS API.
/// </summary>
internal sealed class VonageSmsClient : IVonageSmsClient
{
    private readonly ILogger<VonageSmsClient> _logger;

    private readonly VonageSmsConfiguration _configuration;

    private readonly IVonageSdkClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="VonageSmsClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The Vonage SDK client factory.
    /// </param>
    /// <param name="options">
    /// The communication options.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public VonageSmsClient(
        IVonageSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<VonageSmsClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value
                .Sms
                .Vonage;

        ValidateConfiguration(
            _configuration);

        _client =
            factory.Create(
                _configuration.ApiKey,
                _configuration.ApiSecret);

                
    }

    /// <inheritdoc />
    public async Task<VendorDeliveryResult> SendMessageAsync(
        string recipient,
        string message,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            recipient);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            message);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Sending SMS through Vonage to {Recipient}.",
            recipient);

        try
        {
            SendSmsResponse response =
                await _client
                .SendMessageAsync(
                    _configuration.From,
                    recipient,
                    message,
                    cancellationToken)
                .ConfigureAwait(false);

            ValidateResponse(
                response);

            VendorDeliveryResult result =
                CreateVendorDeliveryResult(
                    response);

            _logger.LogDebug(
                "Vonage accepted SMS. MessageId: {MessageId}",
                result.MessageId);

            return result;
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
                "Vonage returned an error while sending SMS to {Recipient}.",
                recipient);

            throw new CommunicationException(
                "Failed to send SMS using Vonage.",
                exception);
        }
    }

    /// <summary>
    /// Validates the Vonage configuration.
    /// </summary>
    private static void ValidateConfiguration(
        VonageSmsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.ApiKey))
        {
            throw new InvalidOperationException(
                "Vonage ApiKey is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.ApiSecret))
        {
            throw new InvalidOperationException(
                "Vonage ApiSecret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.From))
        {
            throw new InvalidOperationException(
                "Vonage From is not configured.");
        }
    }

    /// <summary>
    /// Validates the Vonage response.
    /// </summary>
    private static void ValidateResponse(
      SendSmsResponse response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        SmsResponseMessage? message =
            response.Messages?
                .FirstOrDefault();

        if (message is null)
        {
            throw new InvalidOperationException(
                "Vonage returned an empty SMS response.");
        }
    }
    /// <summary>
    /// Creates a vendor delivery result.
    /// </summary>
    private static VendorDeliveryResult CreateVendorDeliveryResult(
        SendSmsResponse response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        SmsResponseMessage message =
            response.Messages?
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                "Vonage returned an empty SMS response.");

        return VendorDeliveryResult.Success(
            messageId: message.MessageId,
            providerReference: message.MessageId,
            status: message.Status.ToString(),
            rawResponse: response);
    }

}