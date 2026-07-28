// ===========================================
// File Location:
// src/Infrastructure/Platform.Security.Infrastructure/
// Totp/
// TotpSecretGenerator.cs
// ===========================================

using System.Security.Cryptography;
using System.Text;
using Platform.Identity.Application.Abstractions.Security;

namespace Platform.Security.Infrastructure.Totp;

/// <summary>
/// Provides cryptographically secure TOTP shared secret
/// generation.
///
/// <para>
/// Responsibility:
/// - Generate cryptographically secure shared secrets.
/// - Encode secrets using RFC 4648 Base32.
/// - Produce secrets compatible with RFC 6238
/// authenticator applications.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Infrastructure layer only.
/// - No authentication workflow.
/// - No persistence.
/// - No TOTP verification.
/// - No TOTP code generation.
/// </para>
///
/// <para>
/// Security Model:
/// - Uses RandomNumberGenerator.
/// - Generates 160-bit secrets.
/// - Encodes secrets using Base32 without padding.
/// </para>
/// </summary>
public sealed class TotpSecretGenerator
    : ITotpSecretGenerator
{
    /// <summary>
    /// Default TOTP secret size in bytes.
    /// RFC 4226 recommends 160-bit shared secrets.
    /// </summary>
    private const int SecretLength = 20;

    /// <inheritdoc />
    public string GenerateSecret()
    {
        byte[] secret =
            RandomNumberGenerator.GetBytes(
                SecretLength);

        return Base32Encode(secret);
    }

    /// <summary>
    /// Encodes bytes using RFC 4648 Base32 without padding.
    /// </summary>
    /// <param name="data">
    /// Binary data.
    /// </param>
    /// <returns>
    /// Base32 encoded string.
    /// </returns>
    private static string Base32Encode(
        ReadOnlySpan<byte> data)
    {
        const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        StringBuilder builder =
            new();

        int buffer = 0;
        int bitsLeft = 0;

        foreach (byte value in data)
        {
            buffer <<= 8;
            buffer |= value;
            bitsLeft += 8;

            while (bitsLeft >= 5)
            {
                builder.Append(
                    Alphabet[
                        (buffer >> (bitsLeft - 5)) & 31]);

                bitsLeft -= 5;
            }
        }

        if (bitsLeft > 0)
        {
            builder.Append(
                Alphabet[
                    (buffer << (5 - bitsLeft)) & 31]);
        }

        return builder.ToString();
    }
}