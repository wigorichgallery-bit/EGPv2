// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Security/
// ITotpCodeGenerator.cs
// ===========================================

namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Generates Time-based One-Time Password (TOTP) codes
/// from shared secrets.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate RFC 6238 compatible TOTP codes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Abstract TOTP code generation from infrastructure
/// implementations.
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
/// Must not generate shared secrets.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not verify TOTP codes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist data.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface ITotpCodeGenerator
{
    /// <summary>
    /// Generates a TOTP code for the specified shared secret.
    /// </summary>
    /// <param name="secret">
    /// Base32 encoded shared secret.
    /// </param>
    /// <param name="utcNow">
    /// Current UTC timestamp.
    /// </param>
    /// <returns>
    /// Six-digit TOTP code.
    /// </returns>
    string GenerateCode(
        string secret,
        DateTime utcNow);
}