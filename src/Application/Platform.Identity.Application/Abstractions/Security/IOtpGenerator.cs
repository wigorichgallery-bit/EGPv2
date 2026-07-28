namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Generates one-time passwords (OTP) for authentication
/// challenges.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate cryptographically secure one-time passwords.
/// </description>
/// </item>
/// <item>
/// <description>
/// Abstract OTP generation from infrastructure-specific
/// random number generation.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not hash or protect generated OTP values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist data.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface IOtpGenerator
{
    /// <summary>
    /// Generates a cryptographically secure numeric OTP.
    /// </summary>
    /// <returns>
    /// Plain-text OTP.
    /// </returns>
    string Generate();
}