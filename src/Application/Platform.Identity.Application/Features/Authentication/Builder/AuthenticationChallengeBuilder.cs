// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Builders/
// AuthenticationChallengeBuilder.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Abstractions.Common;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Abstractions;

namespace Platform.Identity.Application.Features.Authentication.Builders;

/// <summary>
/// Builds fully initialized authentication challenge
/// aggregates.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Resolve the authentication challenge type from the
/// user's configured MFA method.
/// </description>
/// </item>
/// <item>
/// <description>
/// Generate the authentication challenge secret.
/// </description>
/// </item>
/// <item>
/// <description>
/// Create a fully initialized
/// <see cref="AuthenticationChallenge"/> aggregate.
/// </description>
/// </item>
/// <item>
/// <description>
/// Preserve the plaintext authentication secret required
/// for challenge delivery.
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
/// Must not persist aggregates.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not deliver authentication challenges.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not contain login business rules.
/// </description>
/// </item>
/// </list>
/// </summary>
public sealed class AuthenticationChallengeBuilder
    : IAuthenticationChallengeBuilder
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IClock _clock;
    private readonly AuthenticationChallengeOptions _options;
    private readonly IAuthenticationChallengeSecretFactory _challengeSecretFactory;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationChallengeBuilder"/> class.
    /// </summary>
    /// <param name="guidGenerator">
    /// Generates authentication challenge identifiers.
    /// </param>
    /// <param name="clock">
    /// Provides the current UTC time.
    /// </param>
    /// <param name="options">
    /// Authentication challenge configuration.
    /// </param>
    /// <param name="challengeSecretFactory">
    /// Creates authentication challenge secrets.
    /// </param>
    public AuthenticationChallengeBuilder(
        IGuidGenerator guidGenerator,
        IClock clock,
        AuthenticationChallengeOptions options,
        IAuthenticationChallengeSecretFactory challengeSecretFactory)
    {
        ArgumentNullException.ThrowIfNull(guidGenerator);
        ArgumentNullException.ThrowIfNull(clock);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(challengeSecretFactory);

        _guidGenerator = guidGenerator;
        _clock = clock;
        _options = options;
        _challengeSecretFactory = challengeSecretFactory;
    }

    /// <inheritdoc />
    public AuthenticationChallengeBuildResult Build(
        UserAccount user,
        AuthenticationChallengePurpose purpose)
    {
        ArgumentNullException.ThrowIfNull(user);

        AuthenticationChallengeType challengeType =
            AuthenticationChallengeTypeResolver.Resolve(
                user.MFAMethod);

        AuthenticationChallengeSecretResult secretResult =
            _challengeSecretFactory.Create(
                challengeType);

        Guid challengeId =
            _guidGenerator.Create();

        DateTime createdAtUtc =
            _clock.UtcNow;

        DateTime expiresAtUtc =
            createdAtUtc.Add(
                _options.LoginChallengeLifetime);

        AuthenticationChallenge challenge =
            AuthenticationChallenge.Create(
                challengeId,
                user.Id,
                challengeType,
                purpose,
                secretResult.Secret,
                createdAtUtc,
                expiresAtUtc);

        return new AuthenticationChallengeBuildResult(
            challenge,
            secretResult.PlainTextSecret);
    }
}