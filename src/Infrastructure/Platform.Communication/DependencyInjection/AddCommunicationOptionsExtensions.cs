using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Platform.Communication.Options;

namespace Platform.Communication.DependencyInjection;

/// <summary>
/// Provides communication options registrations.
/// </summary>
internal static class AddCommunicationOptionsExtensions
{
    /// <summary>
    /// Registers communication options.
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
    internal static IServiceCollection AddCommunicationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        ArgumentNullException.ThrowIfNull(
            configuration);

        services
            .AddOptions<CommunicationOptions>()
            .Bind(
                configuration.GetSection(
                    CommunicationOptions.SectionName))
            .ValidateOnStart();

        return services;
    }
}