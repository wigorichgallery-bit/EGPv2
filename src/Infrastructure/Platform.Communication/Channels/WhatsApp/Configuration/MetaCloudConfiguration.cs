namespace Platform.Communication.Channels.WhatsApp.Configuration;

/// <summary>
/// Represents the Meta WhatsApp Cloud API configuration.
/// </summary>
public sealed class MetaCloudConfiguration
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number identifier.
    /// </summary>
    public string PhoneNumberId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the business account identifier.
    /// </summary>
    public string BusinessAccountId { get; set; } = string.Empty;
}