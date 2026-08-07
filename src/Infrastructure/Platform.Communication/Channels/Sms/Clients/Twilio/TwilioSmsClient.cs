using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Platform.Communication.Channels.Sms.Clients;

/// <summary>
/// Provides communication with the Twilio SMS API.
/// </summary>
internal sealed class TwilioSmsClient : ITwilioSmsClient
{
    private readonly ILogger<TwilioSmsClient> _logger;

    private readonly TwilioSmsConfiguration _configuration;

    private readonly ITwilioSdkClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioSmsClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The Twilio SDK client factory.
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
    /// Thrown when the configuration is invalid.
    /// </exception>
    public TwilioSmsClient(
        ITwilioSdkClientFactory factory,
        IOptions<CommunicationOptions> options,
        ILogger<TwilioSmsClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value
                .Sms
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
            "Sending SMS through Twilio to {Recipient}.",
            recipient);

        try
        {
            PhoneNumber from =
                CreatePhoneNumber(
                    _configuration.FromNumber);

            PhoneNumber to =
                CreatePhoneNumber(
                    recipient);

            MessageResource response =
                await _client
                    .SendMessageAsync(
                        from,
                        to,
                        message,
                        cancellationToken)
                    .ConfigureAwait(false);

            ValidateResponse(
                response);

            VendorDeliveryResult result =
                CreateVendorDeliveryResult(
                    response);

            _logger.LogDebug(
                "Twilio accepted SMS. MessageSid: {MessageSid}",
                result.MessageId);

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Twilio SMS request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Twilio returned an error while sending SMS to {Recipient}.",
                recipient);

            throw new CommunicationException(
                "Failed to send SMS using Twilio.",
                exception);
        }
    }

    /// <summary>
    /// Validates the Twilio configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to validate.
    /// </param>
    private static void ValidateConfiguration(
        TwilioSmsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.AccountSid))
        {
            throw new InvalidOperationException(
                "Twilio AccountSid is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.AuthToken))
        {
            throw new InvalidOperationException(
                "Twilio AuthToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.FromNumber))
        {
            throw new InvalidOperationException(
                "Twilio FromNumber is not configured.");
        }
    }

    /// <summary>
    /// Creates a Twilio phone number.
    /// </summary>
    private static PhoneNumber CreatePhoneNumber(
        string phoneNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            phoneNumber);

        return new PhoneNumber(
            phoneNumber);
    }

    /// <summary>
    /// Validates the Twilio response.
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
                "Twilio did not return a message SID.");
        }
    }

    /// <summary>
    /// Creates a vendor delivery result.
    /// </summary>
    private static VendorDeliveryResult CreateVendorDeliveryResult(
        MessageResource response)
    {
        return VendorDeliveryResult.Success(
            messageId: response.Sid,
            providerReference: response.Sid,
            status: response.Status?.ToString(),
            rawResponse: response);
    }
}