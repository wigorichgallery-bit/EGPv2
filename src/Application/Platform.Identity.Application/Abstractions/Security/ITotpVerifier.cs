// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Security/
// ITotpVerifier.cs
// ===========================================

namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Verifies RFC 6238 Time-based One-Time Password (TOTP)
/// codes.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Verify TOTP codes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Apply validation time windows.
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
/// Must not generate shared secrets.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not generate TOTP codes directly.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist data.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface ITotpVerifier
{
    /// <summary>
    /// Verifies a TOTP code.
    /// </summary>
    /// <param name="secret">
    /// Base32 encoded shared secret.
    /// </param>
    /// <param name="code">
    /// User supplied TOTP code.
    /// </param>
    /// <param name="utcNow">
    /// Current UTC timestamp.
    /// </param>
    /// <returns>
    /// <c>true</c> if the code is valid; otherwise
    /// <c>false</c>.
    /// </returns>
    bool Verify(
        string secret,
        string code,
        DateTime utcNow);
}