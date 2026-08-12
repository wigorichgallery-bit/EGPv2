using Microsoft.Extensions.Options;
using Platform.Identity.Application.Configuration.Authentication;

namespace Platform.Security.Infrastructure.UnitTests.Fixtures;

/// <summary>
/// Provides reusable TOTP configuration and test data
/// for infrastructure unit tests.
/// </summary>
public static class TotpFixture
{
    /// <summary>
    /// Default Base32 secret used by TOTP tests.
    /// </summary>
    public const string Secret =
        "JBSWY3DPEHPK3PXP";

    /// <summary>
    /// Default verification code used by TOTP tests.
    /// </summary>
    public const string ValidCode =
        "123456";

    /// <summary>
    /// Fixed UTC timestamp used to produce deterministic
    /// TOTP test results.
    /// </summary>
    public static readonly DateTime UtcNow =
        new(
            2026,
            1,
            1,
            12,
            0,
            0,
            DateTimeKind.Utc);

    /// <summary>
    /// Creates a default <see cref="TotpOptions"/> instance.
    /// </summary>
    /// <param name="configure">
    /// Optional configuration override.
    /// </param>
    /// <returns>
    /// Configured <see cref="IOptions{TotpOptions}"/>.
    /// </returns>
    public static IOptions<TotpOptions> CreateOptions(
        Action<TotpOptions>? configure = null)
    {
        var options = new TotpOptions
        {
            Issuer = "EGPv2",
            Digits = 6,
            TimeStepSeconds = 30,
            AllowedTimeSteps = 1
        };

        configure?.Invoke(options);

        return Options.Create(options);
    }
}