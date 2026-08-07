using FluentAssertions;
using Platform.Identity.Application.Configuration;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Security.Infrastructure.Authentication.Formatting;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Formatting;

/// <summary>
/// Unit tests for
/// <see cref="AuthenticationChallengeSmsFormatter"/>.
/// </summary>
public sealed class AuthenticationChallengeSmsFormatterTests
{
    private static readonly DateTime CreatedAtUtc =
        new(
            2026,
            1,
            1,
            12,
            0,
            0,
            DateTimeKind.Utc);

    private static AuthenticationMessageOptions CreateOptions()
    {
        return new AuthenticationMessageOptions
        {
            VerificationCodeSmsPrefix = "Your verification code is"
        };
    }

    private static AuthenticationChallengeDeliveryRequest CreateRequest(
        TimeSpan lifetime)
    {
        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.SmsOtp,
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

    /// <summary>
    /// Verifies constructor rejects null options.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        Action act =
            () => new AuthenticationChallengeSmsFormatter(null!);

        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies Format rejects null request.
    /// </summary>
    [Fact]
    public void Format_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        var sut =
            new AuthenticationChallengeSmsFormatter(
                CreateOptions());

        Action act =
            () => sut.Format(null!);

        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies recipient phone number is used.
    /// </summary>
    [Fact]
    public void Format_ShouldUseRecipientPhoneNumber()
    {
        var sut =
            new AuthenticationChallengeSmsFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Recipient
            .Should()
            .Be("+628123456789");
    }

    /// <summary>
    /// Verifies SMS prefix is included.
    /// </summary>
    [Fact]
    public void Format_ShouldContainConfiguredPrefix()
    {
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeSmsFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Body
            .Should()
            .StartWith(options.VerificationCodeSmsPrefix);
    }

    /// <summary>
    /// Verifies verification code is included.
    /// </summary>
    [Fact]
    public void Format_ShouldContainVerificationCode()
    {
        var sut =
            new AuthenticationChallengeSmsFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Body
            .Should()
            .Contain("123456");
    }

    /// <summary>
    /// Verifies expiration minutes are rounded upward.
    /// </summary>
    [Fact]
    public void Format_ShouldRoundExpirationMinutesUp()
    {
        var sut =
            new AuthenticationChallengeSmsFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(61));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Body
            .Should()
            .Contain("2 minute(s)");
    }

    /// <summary>
    /// Verifies minimum expiration is one minute.
    /// </summary>
    [Fact]
    public void Format_ShouldUseMinimumOneMinute()
    {
        var sut =
            new AuthenticationChallengeSmsFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(1));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Body
            .Should()
            .Contain("1 minute(s)");
    }

    /// <summary>
    /// Verifies generated body matches expected format.
    /// </summary>
    [Fact]
    public void Format_ShouldGenerateExpectedMessage()
    {
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeSmsFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        AuthenticationSmsMessage result =
            sut.Format(request);

        result.Body
            .Should()
            .Be(
                "Your verification code is 123456. Expires in 5 minute(s).");
    }
}