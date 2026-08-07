using System.Net.Http;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Provides an abstraction over the Meta Cloud WhatsApp API.
/// </summary>
internal interface IMetaCloudWhatsAppSdkClient
{
    /// <summary>
    /// Sends a WhatsApp message using the Meta Cloud API.
    /// </summary>
    /// <param name="requestUri">
    /// The Meta Cloud API endpoint.
    /// </param>
    /// <param name="accessToken">
    /// The Meta Cloud access token.
    /// </param>
    /// <param name="content">
    /// The HTTP request content.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The HTTP response returned by the Meta Cloud API.
    /// </returns>
    Task<HttpResponseMessage> SendMessageAsync(
        Uri requestUri,
        string accessToken,
        HttpContent content,
        CancellationToken cancellationToken = default);
}