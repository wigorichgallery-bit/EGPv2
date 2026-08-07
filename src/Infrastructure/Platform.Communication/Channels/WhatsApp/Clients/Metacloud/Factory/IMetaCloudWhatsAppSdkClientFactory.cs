using System.Net.Http;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Creates instances of
/// <see cref="IMetaCloudWhatsAppSdkClient"/>.
/// </summary>
internal interface IMetaCloudWhatsAppSdkClientFactory
{
    /// <summary>
    /// Creates a new Meta Cloud WhatsApp SDK client.
    /// </summary>
    /// <param name="httpClient">
    /// The HTTP client used to communicate with
    /// the Meta Cloud WhatsApp API.
    /// </param>
    /// <returns>
    /// A configured
    /// <see cref="IMetaCloudWhatsAppSdkClient"/>.
    /// </returns>
    IMetaCloudWhatsAppSdkClient Create(
        HttpClient httpClient);
}