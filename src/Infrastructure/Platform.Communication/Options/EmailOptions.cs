using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Enums;

namespace Platform.Communication.Options;

/// <summary>
/// Represents email communication configuration.
/// </summary>
public sealed class EmailOptions
{
    /// <summary>
    /// Gets or sets the active email provider.
    /// </summary>
    public EmailProviderType Provider { get; set; }
    /// <summary>
    /// Gets or sets Email configuration.
    /// </summary>
    public EmailConfiguration Configuration {get;set;} = new();
    
}