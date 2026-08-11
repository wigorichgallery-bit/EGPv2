namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Creates MailKit SMTP SDK client instances.
/// </summary>
internal interface IMailKitSmtpSdkClientFactory
{
    /// <summary>
    /// Creates a MailKit SMTP SDK client wrapper.
    /// </summary>
    /// <returns>
    /// A new MailKit SMTP SDK client wrapper.
    /// </returns>
    IMailKitSmtpSdkClient Create();
}