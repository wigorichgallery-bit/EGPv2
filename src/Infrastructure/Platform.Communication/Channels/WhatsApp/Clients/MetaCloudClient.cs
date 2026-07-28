using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Channels.WhatsApp.Models;
using Platform.Communication.Models;
using Platform.Communication.Options;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides communication with the Meta Cloud WhatsApp Business API.
/// </summary>
internal sealed class MetaCloudClient : IMetaCloudClient
{
    /// <summary>
    /// Represents the Meta Graph API version.
    /// </summary>
    private const string GraphApiVersion = "v23.0";

    private readonly HttpClient _httpClient;

    private readonly ILogger<MetaCloudClient> _logger;

    private readonly MetaCloudConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MetaCloudClient"/> class.
    /// </summary>
    /// <param name="httpClient">
    /// The HTTP client used to communicate with the Meta Cloud API.
    /// </param>
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
    /// Thrown when the Meta Cloud configuration is invalid.
    /// </exception>
    public MetaCloudClient(
        HttpClient httpClient,
        IOptions<CommunicationOptions> options,
        ILogger<MetaCloudClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;

        _configuration =
            options.Value.WhatsApp.MetaCloud;

        ValidateConfiguration(_configuration);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _configuration.AccessToken);
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
            "Sending WhatsApp message through Meta Cloud API to {Recipient}.",
            recipient);

        try
        {
            using var request = CreateRequestContent(
                recipient,
                message);

            using var response = await _httpClient.PostAsync(
                    CreateRequestUri(),
                    request,
                    cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<MetaCloudSendMessageResponse>(
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (payload is null)
            {
                throw new InvalidOperationException(
                    "Meta Cloud API returned an empty response.");
            }

            var firstMessage =
                payload.Messages?
                    .FirstOrDefault();

            var messageId =
                firstMessage?.Id;

            _logger.LogDebug(
                "Meta Cloud API returned MessageId {MessageId} for recipient {Recipient}.",
                messageId,
                recipient);

            return VendorDeliveryResult.Success(
                messageId: messageId,
                providerReference: messageId,
                status: "Accepted");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Meta Cloud API request was cancelled.");

            throw;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(
                exception,
                "Meta Cloud API returned an HTTP error while sending a WhatsApp message to {Recipient}.",
                recipient);

            throw;
        }
    }

    /// <summary>
    /// Validates the Meta Cloud configuration.
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
        MetaCloudConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.AccessToken))
        {
            throw new InvalidOperationException(
                "Meta Cloud AccessToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.PhoneNumberId))
        {
            throw new InvalidOperationException(
                "Meta Cloud PhoneNumberId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(configuration.BusinessAccountId))
        {
            throw new InvalidOperationException(
                "Meta Cloud BusinessAccountId is not configured.");
        }
    }

    /// <summary>
    /// Creates the Meta Cloud API request URI.
    /// </summary>
    /// <returns>
    /// The Meta Cloud API request URI.
    /// </returns>
    private string CreateRequestUri()
    {
        return
            $"https://graph.facebook.com/{GraphApiVersion}/{_configuration.PhoneNumberId}/messages";
    }

    /// <summary>
    /// Creates the HTTP request content for the
    /// Meta Cloud API.
    /// </summary>
    /// <param name="recipient">
    /// The recipient phone number in E.164 format.
    /// </param>
    /// <param name="message">
    /// The WhatsApp message text.
    /// </param>
    /// <returns>
    /// The HTTP request content.
    /// </returns>
    private static HttpContent CreateRequestContent(
        string recipient,
        string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        var payload = new MetaCloudTextMessageRequest
        {
            To = recipient,
            Text = new MetaCloudTextMessage
            {
                Body = message,
                PreviewUrl = false
            }
        };

        return JsonContent.Create(payload);
    }
}