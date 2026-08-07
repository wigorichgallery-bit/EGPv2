using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides communication with the Twilio WhatsApp API.
/// </summary>
internal sealed class TwilioWhatsAppClient
    : ITwilioWhatsAppClient
{
    /// <summary>
    /// Represents the Twilio WhatsApp URI prefix.
    /// </summary>
    private const string WhatsAppPrefix =
        "whatsapp:";

    private readonly ILogger<TwilioWhatsAppClient>
        _logger;

    private readonly TwilioWhatsAppConfiguration
        _configuration;

    private readonly ITwilioWhatsAppSdkClient
        _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioWhatsAppClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The Twilio WhatsApp SDK client factory.
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
    /// Thrown when the Twilio WhatsApp configuration
    /// is invalid.
    /// </exception>
    public TwilioWhatsAppClient(
        ITwilioWhatsAppSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<TwilioWhatsAppClient> logger)
    {
        ArgumentNullException.ThrowIfNull(
            factory);

        ArgumentNullException.ThrowIfNull(
            options);

        ArgumentNullException.ThrowIfNull(
            logger);

        _logger = logger;

        _configuration =
            options.Value
                .WhatsApp
                .Twilio;

        ValidateConfiguration(
            _configuration);

        _client =
            factory.Create(
                _configuration.AccountSid,
                _configuration.AuthToken);
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
            "Sending WhatsApp message through Twilio to {Recipient}.",
            recipient);

        try
        {
            MessageResource response =
                await _client
                    .SendMessageAsync(
                        CreatePhoneNumber(
                            _configuration.FromNumber),
                        CreatePhoneNumber(
                            recipient),
                        message,
                        cancellationToken)
                    .ConfigureAwait(false);

            ValidateResponse(
                response);

            VendorDeliveryResult result =
                CreateVendorDeliveryResult(
                    response);

            _logger.LogDebug(
                "Twilio accepted WhatsApp message. MessageId: {MessageId}",
                result.MessageId);

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Twilio WhatsApp request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Twilio returned an error while sending WhatsApp message to {Recipient}.",
                recipient);

            throw new CommunicationException(
                "Failed to send WhatsApp message using Twilio.",
                exception);
        }
    }

    /// <summary>
    /// Validates the Twilio WhatsApp configuration.
    /// </summary>
    private static void ValidateConfiguration(
        TwilioWhatsAppConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.AccountSid))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp AccountSid is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.AuthToken))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp AuthToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.FromNumber))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp FromNumber is not configured.");
        }
    }

    /// <summary>
    /// Creates a Twilio WhatsApp phone number.
    /// </summary>
    private static PhoneNumber CreatePhoneNumber(
        string phoneNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            phoneNumber);

        return new PhoneNumber(
            $"{WhatsAppPrefix}{phoneNumber}");
    }

    /// <summary>
    /// Validates the Twilio SDK response.
    /// </summary>
    private static void ValidateResponse(
        MessageResource response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        if (string.IsNullOrWhiteSpace(
            response.Sid))
        {
            throw new InvalidOperationException(
                "Twilio returned an invalid WhatsApp response.");
        }
    }

    /// <summary>
    /// Creates a vendor delivery result.
    /// </summary>
    private static VendorDeliveryResult CreateVendorDeliveryResult(
        MessageResource response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        return VendorDeliveryResult.Success(
            messageId: response.Sid,
            providerReference: response.Sid,
            status: response.Status?.ToString(),
            rawResponse: response);
    }
}