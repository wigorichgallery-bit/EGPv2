using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.Enums;
using Platform.Communication.Options;

namespace Platform.Communication.DependencyInjection;

/// <summary>
/// Provides communication provider registrations.
/// </summary>
internal static class AddCommunicationProvidersExtensions
{
    /// <summary>
    /// Registers communication providers.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <returns>
    /// The updated service collection.
    /// </returns>
    internal static IServiceCollection AddCommunicationProviders(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // -----------------------------------------------------------------
        // Email Providers
        // -----------------------------------------------------------------

        services.AddTransient<SmtpEmailProvider>();

        services.AddTransient<SendGridEmailProvider>();

        services.AddTransient<MicrosoftGraphEmailProvider>();

        services.AddTransient<IEmailProvider>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<CommunicationOptions>>()
                .Value;

            return options.Email.Provider switch
            {
                EmailProviderType.Smtp =>
                    serviceProvider.GetRequiredService<SmtpEmailProvider>(),

                EmailProviderType.SendGrid =>
                    serviceProvider.GetRequiredService<SendGridEmailProvider>(),

                EmailProviderType.MicrosoftGraph =>
                    serviceProvider.GetRequiredService<MicrosoftGraphEmailProvider>(),

                _ => throw new NotSupportedException(
                    $"The configured email provider '{options.Email.Provider}' is not supported.")
            };
        });

        // -----------------------------------------------------------------
        // SMS Providers
        // -----------------------------------------------------------------

        services.AddTransient<TwilioSmsProvider>();

        services.AddTransient<VonageSmsProvider>();

        services.AddTransient<ISmsProvider>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<CommunicationOptions>>()
                .Value;

            return options.Sms.Provider switch
            {
                SmsProviderType.Twilio =>
                    serviceProvider.GetRequiredService<TwilioSmsProvider>(),

                SmsProviderType.Vonage =>
                    serviceProvider.GetRequiredService<VonageSmsProvider>(),

                _ => throw new NotSupportedException(
                    $"The configured SMS provider '{options.Sms.Provider}' is not supported.")
            };
        });

        // -----------------------------------------------------------------
        // WhatsApp Providers
        // -----------------------------------------------------------------

        services.AddTransient<MetaCloudWhatsAppProvider>();

        services.AddTransient<TwilioWhatsAppProvider>();

        services.AddTransient<IWhatsAppProvider>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<CommunicationOptions>>()
                .Value;

            return options.WhatsApp.Provider switch
            {
                WhatsAppProviderType.MetaCloud =>
                    serviceProvider.GetRequiredService<MetaCloudWhatsAppProvider>(),

                WhatsAppProviderType.Twilio =>
                    serviceProvider.GetRequiredService<TwilioWhatsAppProvider>(),

                _ => throw new NotSupportedException(
                    $"The configured WhatsApp provider '{options.WhatsApp.Provider}' is not supported.")
            };
        });

        return services;
    }
}