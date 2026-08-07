using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Security.Infrastructure.UnitTests.Fixtures;

/// <summary>
/// Provides reusable password-related objects for unit tests.
/// </summary>
public static class PasswordFixture
{
    /// <summary>
    /// Default password used by password-related tests.
    /// </summary>
    public const string ValidPassword =
        "P@ssw0rd123!";

    /// <summary>
    /// Alternate password used by verification tests.
    /// </summary>
    public const string InvalidPassword =
        "WrongPassword123!";

    /// <summary>
    /// Creates a user account with a configurable password hash.
    /// </summary>
    /// <param name="passwordHash">
    /// Password hash assigned to the user.
    /// </param>
    /// <returns>
    /// A valid <see cref="UserAccount"/> instance.
    /// </returns>
    public static UserAccount CreateUser(
        string passwordHash = "HASH")
    {
        return new UserAccount(
            Guid.NewGuid(),
            "john",
            new EmailAddress("john@example.com"),
            new PhoneNumber("+628123456789"),
            passwordHash,
            AuthenticationFixture.CreatedAtUtc);
    }
}