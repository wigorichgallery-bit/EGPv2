using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Sms.Configuration;
using Platform.Communication.Models;
using Platform.Communication.Options;

using Twilio;
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

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TwilioSmsClient"/> class.
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
    /// Thrown when the Twilio configuration is invalid.
    /// </exception>
    public TwilioSmsClient(
        IOptions<CommunicationOptions> options,
        ILogger<TwilioSmsClient> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value.Sms.Twilio;

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
            "Sending SMS through Twilio to {Recipient}.",
            recipient);

        try
        {
            var response =
                await MessageResource.CreateAsync(
                        to: CreatePhoneNumber(recipient),
                        from: CreatePhoneNumber(
                            _configuration.FromNumber),
                        body: message)
                    .ConfigureAwait(false);

            _logger.LogDebug(
                "Twilio returned MessageSid {MessageSid} for recipient {Recipient}.",
                response.Sid,
                recipient);

            return VendorDeliveryResult.Success(
                messageId: response.Sid,
                providerReference: response.Sid,
                status: response.Status.ToString());
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
                "Twilio returned an error while sending an SMS to {Recipient}.",
                recipient);

            throw;
        }
    }

    /// <summary>
    /// Validates the Twilio configuration.
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
        TwilioSmsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.AccountSid))
        {
            throw new InvalidOperationException(
                "Twilio AccountSid is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.AuthToken))
        {
            throw new InvalidOperationException(
                "Twilio AuthToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.FromNumber))
        {
            throw new InvalidOperationException(
                "Twilio FromNumber is not configured.");
        }
    }

    /// <summary>
    /// Creates a Twilio phone number instance.
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

        return new PhoneNumber(phoneNumber);
    }
}