// ===========================================
// File Location :
// src/Infrastructure/Platform.Security.Infrastructure/
// Otp/OtpGenerator.cs
// ===========================================

using System.Security.Cryptography;
using Platform.Identity.Application.Abstractions.Security;

namespace Platform.Security.Infrastructure.Otp;

/// <summary>
/// Provides cryptographically secure numeric
/// one-time password (OTP) generation.
///
/// <para>
/// Responsibility:
/// - Generate numeric OTP values.
/// - Use a cryptographically secure random source.
/// - Encapsulate OTP generation implementation.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Infrastructure layer only.
/// - No business logic.
/// - No persistence.
/// - No hashing.
/// - No authentication workflow orchestration.
/// </para>
///
/// <para>
/// Security Model:
/// - Uses <see cref="RandomNumberGenerator"/>.
/// - Generates uniformly distributed digits.
/// - Thread-safe.
/// </para>
///
/// <para>
/// Default OTP Format:
/// - Numeric.
/// - Six digits.
/// </para>
/// </summary>
public sealed class OtpGenerator
    : IOtpGenerator
{
    /// <summary>
    /// Number of digits generated for each OTP.
    /// </summary>
    private const int OtpLength = 6;

    /// <inheritdoc />
    public string Generate()
    {
        Span<char> digits =
            stackalloc char[OtpLength];

        for (int i = 0; i < OtpLength; i++)
        {
            digits[i] =
                (char)('0' + RandomNumberGenerator.GetInt32(10));
        }

        return new string(digits);
    }
}