namespace Platform.Communication.Enums;

/// <summary>
/// Defines the supported email providers.
/// </summary>
public enum EmailProviderType
{
    /// <summary>
    /// SMTP provider.
    /// </summary>
    Smtp = 0,

    /// <summary>
    /// Microsoft Graph Mail API provider.
    /// </summary>
    MicrosoftGraph = 1,

    /// <summary>
    /// SendGrid provider.
    /// </summary>
    SendGrid = 2
}