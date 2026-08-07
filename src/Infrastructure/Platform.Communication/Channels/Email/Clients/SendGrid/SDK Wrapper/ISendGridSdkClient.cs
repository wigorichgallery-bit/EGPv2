using SendGrid.Helpers.Mail;

namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Provides an abstraction over the SendGrid SDK client.
/// </summary>
internal interface ISendGridSdkClient
{
    /// <summary>
    /// Sends an email using the SendGrid SDK.
    /// </summary>
    Task<SendGrid.Response> SendEmailAsync(
        SendGridMessage message,
        CancellationToken cancellationToken = default);
}