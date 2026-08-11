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
    /// Registers communication clients and their SDK wrappers.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <returns>
    /// The updated service collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> is
    /// <see langword="null"/>.
    /// </exception>
    internal static IServiceCollection AddCommunicationClients(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(
            services);

        // ==========================================================
        // Email
        // ==========================================================

        services.AddTransient<
            IMailKitSmtpSdkClientFactory,
            MailKitSmtpSdkClientFactory>();

        services.AddSingleton<
            ISendGridSdkClientFactory,
            SendGridSdkClientFactory>();

        services.AddTransient<
            IMailKitSmtpClient,
            MailKitSmtpClient>();

        services.AddTransient<
            ISendGridClient,
            SendGridClient>();

        services.AddTransient<
            IGraphClient,
            GraphClient>();

        // ==========================================================
        // SMS
        // ==========================================================

        services.AddTransient<
            ITwilioSmsClient,
            TwilioSmsClient>();

        services.AddTransient<
            IVonageSmsClient,
            VonageSmsClient>();

        // ==========================================================
        // WhatsApp
        // ==========================================================

        services.AddTransient<
            IMetaCloudWhatsAppClient,
            MetaCloudWhatsAppClient>();

        services.AddTransient<
            ITwilioWhatsAppClient,
            TwilioWhatsAppClient>();

        return services;
    }
}