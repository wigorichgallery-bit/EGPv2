using Microsoft.Extensions.DependencyInjection;

using Platform.Communication.Channels.Email.Sender;
using Platform.Communication.Channels.Sms.Sender;
using Platform.Communication.Channels.WhatsApp.Sender;

namespace Platform.Communication.DependencyInjection;

/// <summary>
/// Provides sender service registrations.
/// </summary>
internal static class AddCommunicationSendersExtensions
{
    /// <summary>
    /// Registers communication senders.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <returns>
    /// The updated service collection.
    /// </returns>
    internal static IServiceCollection AddCommunicationSenders(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddTransient<IEmailSender, EmailSender>()
            .AddTransient<ISmsSender, SmsSender>()
            .AddTransient<IWhatsAppSender, WhatsAppSender>();

        return services;
    }
}