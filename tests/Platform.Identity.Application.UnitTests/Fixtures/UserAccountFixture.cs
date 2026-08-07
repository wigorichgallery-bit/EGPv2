using Platform.Identity.Application.UnitTests.Fixtures.Builders;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.UnitTests.Fixtures;

/// <summary>
/// Provides reusable <see cref="UserAccount"/> instances
/// for application unit tests.
/// </summary>
public static class UserAccountFixture
{
    /// <summary>
    /// Creates a default user.
    /// </summary>
    public static UserAccount Create()
    {
        return UserAccountBuilder.Default.Build();
    }

    /// <summary>
    /// Creates a user with a verified email address.
    /// </summary>
    public static UserAccount CreateEmailVerified()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyEmail(now);

        return user;
    }

    /// <summary>
    /// Creates a user with a verified phone number.
    /// </summary>
    public static UserAccount CreatePhoneVerified()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyPhone(now);

        return user;
    }

    /// <summary>
    /// Creates a user with verified email and phone number.
    /// </summary>
    public static UserAccount CreateFullyVerified()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyEmail(now);
        user.VerifyPhone(now);

        return user;
    }

    /// <summary>
    /// Creates a user with TOTP multi-factor authentication enabled.
    /// </summary>
    public static UserAccount CreateTotpUser()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyEmail(now);

        user.SetTotpSecret(
            "ENCRYPTED_TOTP_SECRET",
            now);

        user.EnableMFA(
            MFAMethod.TOTP,
            now);

        return user;
    }

    /// <summary>
    /// Creates a user with email-based multi-factor authentication enabled.
    /// </summary>
    public static UserAccount CreateEmailMfaUser()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyEmail(now);

        user.EnableMFA(
            MFAMethod.Email,
            now);

        return user;
    }

    /// <summary>
    /// Creates a user with SMS-based multi-factor authentication enabled.
    /// </summary>
    public static UserAccount CreateSmsMfaUser()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyPhone(now);

        user.EnableMFA(
            MFAMethod.SMS,
            now);

        return user;
    }

    /// <summary>
    /// Creates a user with WhatsApp-based multi-factor authentication enabled.
    /// </summary>
    public static UserAccount CreateWhatsAppMfaUser()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.VerifyPhone(now);

        user.EnableMFA(
            MFAMethod.WhatsApp,
            now);

        return user;
    }

    public static UserAccount CreateLocked()
    {
        var now = DateTime.UtcNow;

        var user = Create();

        user.RegisterFailedLoginAttempt(
            1,
            TimeSpan.FromMinutes(30),
            now);

        return user;
    }
}