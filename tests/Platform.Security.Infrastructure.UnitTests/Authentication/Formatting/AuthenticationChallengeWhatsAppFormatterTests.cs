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
/// <see cref="AuthenticationChallengeWhatsAppFormatter"/>.
/// </summary>
public sealed class AuthenticationChallengeWhatsAppFormatterTests
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
            VerificationCodeWhatsAppPrefix =
                "Your verification code is",
            IgnoreMessage =
                "Ignore this message."
        };
    }

    private static AuthenticationChallengeDeliveryRequest CreateRequest(
        TimeSpan lifetime)
    {
        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.WhatsAppOtp,
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
        // Act
        Action act =
            () => new AuthenticationChallengeWhatsAppFormatter(
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies Format rejects null request.
    /// </summary>
    [Fact]
    public void Format_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                CreateOptions());

        // Act
        Action act =
            () => sut.Format(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies recipient phone number is used.
    /// </summary>
    [Fact]
    public void Format_ShouldUseRecipientPhoneNumber()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Recipient
            .Should()
            .Be("+628123456789");
    }

    /// <summary>
    /// Verifies configured prefix is included.
    /// </summary>
    [Fact]
    public void Format_ShouldContainConfiguredPrefix()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .StartWith(
                options.VerificationCodeWhatsAppPrefix);
    }

    /// <summary>
    /// Verifies verification code is included.
    /// </summary>
    [Fact]
    public void Format_ShouldContainVerificationCode()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain("123456");
    }

    /// <summary>
    /// Verifies ignore message is included.
    /// </summary>
    [Fact]
    public void Format_ShouldContainIgnoreMessage()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain(options.IgnoreMessage);
    }

    /// <summary>
    /// Verifies expiration minutes are rounded upward.
    /// </summary>
    [Fact]
    public void Format_ShouldRoundExpirationMinutesUp()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(61));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
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
        // Arrange
        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(1));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain("1 minute(s)");
    }

    /// <summary>
    /// Verifies generated WhatsApp message matches
    /// expected format.
    /// </summary>
    [Fact]
    public void Format_ShouldGenerateExpectedMessage()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeWhatsAppFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationWhatsAppMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Be(
                "Your verification code is 123456\n\n" +
                "This code expires in 5 minute(s).\n\n" +
                "Ignore this message.");
    }
}