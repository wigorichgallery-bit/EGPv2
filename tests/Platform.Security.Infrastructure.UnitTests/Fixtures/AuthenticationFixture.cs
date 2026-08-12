using Platform.Identity.Application.Configuration.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Security.Infrastructure.UnitTests.Fixtures;

/// <summary>
/// Provides reusable authentication test objects.
/// </summary>
public static class AuthenticationFixture
{
    /// <summary>
    /// Fixed timestamp used by authentication tests.
    /// </summary>
    public static readonly DateTime CreatedAtUtc =
        new(
            2026,
            1,
            1,
            12,
            0,
            0,
            DateTimeKind.Utc);

    /// <summary>
    /// Creates default authentication message options.
    /// </summary>
    public static AuthenticationMessageOptions CreateOptions()
    {
        return new AuthenticationMessageOptions
        {
            ApplicationName = "EGPv2",
            VerificationCodeEmailSubject = "Verification Code",
            VerificationCodeSmsPrefix = "Your verification code is",
            VerificationCodeWhatsAppPrefix = "Your verification code is",
            IgnoreMessage = "Ignore this message."
        };
    }

    /// <summary>
    /// Creates a valid authentication delivery request.
    /// </summary>
    public static AuthenticationChallengeDeliveryRequest CreateDeliveryRequest(
        AuthenticationChallengeType challengeType,
        TimeSpan lifetime)
    {
        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                challengeType,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("EncryptedSecret"),
                CreatedAtUtc,
                CreatedAtUtc.Add(lifetime));

        var user =
            new UserAccount(
                Guid.NewGuid(),
                "john",
                new EmailAddress("john@example.com"),
                new PhoneNumber("+628123456789"),
                "HASH",
                CreatedAtUtc);

        return new AuthenticationChallengeDeliveryRequest(
            challenge,
            user,
            "123456");
    }
}