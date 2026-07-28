// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Factories/
// AuthenticationChallengeSecretFactory.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.Features.Authentication.Factories;

/// <summary>
/// Creates protected authentication challenge secrets.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate authentication challenge secrets for supported
/// authentication challenge types.
/// </description>
/// </item>
/// <item>
/// <description>
/// Protect generated secrets before they enter the domain
/// model.
/// </description>
/// </item>
/// <item>
/// <description>
/// Return both the protected secret for persistence and the
/// plaintext secret required for challenge delivery or
/// provisioning.
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
/// Must not persist data.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not dispatch authentication challenges.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not orchestrate authentication workflows.
/// </description>
/// </item>
/// </list>
/// </summary>
public sealed class AuthenticationChallengeSecretFactory
    : IAuthenticationChallengeSecretFactory
{
    private readonly IOtpGenerator _otpGenerator;
    private readonly ITotpSecretGenerator _totpSecretGenerator;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeSecretFactory"/>
    /// class.
    /// </summary>
    /// <param name="otpGenerator">
    /// OTP generator.
    /// </param>
    /// <param name="totpSecretGenerator">
    /// TOTP shared secret generator.
    /// </param>
    /// <param name="passwordHasher">
    /// Secret hasher.
    /// </param>
    public AuthenticationChallengeSecretFactory(
        IOtpGenerator otpGenerator,
        ITotpSecretGenerator totpSecretGenerator,
        IPasswordHasher passwordHasher)
    {
        ArgumentNullException.ThrowIfNull(
            otpGenerator);

        ArgumentNullException.ThrowIfNull(
            totpSecretGenerator);

        ArgumentNullException.ThrowIfNull(
            passwordHasher);

        _otpGenerator =
            otpGenerator;

        _totpSecretGenerator =
            totpSecretGenerator;

        _passwordHasher =
            passwordHasher;
    }

    /// <inheritdoc />
    public AuthenticationChallengeSecretResult Create(
        AuthenticationChallengeType challengeType)
    {
        return challengeType switch
        {
            AuthenticationChallengeType.EmailOtp =>
                CreateOtpSecret(),

            AuthenticationChallengeType.SmsOtp =>
                CreateOtpSecret(),

            AuthenticationChallengeType.WhatsAppOtp =>
                CreateOtpSecret(),

            AuthenticationChallengeType.Totp =>
                CreateTotpSecret(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(challengeType),
                challengeType,
                "Unsupported authentication challenge type.")
        };
    }

    /// <summary>
    /// Creates a protected OTP challenge secret.
    /// </summary>
    /// <returns>
    /// The protected OTP secret and its corresponding
    /// plaintext value.
    /// </returns>
    private AuthenticationChallengeSecretResult
        CreateOtpSecret()
    {
        string otp =
            _otpGenerator.Generate();

        string protectedSecret =
            _passwordHasher.Hash(
                otp);

        return new AuthenticationChallengeSecretResult(
            new ChallengeSecret(
                protectedSecret),
            otp);
    }

    /// <summary>
    /// Creates a TOTP shared secret.
    /// </summary>
    /// <returns>
    /// The generated shared secret for persistence and
    /// provisioning.
    /// </returns>
    private AuthenticationChallengeSecretResult
        CreateTotpSecret()
    {
        string secret =
            _totpSecretGenerator.GenerateSecret();

        return new AuthenticationChallengeSecretResult(
            new ChallengeSecret(
                secret),
            secret);
    }
}