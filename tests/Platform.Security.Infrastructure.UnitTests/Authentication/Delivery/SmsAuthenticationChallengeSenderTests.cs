using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Security.Infrastructure.Authentication.Delivery;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Delivery;

/// <summary>
/// Unit tests for
/// <see cref="SmsAuthenticationChallengeSender"/>.
/// </summary>
public sealed class SmsAuthenticationChallengeSenderTests
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

    private static AuthenticationChallengeDeliveryRequest CreateRequest()
    {
        var challenge =
            AuthenticationChallenge.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                AuthenticationChallengeType.SmsOtp,
                AuthenticationChallengePurpose.Login,
                new ChallengeSecret("EncryptedSecret"),
                CreatedAtUtc,
                CreatedAtUtc.AddMinutes(5));

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
    /// Verifies constructor rejects null formatter.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        // Act
        Action act =
            () => new SmsAuthenticationChallengeSender(
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies SendAsync rejects null request.
    /// </summary>
    [Fact]
    public async Task SendAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var formatter =
            new Mock<IAuthenticationChallengeSmsFormatter>();

        var sut =
            new SmsAuthenticationChallengeSender(
                formatter.Object);

        // Act
        Func<Task> act =
            () => sut.SendAsync(null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>();

        formatter.Verify(
            x => x.Format(It.IsAny<AuthenticationChallengeDeliveryRequest>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies formatter is invoked before delivery.
    /// </summary>
    [Fact]
    public async Task SendAsync_ShouldInvokeFormatter()
    {
        // Arrange
        var request =
            CreateRequest();

        var message =
            new AuthenticationSmsMessage(
                "+628123456789",
                "Body");

        var formatter =
            new Mock<IAuthenticationChallengeSmsFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(message);

        var sut =
            new SmsAuthenticationChallengeSender(
                formatter.Object);

        // Act
        Func<Task> act =
            () => sut.SendAsync(request);

        // Assert
        await act.Should()
            .ThrowAsync<NotSupportedException>();

        formatter.Verify(
            x => x.Format(request),
            Times.Once);
    }

    /// <summary>
    /// Verifies transport exception is propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_ShouldThrowNotSupportedException()
    {
        // Arrange
        var request =
            CreateRequest();

        var formatter =
            new Mock<IAuthenticationChallengeSmsFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(
                new AuthenticationSmsMessage(
                    "+628123456789",
                    "Body"));

        var sut =
            new SmsAuthenticationChallengeSender(
                formatter.Object);

        // Act
        Func<Task> act =
            () => sut.SendAsync(request);

        // Assert
        await act.Should()
            .ThrowAsync<NotSupportedException>()
            .WithMessage(
                "No SMS communication provider has been configured.");
    }

    /// <summary>
    /// Verifies formatter exception is propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_ShouldPropagateFormatterException()
    {
        // Arrange
        var request =
            CreateRequest();

        var formatter =
            new Mock<IAuthenticationChallengeSmsFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Throws(
                new InvalidOperationException(
                    "Formatter failure."));

        var sut =
            new SmsAuthenticationChallengeSender(
                formatter.Object);

        // Act
        Func<Task> act =
            () => sut.SendAsync(request);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Formatter failure.");

        formatter.Verify(
            x => x.Format(request),
            Times.Once);
    }

    /// <summary>
    /// Verifies sending with a cancellation token follows the
    /// current production behavior.
    /// </summary>
    /// <remarks>
    /// The current implementation cannot observe the token because
    /// <c>SendCoreAsync</c> is a private static method that always
    /// throws <see cref="NotSupportedException"/>.
    /// </remarks>
    [Fact]
    public async Task SendAsync_ShouldThrowNotSupportedException_WithCancellationToken()
    {
        // Arrange
        var request =
            CreateRequest();

        var formatter =
            new Mock<IAuthenticationChallengeSmsFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(
                new AuthenticationSmsMessage(
                    "+628123456789",
                    "Body"));

        var sut =
            new SmsAuthenticationChallengeSender(
                formatter.Object);

        using var cts =
            new CancellationTokenSource();

        // Act
        Func<Task> act =
            () => sut.SendAsync(
                request,
                cts.Token);

        // Assert
        await act.Should()
            .ThrowAsync<NotSupportedException>();

        formatter.Verify(
            x => x.Format(request),
            Times.Once);
    }
}