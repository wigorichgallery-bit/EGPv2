namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates MailKit SMTP client instances.
/// </summary>
internal interface IMailKitSmtpClientFactory
{
    /// <summary>
    /// Creates a MailKit SMTP client wrapper.
    /// </summary>
    IMailKitSmtpClient Create();
}