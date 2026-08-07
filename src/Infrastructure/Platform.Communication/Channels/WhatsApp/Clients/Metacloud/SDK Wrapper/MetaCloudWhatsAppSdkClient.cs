using System.Net.Http;
using System.Net.Http.Headers;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides a wrapper around the Meta Cloud WhatsApp HTTP API.
/// </summary>
internal sealed class MetaCloudWhatsAppSdkClient
    : IMetaCloudWhatsAppSdkClient
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="MetaCloudWhatsAppSdkClient"/> class.
    /// </summary>
    /// <param name="httpClient">
    /// The HTTP client.
    /// </param>
    public MetaCloudWhatsAppSdkClient(
        HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(
            httpClient);

        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public Task<HttpResponseMessage> SendMessageAsync(
        Uri requestUri,
        string accessToken,
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(
            requestUri);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            accessToken);

        ArgumentNullException.ThrowIfNull(
            content);

        cancellationToken.ThrowIfCancellationRequested();

        HttpRequestMessage request =
            new(
                HttpMethod.Post,
                requestUri)
            {
                Content = content
            };

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                accessToken);

        return _httpClient.SendAsync(
            request,
            cancellationToken);
    }
}