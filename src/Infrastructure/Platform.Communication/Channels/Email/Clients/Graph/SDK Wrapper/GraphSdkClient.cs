using Microsoft.Graph;
using Microsoft.Graph.Users.Item.SendMail;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides the default implementation of
/// <see cref="IGraphSdkClient"/>.
/// </summary>
internal sealed class GraphSdkClient
    : IGraphSdkClient
{
    private readonly GraphServiceClient _client;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GraphSdkClient"/> class.
    /// </summary>
    /// <param name="client">
    /// The Microsoft Graph service client.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="client"/> is
    /// <see langword="null"/>.
    /// </exception>
    public GraphSdkClient(
        GraphServiceClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    /// <inheritdoc />
    public Task SendMailAsync(
        string userId,
        SendMailPostRequestBody request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(request);

        return _client
            .Users[userId]
            .SendMail
            .PostAsync(
                request,
                cancellationToken: cancellationToken);
    }
}