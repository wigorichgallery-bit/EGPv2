using System.Net.Http;

namespace Platform.Communication.Channels.WhatsApp.Clients;

/// <summary>
/// Creates instances of
/// <see cref="IMetaCloudWhatsAppSdkClient"/>.
/// </summary>
internal sealed class MetaCloudWhatsAppSdkClientFactory
    : IMetaCloudWhatsAppSdkClientFactory
{
    /// <inheritdoc />
    public IMetaCloudWhatsAppSdkClient Create(
        HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(
            httpClient);

        return new MetaCloudWhatsAppSdkClient(
            httpClient);
    }
}