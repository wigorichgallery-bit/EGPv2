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
/// <see cref="AuthenticationChallengeEmailFormatter"/>.
/// </summary>
public sealed class AuthenticationChallengeEmailFormatterTests
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
            ApplicationName = "EGPv2",
            VerificationCodeEmailSubject = "Verification Code",
            IgnoreMessage = "Ignore this message."
        };
    }

    private static AuthenticationChallengeDeliveryRequest CreateRequest(
        TimeSpan lifetime)
    {
        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.EmailOtp,
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
        Action act = () =>
            new AuthenticationChallengeEmailFormatter(
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
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        // Act
        Action act = () =>
            sut.Format(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies recipient is user's email.
    /// </summary>
    [Fact]
    public void Format_ShouldUseUserEmailAsRecipient()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.Recipient
            .Should()
            .Be("john@example.com");
    }

    /// <summary>
    /// Verifies configured subject is used.
    /// </summary>
    [Fact]
    public void Format_ShouldUseConfiguredSubject()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeEmailFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.Subject
            .Should()
            .Be(options.VerificationCodeEmailSubject);
    }

    /// <summary>
    /// Verifies email body contains application name.
    /// </summary>
    [Fact]
    public void Format_ShouldContainApplicationName()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeEmailFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain(options.ApplicationName);
    }

    /// <summary>
    /// Verifies email body contains verification code.
    /// </summary>
    [Fact]
    public void Format_ShouldContainPlainTextSecret()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain("123456");
    }

    /// <summary>
    /// Verifies email body contains ignore message.
    /// </summary>
    [Fact]
    public void Format_ShouldContainIgnoreMessage()
    {
        // Arrange
        var options =
            CreateOptions();

        var sut =
            new AuthenticationChallengeEmailFormatter(
                options);

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
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
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(61));

        // Act
        AuthenticationEmailMessage result =
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
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromSeconds(1));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.Body
            .Should()
            .Contain("1 minute(s)");
    }

    /// <summary>
    /// Verifies generated email is plain text.
    /// </summary>
    [Fact]
    public void Format_ShouldReturnPlainTextMessage()
    {
        // Arrange
        var sut =
            new AuthenticationChallengeEmailFormatter(
                CreateOptions());

        var request =
            CreateRequest(
                TimeSpan.FromMinutes(5));

        // Act
        AuthenticationEmailMessage result =
            sut.Format(request);

        // Assert
        result.IsHtml
            .Should()
            .BeFalse();
    }
}