namespace Platform.Communication.Channels.Email.Configuration;

/// <summary>
/// Represents SMTP provider configuration.
/// </summary>
public sealed class SmtpConfiguration
{
    /// <summary>
    /// Gets or sets the SMTP host.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SMTP port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether SSL is enabled.
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// Gets or sets the default sender email address.
    /// </summary>
    public string SenderAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default sender display name.
    /// </summary>
    public string SenderName { get; set; } = string.Empty;
}