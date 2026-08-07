using Azure.Identity;

using Microsoft.Graph;

using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates Microsoft Graph SDK clients.
/// </summary>
internal sealed class GraphSdkClientFactory
    : IGraphSdkClientFactory
{
    /// <inheritdoc />
    public IGraphSdkClient Create(
        MicrosoftGraphConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            configuration.TenantId);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            configuration.ClientId);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            configuration.ClientSecret);

        var credential =
            new ClientSecretCredential(
                configuration.TenantId,
                configuration.ClientId,
                configuration.ClientSecret);

        var graphServiceClient =
            new GraphServiceClient(
                credential);

        return new GraphSdkClient(
            graphServiceClient);
    }
}