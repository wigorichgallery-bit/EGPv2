namespace Platform.Communication.Channels.Email.Configuration;

/// <summary>
/// Represents SendGrid provider configuration.
/// </summary>
public sealed class SendGridConfiguration
{
    /// <summary>
    /// Gets or sets the SendGrid API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default sender email address.
    /// </summary>
    public string SenderAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default sender display name.
    /// </summary>
    public string SenderName { get; set; } = string.Empty;
}