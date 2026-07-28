// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Security/
// ITotpSecretGenerator.cs
// ===========================================

namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Generates shared secrets for Time-based One-Time Password
/// (TOTP) authentication.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate cryptographically secure shared secrets.
/// </description>
/// </item>
/// <item>
/// <description>
/// Abstract TOTP secret generation from infrastructure-
/// specific implementations.
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
/// Must not generate TOTP codes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not verify TOTP codes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist secrets.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not encrypt or hash generated secrets.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Design Notes:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generated secrets are intended to comply with RFC 6238
/// and RFC 4226 compatible authenticators.
/// </description>
/// </item>
/// <item>
/// <description>
/// Implementations should generate secrets suitable for
/// Base32 encoding and authenticator applications such as
/// Microsoft Authenticator, Google Authenticator,
/// Authy, and compatible clients.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface ITotpSecretGenerator
{
    /// <summary>
    /// Generates a new cryptographically secure shared
    /// secret.
    /// </summary>
    /// <returns>
    /// A Base32-compatible shared secret.
    /// </returns>
    string GenerateSecret();
}