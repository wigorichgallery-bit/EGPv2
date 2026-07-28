namespace Platform.Communication.Options;

/// <summary>
/// Represents communication configuration.
/// </summary>
public sealed class CommunicationOptions
{
    /// <summary>
    /// Gets the root configuration section name.
    /// </summary>
    public const string SectionName = "Communication";

    /// <summary>
    /// Gets or sets the email configuration.
    /// </summary>
    public EmailOptions Email { get; set; } = new();

    /// <summary>
    /// Gets or sets the SMS configuration.
    /// </summary>
    public SmsOptions Sms { get; set; } = new();

    /// <summary>
    /// Gets or sets the WhatsApp configuration.
    /// </summary>
    public WhatsAppOptions WhatsApp { get; set; } = new();
}