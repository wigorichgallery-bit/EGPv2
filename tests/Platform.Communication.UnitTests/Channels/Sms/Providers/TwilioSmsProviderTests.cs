using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Sms.Clients;
using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Sms.Providers;

/// <summary>
/// Contains unit tests for <see cref="TwilioSmsProvider"/>.
/// </summary>
public sealed class TwilioSmsProviderTests
{
    private readonly ITwilioSmsClient _client;

    private readonly ILogger<TwilioSmsProvider> _logger;

    public TwilioSmsProviderTests()
    {
        _client = Substitute.For<ITwilioSmsClient>();
        _logger = Substitute.For<ILogger<TwilioSmsProvider>>();
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the client is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new TwilioSmsProvider(
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
        // Arrange / Act
        Action action = () =>
            _ = new TwilioSmsProvider(
                _client,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    /// <summary>
    /// Verifies that SendAsync throws an
    /// <see cref="ArgumentNullException"/>
    /// when the message is null.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange
        TwilioSmsProvider provider = new(
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
    /// Verifies that SendAsync throws an
    /// <see cref="OperationCanceledException"/>
    /// when cancellation has already been requested.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        CancellationToken cancellationToken = new(canceled: true);

        // Act
        Func<Task> action = () =>
            provider.SendAsync(
                SmsMessageTestData.CreateValid(),
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
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        SmsMessage message = SmsMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
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
    /// Verifies that SendAsync returns a failed
    /// delivery result when the provider does not
    /// return a message identifier.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdIsEmpty()
    {
        // Arrange
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        SmsMessage message = SmsMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(string.Empty));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should()
            .Be("The provider did not return a message identifier.");
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
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        SmsMessage message = SmsMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(
                _ => throw new OperationCanceledException());

        // Act
        Func<Task> action = () =>
            provider.SendAsync(message);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies that SendAsync returns a failed
    /// delivery result when the client throws an exception.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsException()
    {
        // Arrange
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        SmsMessage message = SmsMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(
                _ => throw new InvalidOperationException("Twilio failure."));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be("Twilio failure.");
    }

    /// <summary>
    /// Verifies that SendAsync sends a message
    /// to every recipient.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_InvokeClientForEachRecipient_When_MultipleRecipientsExist()
    {
        // Arrange
        TwilioSmsProvider provider = new(
            _client,
            _logger);

        SmsMessage message = SmsMessageTestData.CreateMultipleRecipients();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(VendorDeliveryResult.Success("MSG-001"));

        // Act
        await provider.SendAsync(message);

        // Assert
        await _client
            .Received(message.To.Count)
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());
    }
}