// ===========================================
// File Location:
// src/Infrastructure/Platform.Security.Infrastructure/
// Totp/
// TotpVerifier.cs
// ===========================================
using Microsoft.Extensions.Options;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Abstractions.Security;

namespace Platform.Security.Infrastructure.Totp;

/// <summary>
/// Verifies RFC 6238 compliant TOTP codes.
///
/// <para>
/// Responsibility:
/// - Verify TOTP codes.
/// - Apply configurable validation window.
/// </para>
/// </summary>
public sealed class TotpVerifier
    : ITotpVerifier
{
    private readonly ITotpCodeGenerator _generator;
    private readonly TotpOptions _options;
    public TotpVerifier(
        ITotpCodeGenerator generator,
        IOptions<TotpOptions> options)
    {        
        _generator = generator;
        _options = options.Value;

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
        _options.TimeStepSeconds);

        ArgumentOutOfRangeException.ThrowIfNegative(
        _options.AllowedTimeSteps);
    }

    /// <inheritdoc />
    public bool Verify(
        string secret,
        string code,
        DateTime utcNow)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        for (int offset = -_options.AllowedTimeSteps;
             offset <= _options.AllowedTimeSteps;
             offset++)
        {
            DateTime candidate =
                utcNow.AddSeconds(
                    offset * _options.TimeStepSeconds);

            string expected =
                _generator.GenerateCode(
                    secret,
                    candidate);

            if (string.Equals(
                expected,
                code,
                StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}