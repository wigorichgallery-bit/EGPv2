using Microsoft.Extensions.DependencyInjection;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Sms.Clients;
using Platform.Communication.Channels.WhatsApp.Clients;

namespace Platform.Communication.DependencyInjection;

/// <summary>
/// Provides communication client registrations.
/// </summary>
internal static class AddCommunicationClientsExtensions
{
    /// <summary>
    /// Registers communication clients.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <returns>
    /// The updated service collection.
    /// </returns>
    internal static IServiceCollection AddCommunicationClients(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Email
        services.AddTransient<ISmtpClient, SmtpClient>();
        services.AddTransient<ISendGridClient, SendGridClient>();
        services.AddTransient<IGraphClient, GraphClient>();

        // SMS
        services.AddTransient<ITwilioSmsClient, TwilioSmsClient>();
        services.AddTransient<IVonageSmsClient, VonageSmsClient>();

        // WhatsApp
        services.AddTransient<IMetaCloudClient, MetaCloudClient>();
        services.AddTransient<ITwilioWhatsAppClient, TwilioWhatsAppClient>();

        return services;
    }
}