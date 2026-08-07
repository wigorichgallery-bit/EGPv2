using Platform.Communication.Channels.Email.Configuration;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates Microsoft Graph SDK clients.
/// </summary>
internal interface IGraphSdkClientFactory
{
    /// <summary>
    /// Creates a Microsoft Graph SDK client.
    /// </summary>
    /// <param name="configuration">
    /// The Microsoft Graph configuration.
    /// </param>
    /// <returns>
    /// A configured <see cref="IGraphSdkClient"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configuration"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more configuration values are invalid.
    /// </exception>
    IGraphSdkClient Create(
        MicrosoftGraphConfiguration configuration);
}