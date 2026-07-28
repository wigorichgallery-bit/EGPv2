// ===========================================
// File Location:
// src/Infrastructure/Platform.Security.Infrastructure/
// Totp/
// TotpCodeGenerator.cs
// ===========================================
using Microsoft.Extensions.Options;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Configuration.Authentication;

namespace Platform.Security.Infrastructure.Totp;

/// <summary>
/// Generates RFC 6238 compliant Time-based One-Time
/// Password (TOTP) codes.
///
/// <para>
/// Responsibility:
/// - Generate TOTP codes.
/// - Implement RFC 6238.
/// - Decode Base32 secrets.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Infrastructure only.
/// - No verification.
/// - No persistence.
/// - No workflow orchestration.
/// </para>
/// </summary>
public sealed class TotpCodeGenerator
    : ITotpCodeGenerator
{
    private readonly TotpOptions _options;
    private readonly int _modulus;
    public TotpCodeGenerator(
    IOptions<TotpOptions> options)
    {
        _options = options.Value;
        _modulus = GetModulus(_options.Digits);
    }

    /// <inheritdoc />
    public string GenerateCode(
        string secret,
        DateTime utcNow)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        byte[] key =
            Base32Decode(secret);

        long counter =
            new DateTimeOffset(
                utcNow,
                TimeSpan.Zero)
            .ToUnixTimeSeconds()
            / _options.TimeStepSeconds;

        Span<byte> counterBytes =
            stackalloc byte[8];

        BinaryPrimitives.WriteInt64BigEndian(
            counterBytes,
            counter);

        byte[] hash =
            HMACSHA1.HashData(
                key,
                counterBytes);

        int offset =
            hash[^1] & 0x0F;

        int binary =
            ((hash[offset] & 0x7F) << 24)
            | (hash[offset + 1] << 16)
            | (hash[offset + 2] << 8)
            | hash[offset + 3];

        int otp =
            binary % _modulus;

        return otp.ToString(
            $"D{_options.Digits}");
    }

    private static byte[] Base32Decode(
        string value)
    {
        const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        value =
            value.TrimEnd('=').ToUpperInvariant();

        List<byte> bytes =
            [];

        int buffer = 0;
        int bitsLeft = 0;

        foreach (char c in value)
        {
            int index =
                Alphabet.IndexOf(c);

            if (index < 0)
            {
                throw new FormatException(
                    "Invalid Base32 secret.");
            }

            buffer <<= 5;
            buffer |= index;
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                bytes.Add(
                    (byte)((buffer >> (bitsLeft - 8)) & 0xFF));

                bitsLeft -= 8;
            }
        }

        return bytes.ToArray();
    }

    private static int GetModulus(int digits)
    {
        int modulus = 1;

        for (int i = 0; i < digits; i++)
        {
            modulus *= 10;
        }

        return modulus;
    }
}