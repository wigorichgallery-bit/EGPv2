using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.UnitTests.Fixtures;

/// <summary>
/// Provides reusable <see cref="ChallengeSecret"/> instances
/// for application unit tests.
/// </summary>
public static class ChallengeSecretFixture
{
    /// <summary>
    /// Gets the default protected challenge secret.
    /// </summary>
    public const string DefaultValue =
        "HASHED_OR_ENCRYPTED_SECRET";

    /// <summary>
    /// Creates a valid <see cref="ChallengeSecret"/>.
    /// </summary>
    /// <param name="value">
    /// Optional protected secret value.
    /// </param>
    /// <returns>
    /// A valid <see cref="ChallengeSecret"/>.
    /// </returns>
    public static ChallengeSecret Create(
        string value = DefaultValue)
    {
        return new ChallengeSecret(value);
    }
}