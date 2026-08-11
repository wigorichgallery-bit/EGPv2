using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Email.Providers;

/// <summary>
/// Contains unit tests for
/// <see cref="SendGridEmailProvider"/>.
/// </summary>
public sealed class SendGridEmailProviderTests
{
    private readonly ISendGridClient _client;

    private readonly ILogger<SendGridEmailProvider> _logger;

    public SendGridEmailProviderTests()
    {
        _client =
            Substitute.For<ISendGridClient>();

        _logger =
            Substitute.For<
                ILogger<SendGridEmailProvider>>();
    }

    // ==========================================================
    // Constructor
    // ==========================================================

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the client is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Act

        Action action =
            () =>
                _ = new SendGridEmailProvider(
                    null!,
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Act

        Action action =
            () =>
                _ = new SendGridEmailProvider(
                    _client,
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    // ==========================================================
    // SendAsync - Validation
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync throws an
    /// <see cref="ArgumentNullException"/>
    /// when the message is null.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        // Act

        Func<Task> action =
            () =>
                provider.SendAsync(
                    null!);

        // Assert

        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("message");
    }

    /// <summary>
    /// Verifies that SendAsync throws an
    /// <see cref="OperationCanceledException"/>
    /// when cancellation has already been requested.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        CancellationToken cancellationToken =
            new(canceled: true);

        // Act

        Func<Task> action =
            () =>
                provider.SendAsync(
                    CreateMessage(),
                    cancellationToken);

        // Assert

        await action.Should()
            .ThrowAsync<OperationCanceledException>();

        await _client
            .DidNotReceive()
            .SendEmailAsync(
                Arg.Any<EmailMessage>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // SendAsync - Success
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync returns a successful
    /// delivery result when the client succeeds.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnSuccess_When_ClientSucceeds()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: "MSG-001"));

        // Act

        DeliveryResult result =
            await provider.SendAsync(
                message);

        // Assert

        result.Succeeded
            .Should()
            .BeTrue();

        result.ProviderMessageId
            .Should()
            .Be("MSG-001");

        result.ErrorMessage
            .Should()
            .BeNull();
    }

    /// <summary>
    /// Verifies that SendAsync forwards
    /// the cancellation token to the client.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ForwardCancellationToken()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        using CancellationTokenSource cancellationTokenSource =
            new();

        CancellationToken cancellationToken =
            cancellationTokenSource.Token;

        _client
            .SendEmailAsync(
                message,
                cancellationToken)
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: "MSG-001"));

        // Act

        DeliveryResult result =
            await provider.SendAsync(
                message,
                cancellationToken);

        // Assert

        result.Succeeded
            .Should()
            .BeTrue();

        await _client
            .Received(1)
            .SendEmailAsync(
                message,
                cancellationToken);
    }

    // ==========================================================
    // SendAsync - Cancellation
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync rethrows an
    /// <see cref="OperationCanceledException"/>
    /// thrown by the client.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_RethrowOperationCanceledException_When_ClientCancels()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                Arg.Any<EmailMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<VendorDeliveryResult>(
                    new OperationCanceledException()));

        // Act

        Func<Task> action =
            () =>
                provider.SendAsync(
                    message);

        // Assert

        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    // ==========================================================
    // SendAsync - CommunicationException
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync returns a failed
    /// delivery result when the client throws
    /// a <see cref="CommunicationException"/>.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsCommunicationException()
    {
        // Arrange

        SendGridEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        CommunicationException exception =
            new(
                "SendGrid failed.");

        _client
            .SendEmailAsync(
                Arg.Any<EmailMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<VendorDeliveryResult>(
                    exception));

        // Act

        DeliveryResult result =
            await provider.SendAsync(
                message);

        // Assert

        result.Succeeded
            .Should()
            .BeFalse();

        result.ErrorMessage
            .Should()
            .Be(
                "SendGrid failed.");
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private SendGridEmailProvider CreateSut()
    {
        return new SendGridEmailProvider(
            _client,
            _logger);
    }

    /// <summary>
    /// Creates a valid email message for testing.
    /// </summary>
    private static EmailMessage CreateMessage()
    {
        return new EmailMessage(
            [
                new EmailAddress(
                    "user@example.com")
            ],
            "Subject",
            "Body");
    }
}