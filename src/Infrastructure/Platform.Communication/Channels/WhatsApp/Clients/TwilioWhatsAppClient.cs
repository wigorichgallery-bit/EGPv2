using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides communication with the Twilio WhatsApp API.
/// </summary>
internal sealed class TwilioWhatsAppClient : ITwilioWhatsAppClient
{
    /// <summary>
    /// Represents the Twilio WhatsApp URI prefix.
    /// </summary>
    private const string WhatsAppPrefix = "whatsapp:";

    private readonly ILogger<TwilioWhatsAppClient> _logger;

    private readonly TwilioWhatsAppConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioWhatsAppClient"/> class.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="options">
    /// The communication options.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Twilio WhatsApp configuration is invalid.
    /// </exception>
    public TwilioWhatsAppClient(
        ILogger<TwilioWhatsAppClient> logger,
        IOptions<CommunicationOptions> options)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);

        _logger = logger;

        _configuration =
            options.Value.WhatsApp.Twilio;

        ValidateConfiguration(_configuration);

        TwilioClient.Init(
            _configuration.AccountSid,
            _configuration.AuthToken);
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
            "Sending WhatsApp message through Twilio to {Recipient}.",
            recipient);

        var response = await MessageResource.CreateAsync(
                from: CreatePhoneNumber(
                    _configuration.FromNumber),
                to: CreatePhoneNumber(
                    recipient),
                body: message)
            .ConfigureAwait(false);

        _logger.LogDebug(
            "Twilio API returned MessageSid {MessageSid}.",
            response.Sid);

        if (response is null)
        {
            throw new InvalidOperationException(
                "Twilio API returned an empty response.");
        }

        _logger.LogDebug(
            "Twilio API returned MessageSid {MessageSid}.",
            response.Sid);

        return VendorDeliveryResult.Success(
            messageId: response.Sid,
            providerReference: response.Sid,
            status: response.Status?.ToString());
    }

    /// <summary>
    /// Validates the Twilio WhatsApp configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to validate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="configuration"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when one or more required configuration
    /// values are missing.
    /// </exception>
    private static void ValidateConfiguration(
        TwilioWhatsAppConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.AccountSid))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp AccountSid is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.AuthToken))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp AuthToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.FromNumber))
        {
            throw new InvalidOperationException(
                "Twilio WhatsApp FromNumber is not configured.");
        }
    }

    /// <summary>
    /// Creates a Twilio WhatsApp phone number.
    /// </summary>
    /// <param name="phoneNumber">
    /// The phone number in E.164 format.
    /// </param>
    /// <returns>
    /// A <see cref="PhoneNumber"/> instance.
    /// </returns>
    private static PhoneNumber CreatePhoneNumber(
        string phoneNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

        return new PhoneNumber(
            $"{WhatsAppPrefix}{phoneNumber}");
    }
}