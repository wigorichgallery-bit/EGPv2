namespace Platform.Communication.Channels.Email.Clients;

/// <summary>
/// Default factory for
/// <see cref="IMailKitSmtpSdkClientFactory"/>.
/// </summary>
internal sealed class MailKitSmtpSdkClientFactory
    : IMailKitSmtpSdkClientFactory
{
    /// <inheritdoc />
    public IMailKitSmtpSdkClient Create()
    {
        return new MailKitSmtpSdkClient();
    }
}