using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Email.Providers;

/// <summary>
/// Contains unit tests for <see cref="SendGridEmailProvider"/>.
/// </summary>
public sealed class SendGridEmailProviderTests
{
    private readonly ISendGridClient _client;

    private readonly ILogger<SendGridEmailProvider> _logger;

    public SendGridEmailProviderTests()
    {
        _client = Substitute.For<ISendGridClient>();
        _logger = Substitute.For<ILogger<SendGridEmailProvider>>();
    }

    /// <summary>
    /// Verifies that the constructor throws when
    /// the SendGrid client is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new SendGridEmailProvider(
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
            _ = new SendGridEmailProvider(
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
        SendGridEmailProvider provider = new(
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
    /// cancellation has been requested.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange
        SendGridEmailProvider provider = new(
            _client,
            _logger);

        CancellationToken cancellationToken =
            new(canceled: true);

        // Act
        Func<Task> action = () =>
            provider.SendAsync(
                CreateMessage(),
                cancellationToken);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies that SendAsync returns a successful
    /// delivery result when the client succeeds.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnSuccess_When_ClientSucceeds()
    {
        // Arrange
        SendGridEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success("MSG-001"));

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
    /// thrown by the client.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_RethrowOperationCanceledException_When_ClientCancels()
    {
        // Arrange
        SendGridEmailProvider provider = new(
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
    /// the client throws an exception.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsException()
    {
        // Arrange
        SendGridEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(_ =>
                throw new InvalidOperationException("SendGrid failed."));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be("SendGrid failed.");
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