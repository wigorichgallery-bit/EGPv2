using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.UnitTests.Fixtures;

/// <summary>
/// Provides reusable <see cref="AuthenticationChallenge"/> instances
/// for application unit tests.
///
/// <para>
/// This fixture centralizes aggregate creation to eliminate duplicated
/// setup code across unit tests.
/// </para>
/// </summary>
public static class AuthenticationChallengeFixture
{
    /// <summary>
    /// Default protected challenge secret used when
    /// no explicit secret is supplied.
    /// </summary>
    private static readonly ChallengeSecret DefaultSecret =
        new("HASHED_SECRET");

    /// <summary>
    /// Creates a valid authentication challenge aggregate.
    /// </summary>
    /// <param name="challengeSecret">
    /// Optional protected challenge secret.
    /// When omitted, a default protected secret is used.
    /// </param>
    /// <param name="challengeId">
    /// Optional challenge identifier.
    /// </param>
    /// <param name="userId">
    /// Optional user identifier.
    /// </param>
    /// <param name="challengeType">
    /// Authentication challenge type.
    /// </param>
    /// <param name="purpose">
    /// Authentication challenge purpose.
    /// </param>
    /// <param name="createdAtUtc">
    /// Optional creation timestamp.
    /// </param>
    /// <param name="expiresAtUtc">
    /// Optional expiration timestamp.
    /// </param>
    /// <returns>
    /// A valid <see cref="AuthenticationChallenge"/> aggregate.
    /// </returns>
    public static AuthenticationChallenge Create(
        ChallengeSecret? challengeSecret = null,
        Guid? challengeId = null,
        Guid? userId = null,
        AuthenticationChallengeType challengeType =
            AuthenticationChallengeType.EmailOtp,
        AuthenticationChallengePurpose purpose =
            AuthenticationChallengePurpose.Login,
        DateTime? createdAtUtc = null,
        DateTime? expiresAtUtc = null)
    {
        var created =
            createdAtUtc ?? DateTime.UtcNow;

        var expires =
            expiresAtUtc ?? created.AddMinutes(5);

        return AuthenticationChallenge.Create(
            challengeId ?? Guid.NewGuid(),
            userId ?? Guid.NewGuid(),
            challengeType,
            purpose,
            challengeSecret ?? DefaultSecret,
            created,
            expires);
    }

    /// <summary>
    /// Creates a challenge using the specified plaintext
    /// secret value.
    /// </summary>
    /// <param name="protectedSecret">
    /// Protected secret value.
    /// </param>
    /// <param name="challengeType">
    /// Authentication challenge type.
    /// </param>
    /// <param name="purpose">
    /// Authentication challenge purpose.
    /// </param>
    /// <returns>
    /// A valid <see cref="AuthenticationChallenge"/> aggregate.
    /// </returns>
    public static AuthenticationChallenge Create(
        string protectedSecret,
        AuthenticationChallengeType challengeType =
            AuthenticationChallengeType.EmailOtp,
        AuthenticationChallengePurpose purpose =
            AuthenticationChallengePurpose.Login)
    {
        return Create(
            new ChallengeSecret(protectedSecret),
            challengeType: challengeType,
            purpose: purpose);
    }
}