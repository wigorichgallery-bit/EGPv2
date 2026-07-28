namespace Platform.Communication.Channels.Email.Configuration;

/// <summary>
/// Represents the email provider configurations.
/// </summary>
public sealed class EmailConfiguration
{
    /// <summary>
    /// Gets the SMTP configuration.
    /// </summary>
    public SmtpConfiguration Smtp { get; init; } = new();

    /// <summary>
    /// Gets the Microsoft Graph configuration.
    /// </summary>
    public MicrosoftGraphConfiguration MicrosoftGraph { get; init; } = new();

    /// <summary>
    /// Gets the SendGrid configuration.
    /// </summary>
    public SendGridConfiguration SendGrid { get; init; } = new();
}