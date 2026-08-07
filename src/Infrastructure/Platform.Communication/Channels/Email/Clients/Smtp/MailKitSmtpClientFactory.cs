namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Default factory for <see cref="IMailKitSmtpClient"/>.
/// </summary>
internal sealed class MailKitSmtpClientFactory
    : IMailKitSmtpClientFactory
{
    /// <inheritdoc />
    public IMailKitSmtpClient Create()
    {
        return new MailKitSmtpClient();
    }
}