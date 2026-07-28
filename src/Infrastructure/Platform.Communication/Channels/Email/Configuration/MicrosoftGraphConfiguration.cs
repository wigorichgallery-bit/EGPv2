namespace Platform.Communication.Channels.Email.Configuration;

/// <summary>
/// Represents Microsoft Graph Mail configuration.
/// </summary>
public sealed class MicrosoftGraphConfiguration
{
    /// <summary>
    /// Gets or sets the Azure AD tenant identifier.
    /// </summary>
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Azure AD application client identifier.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Azure AD application client secret.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mailbox user identifier or email address.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
}