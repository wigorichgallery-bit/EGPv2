// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Configuration/
// AuthenticationMessageOptions.cs
// ===========================================

namespace Platform.Identity.Application.Configuration.Authentication;

/// <summary>
/// Represents configurable messaging settings used when
/// delivering authentication challenges.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Provide configurable message metadata for
/// authentication delivery.
/// </description>
/// </item>
/// <item>
/// <description>
/// Centralize authentication message configuration across
/// all supported delivery channels.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This configuration is consumed by authentication message
/// formatters and must not contain transport-specific
/// settings.
/// </para>
/// </summary>
public sealed class AuthenticationMessageOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Authentication:Messages";

    /// <summary>
    /// Gets or sets the application name displayed in
    /// authentication messages.
    /// </summary>
    public string ApplicationName { get; init; }
        = string.Empty;

    /// <summary>
    /// Gets or sets the email subject used for verification
    /// code messages.
    /// </summary>
    public string VerificationCodeEmailSubject { get; init; }
        = "Your verification code";

    /// <summary>
    /// Gets or sets the SMS prefix used for verification
    /// code messages.
    /// </summary>
    public string VerificationCodeSmsPrefix { get; init; }
        = "Your verification code is";

    /// <summary>
    /// Gets or sets the WhatsApp prefix used for verification
    /// code messages.
    /// </summary>
    public string VerificationCodeWhatsAppPrefix { get; init; }
        = "Your verification code is";

    /// <summary>
    /// Gets or sets the message displayed when the recipient
    /// did not request the authentication challenge.
    /// </summary>
    public string IgnoreMessage { get; init; }
        = "If you did not request this authentication challenge, you can safely ignore this message.";
}