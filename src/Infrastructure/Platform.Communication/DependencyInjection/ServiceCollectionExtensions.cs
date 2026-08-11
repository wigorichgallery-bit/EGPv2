using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Platform.Communication.DependencyInjection;

/// <summary>
/// Provides dependency injection registration for
/// Platform.Communication.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Platform.Communication services.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <param name="configuration">
    /// The application configuration.
    /// </param>
    /// <returns>
    /// The updated service collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or
    /// <paramref name="configuration"/> is
    /// <see langword="null"/>.
    /// </exception>
    public static IServiceCollection AddPlatformCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        services
            .AddCommunicationOptions(
                configuration)
            .AddCommunicationClients()
            .AddCommunicationProviders()
            .AddCommunicationSenders();

        return services;
    }
}