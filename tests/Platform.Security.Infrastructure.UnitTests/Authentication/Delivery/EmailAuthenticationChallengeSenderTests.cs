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
/// <see cref="EmailAuthenticationChallengeSender"/>.
/// </summary>
public sealed class EmailAuthenticationChallengeSenderTests
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
                AuthenticationChallengeType.EmailOtp,
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
            () => new EmailAuthenticationChallengeSender(
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
            new Mock<IAuthenticationChallengeEmailFormatter>();

        var sut =
            new EmailAuthenticationChallengeSender(
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
            new AuthenticationEmailMessage(
                "john@example.com",
                "Subject",
                "Body",
                false);

        var formatter =
            new Mock<IAuthenticationChallengeEmailFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(message);

        var sut =
            new EmailAuthenticationChallengeSender(
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
            new Mock<IAuthenticationChallengeEmailFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(
                new AuthenticationEmailMessage(
                    "john@example.com",
                    "Subject",
                    "Body",
                    false));

        var sut =
            new EmailAuthenticationChallengeSender(
                formatter.Object);

        // Act
        Func<Task> act =
            () => sut.SendAsync(request);

        // Assert
        await act.Should()
            .ThrowAsync<NotSupportedException>()
            .WithMessage(
                "No email communication provider has been configured.");
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
            new Mock<IAuthenticationChallengeEmailFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Throws(
                new InvalidOperationException(
                    "Formatter failure."));

        var sut =
            new EmailAuthenticationChallengeSender(
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
    /// Verifies cancellation token reaches the delivery path.
    /// </summary>
    /// <remarks>
    /// The current implementation cannot observe the token because
    /// <c>SendCoreAsync</c> is a private static method that always
    /// throws <see cref="NotSupportedException"/>.
    /// This test documents the current production behavior.
    /// </remarks>
    [Fact]
    public async Task SendAsync_ShouldThrowNotSupportedException_WithCancellationToken()
    {
        // Arrange
        var request =
            CreateRequest();

        var formatter =
            new Mock<IAuthenticationChallengeEmailFormatter>();

        formatter
            .Setup(x => x.Format(request))
            .Returns(
                new AuthenticationEmailMessage(
                    "john@example.com",
                    "Subject",
                    "Body",
                    false));

        var sut =
            new EmailAuthenticationChallengeSender(
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