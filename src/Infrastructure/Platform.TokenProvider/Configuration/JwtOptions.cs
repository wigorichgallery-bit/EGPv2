// ===========================================
// File Location :
// src/Infrastructure/Platform.TokenProvider/Configuration/JwtOptions.cs
// ===========================================
namespace Platform.TokenProvider.Configuration;

/// <summary>
/// Represents the strongly typed configuration for JSON Web Token (JWT)
/// generation and validation.
///
/// <para>
/// This class is bound from the <c>Jwt</c> section of the application
/// configuration using the Microsoft Options Pattern.
/// </para>
///
/// <para>
/// Responsibilities:
/// <list type="bullet">
/// <item><description>Provide JWT issuer configuration.</description></item>
/// <item><description>Provide JWT audience configuration.</description></item>
/// <item><description>Provide signing secret configuration.</description></item>
/// <item><description>Provide access token lifetime.</description></item>
/// <item><description>Provide refresh token lifetime.</description></item>
/// </list>
/// </para>
///
/// <para>
/// This class contains configuration data only and must not contain
/// validation logic or runtime behavior.
/// </para>
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Gets or sets the JWT issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JWT audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the secret key used to sign JWT tokens.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the access token lifetime in minutes.
    /// </summary>
    public int AccessTokenLifetimeMinutes { get; set; }

    /// <summary>
    /// Gets or sets the refresh token lifetime in days.
    /// </summary>
    public int RefreshTokenLifetimeDays { get; set; }
}