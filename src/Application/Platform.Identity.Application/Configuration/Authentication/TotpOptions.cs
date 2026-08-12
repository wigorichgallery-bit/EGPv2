// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Configuration/
// TotpOptions.cs
// ===========================================

namespace Platform.Identity.Application.Configuration.Authentication;

/// <summary>
/// Represents configuration options for Time-based
/// One-Time Password (TOTP) authentication.
///
/// <para>
/// These options are shared by the TOTP secret generator,
/// code generator, verifier, and provisioning service.
/// </para>
///
/// <para>
/// Values should remain consistent across all TOTP
/// operations to ensure interoperability with standard
/// authenticator applications.
/// </para>
/// </summary>
public sealed class TotpOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Authentication:Totp";

    /// <summary>
    /// Gets or initializes the issuer name displayed by
    /// authenticator applications.
    /// </summary>
    public string Issuer { get; init; } = "Platform";

    /// <summary>
    /// Gets or initializes the number of digits contained
    /// in each generated verification code.
    /// </summary>
    public int Digits { get; init; } = 6;

    /// <summary>
    /// Gets or initializes the lifetime of each TOTP time
    /// step in seconds.
    /// </summary>
    public int TimeStepSeconds { get; init; } = 30;

    /// <summary>
    /// Gets or initializes the number of adjacent time
    /// steps accepted during verification to compensate
    /// for clock skew.
    /// </summary>
    public int AllowedTimeSteps { get; init; } = 1;

    /// <summary>
    /// Gets or initializes the number of random bytes used
    /// when generating a shared secret before Base32
    /// encoding.
    /// </summary>
    public int SecretLengthBytes { get; init; } = 20;
}