using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Email.Providers;

/// <summary>
/// Contains unit tests for <see cref="SmtpEmailProvider"/>.
/// </summary>
public sealed class SmtpEmailProviderTests
{
    private readonly ISmtpClient _client;

    private readonly ILogger<SmtpEmailProvider> _logger;

    public SmtpEmailProviderTests()
    {
        _client = Substitute.For<ISmtpClient>();
        _logger = Substitute.For<ILogger<SmtpEmailProvider>>();
    }

    /// <summary>
    /// Verifies that the constructor throws when
    /// the SMTP client is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new SmtpEmailProvider(
                null!,
                _logger);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    /// <summary>
    /// Verifies that the constructor throws when
    /// the logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new SmtpEmailProvider(
                _client,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    /// <summary>
    /// Verifies that SendAsync throws when
    /// the message is null.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange
        SmtpEmailProvider provider = new(
            _client,
            _logger);

        // Act
        Func<Task> action = () =>
            provider.SendAsync(null!);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("message");
    }

    /// <summary>
    /// Verifies that SendAsync throws when
    /// the cancellation token is already cancelled.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange
        SmtpEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        CancellationToken cancellationToken =
            new(canceled: true);

        // Act
        Func<Task> action = () =>
            provider.SendAsync(
                message,
                cancellationToken);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies that SendAsync returns a successful
    /// delivery result when the SMTP client succeeds.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnSuccess_When_ClientSucceeds()
    {
        // Arrange
        SmtpEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    "MSG-001"));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.ProviderMessageId.Should().Be("MSG-001");
        result.ErrorMessage.Should().BeNull();
    }

    /// <summary>
    /// Verifies that SendAsync rethrows an
    /// <see cref="OperationCanceledException"/>
    /// thrown by the SMTP client.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_RethrowOperationCanceledException_When_ClientCancels()
    {
        // Arrange
        SmtpEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(_ =>
                throw new OperationCanceledException());

        // Act
        Func<Task> action = () =>
            provider.SendAsync(message);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies that SendAsync returns
    /// a failed delivery result when
    /// the SMTP client throws an exception.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsException()
    {
        // Arrange
        SmtpEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(_ =>
                throw new InvalidOperationException("SMTP failed."));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be("SMTP failed.");
    }

    private static EmailMessage CreateMessage()
    {
        return new EmailMessage(
        [
            new EmailAddress("user@example.com")
        ],
        "Subject",
        "Body");
    }
}