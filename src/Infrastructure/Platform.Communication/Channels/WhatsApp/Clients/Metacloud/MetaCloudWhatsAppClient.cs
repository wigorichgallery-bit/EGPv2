using System.Net.Http;
using System.Net.Http.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.WhatsApp.Configuration;
using Platform.Communication.Channels.WhatsApp.Models;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides communication with the Meta Cloud
/// WhatsApp Business API.
/// </summary>
internal sealed class MetaCloudWhatsAppClient
    : IMetaCloudWhatsAppClient
{
    /// <summary>
    /// Represents the Meta Graph API version.
    /// </summary>
    private const string GraphApiVersion = "v23.0";

    private readonly ILogger<MetaCloudWhatsAppClient> _logger;

    private readonly MetaCloudWhatsAppConfiguration _configuration;

    private readonly IMetaCloudWhatsAppSdkClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MetaCloudWhatsAppClient"/> class.
    /// </summary>
    /// <param name="factory">
    /// The Meta Cloud SDK client factory.
    /// </param>
    /// <param name="httpClientFactory">
    /// The HTTP client factory.
    /// </param>
    /// <param name="options">
    /// The communication options.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public MetaCloudWhatsAppClient(
        IMetaCloudWhatsAppSdkClientFactory factory,
        IHttpClientFactory httpClientFactory,
        IOptions<CommunicationOptions> options,
        ILogger<MetaCloudWhatsAppClient> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        _configuration =
            options.Value
                .WhatsApp
                .MetaCloud;

        ValidateConfiguration(
            _configuration);

        _client =
            factory.Create(
                httpClientFactory.CreateClient());
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
            "Sending WhatsApp message through Meta Cloud to {Recipient}.",
            recipient);

        try
        {
            Uri requestUri =
                CreateRequestUri();

            HttpContent content =
                CreateRequestContent(
                    recipient,
                    message);

            HttpResponseMessage response =
                await _client
                    .SendMessageAsync(
                        requestUri,
                        _configuration.AccessToken,
                        content,
                        cancellationToken)
                    .ConfigureAwait(false);

            ValidateResponse(
                response);

            MetaCloudSendMessageResponse payload =
                await ReadResponseAsync(
                    response,
                    cancellationToken)
                .ConfigureAwait(false);

            VendorDeliveryResult result =
                CreateVendorDeliveryResult(
                    payload,
                    response);

            _logger.LogDebug(
                "Meta Cloud accepted WhatsApp message. MessageId: {MessageId}",
                result.MessageId);

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Meta Cloud WhatsApp request was cancelled.");

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Meta Cloud returned an error while sending WhatsApp message.");

            throw new CommunicationException(
                "Failed to send WhatsApp message using Meta Cloud.",
                exception);
        }
    }

    /// <summary>
    /// Validates the Meta Cloud configuration.
    /// </summary>
    private static void ValidateConfiguration(
        MetaCloudWhatsAppConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            configuration);

        if (string.IsNullOrWhiteSpace(
            configuration.AccessToken))
        {
            throw new InvalidOperationException(
                "Meta Cloud AccessToken is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.PhoneNumberId))
        {
            throw new InvalidOperationException(
                "Meta Cloud PhoneNumberId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(
            configuration.BusinessAccountId))
        {
            throw new InvalidOperationException(
                "Meta Cloud BusinessAccountId is not configured.");
        }
    }

    /// <summary>
    /// Creates the Meta Cloud request URI.
    /// </summary>
    private Uri CreateRequestUri()
    {
        return new Uri(
            $"https://graph.facebook.com/{GraphApiVersion}/{_configuration.PhoneNumberId}/messages");
    }

    /// <summary>
    /// Creates the HTTP request content.
    /// </summary>
    private static HttpContent CreateRequestContent(
        string recipient,
        string message)
    {
        MetaCloudTextMessageRequest payload =
            new()
            {
                To = recipient,

                Text =
                    new MetaCloudTextMessage
                    {
                        Body = message,
                        PreviewUrl = false
                    }
            };

        return JsonContent.Create(
            payload);
    }

    /// <summary>
    /// Validates the Meta Cloud response.
    /// </summary>
    private static void ValidateResponse(
        HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(
            response);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Meta Cloud returned status code {response.StatusCode}.");
        }
    }

    /// <summary>
    /// Reads the Meta Cloud response payload.
    /// </summary>
    private static async Task<MetaCloudSendMessageResponse>
        ReadResponseAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
    {
        MetaCloudSendMessageResponse? payload =
            await response.Content
                .ReadFromJsonAsync<MetaCloudSendMessageResponse>(
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        return payload
            ?? throw new InvalidOperationException(
                "Meta Cloud returned an empty response.");
    }

    /// <summary>
    /// Creates a vendor delivery result.
    /// </summary>
    private static VendorDeliveryResult
        CreateVendorDeliveryResult(
            MetaCloudSendMessageResponse payload,
            HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(
            payload);

        MetaCloudMessage message =
            payload.Messages?
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                "Meta Cloud returned an empty message collection.");

        return VendorDeliveryResult.Success(
            messageId: message.Id,
            providerReference: message.Id,
            status: response.StatusCode.ToString(),
            rawResponse: payload);
    }
}