using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Sms.Clients;
using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Sms.Providers;

/// <summary>
/// Contains unit tests for
/// <see cref="VonageSmsProvider"/>.
/// </summary>
public sealed class VonageSmsProviderTests
{
    private readonly IVonageSmsClient _client;

    private readonly ILogger<VonageSmsProvider> _logger;

    public VonageSmsProviderTests()
    {
        _client =
            Substitute.For<IVonageSmsClient>();

        _logger =
            Substitute.For<
                ILogger<VonageSmsProvider>>();
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
                _ = new VonageSmsProvider(
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
                _ = new VonageSmsProvider(
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

        VonageSmsProvider provider =
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

    // /// <summary>
    // /// Verifies that SendAsync returns a failed
    // /// delivery result when no recipients exist.
    // /// </summary>
    // [Fact]
    // public async Task SendAsync_Should_ReturnFailure_When_NoRecipientsExist()
    // {
    //     // Arrange

    //     VonageSmsProvider provider =
    //         CreateSut();

    //     SmsMessage message =
    //         CreateMessageWithoutRecipients();

    //     // Act

    //     DeliveryResult result =
    //         await provider.SendAsync(
    //             message);

    //     // Assert

    //     result.Succeeded
    //         .Should()
    //         .BeFalse();

    //     result.ErrorMessage
    //         .Should()
    //         .Be(
    //             "No recipient was specified.");

    //     await _client
    //         .DidNotReceive()
    //         .SendMessageAsync(
    //             Arg.Any<string>(),
    //             Arg.Any<string>(),
    //             Arg.Any<CancellationToken>());
    // }

    /// <summary>
    /// Verifies that SendAsync throws an
    /// <see cref="OperationCanceledException"/>
    /// when cancellation has already been requested.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        CancellationToken cancellationToken =
            new(canceled: true);

        // Act

        Func<Task> action =
            () =>
                provider.SendAsync(
                    message,
                    cancellationToken);

        // Assert

        await action.Should()
            .ThrowAsync<OperationCanceledException>();

        await _client
            .DidNotReceive()
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
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

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
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
    /// Verifies that SendAsync returns a failed
    /// delivery result when the client does not
    /// return a message identifier.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdIsEmpty()
    {
        // Arrange

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: string.Empty));

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
                "The provider did not return a message identifier.");
    }

    // ==========================================================
    // SendAsync - Multiple Recipients
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync sends the message
    /// to every recipient.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_SendMessageToEveryRecipient()
    {
        // Arrange

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage(
                "+628123456789",
                "+628987654321");

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                message.Message,
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

        await _client
            .Received(2)
            .SendMessageAsync(
                Arg.Any<string>(),
                message.Message,
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that SendAsync returns the
    /// message identifier from the last recipient.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnLastMessageId_When_MultipleRecipientsExist()
    {
        // Arrange

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage(
                "+628123456789",
                "+628987654321");

        _client
            .SendMessageAsync(
                "+628123456789",
                message.Message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: "MSG-001"));

        _client
            .SendMessageAsync(
                "+628987654321",
                message.Message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: "MSG-002"));

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
            .Be("MSG-002");
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

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
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

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        CommunicationException exception =
            new(
                "Vonage failed.");

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
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
                "Vonage failed.");
    }

    // ==========================================================
    // SendAsync - CancellationToken
    // ==========================================================

    /// <summary>
    /// Verifies that SendAsync forwards
    /// the cancellation token to the client.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ForwardCancellationToken()
    {
        // Arrange

        VonageSmsProvider provider =
            CreateSut();

        SmsMessage message =
            CreateMessage();

        using CancellationTokenSource cancellationTokenSource =
            new();

        CancellationToken cancellationToken =
            cancellationTokenSource.Token;

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                message.Message,
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
            .SendMessageAsync(
                Arg.Any<string>(),
                message.Message,
                cancellationToken);
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private VonageSmsProvider CreateSut()
    {
        return new VonageSmsProvider(
            _client,
            _logger);
    }

    /// <summary>
    /// Creates a valid SMS message.
    /// </summary>
    private static SmsMessage CreateMessage()
    {
        return CreateMessage(
            "+628123456789");
    }

    /// <summary>
    /// Creates an SMS message with
    /// the specified recipients.
    /// </summary>
    private static SmsMessage CreateMessage(
        params string[] recipients)
    {
        return new SmsMessage(
            [
                .. recipients.Select(
                    recipient =>
                        new PhoneNumber(
                            recipient))
            ],
            "Test SMS message.");
    }

    // /// <summary>
    // /// Creates an SMS message without recipients.
    // /// </summary>
    // private static SmsMessage CreateMessageWithoutRecipients()
    // {
    //     return new SmsMessage(
    //         [],
    //         "Test SMS message.");
    // }
}