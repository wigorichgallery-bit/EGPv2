
using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;

namespace Platform.Communication.UnitTests.Channels.Email.Providers;

/// <summary>
/// Contains unit tests for
/// <see cref="MicrosoftGraphEmailProvider"/>.
/// </summary>
public sealed class MicrosoftGraphEmailProviderTests
{
    private readonly IGraphClient _client;

    private readonly ILogger<MicrosoftGraphEmailProvider> _logger;

    public MicrosoftGraphEmailProviderTests()
    {
        _client =
            Substitute.For<IGraphClient>();

        _logger =
            Substitute.For<
                ILogger<MicrosoftGraphEmailProvider>>();
    }

    // ==========================================================
    // Constructor
    // ==========================================================

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Act

        Action action =
            () =>
                _ = new MicrosoftGraphEmailProvider(
                    null!,
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Act

        Action action =
            () =>
                _ = new MicrosoftGraphEmailProvider(
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

    [Fact]
    public async Task SendAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
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

    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_CancellationRequested()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
            CreateSut();

        EmailMessage message =
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
            .SendEmailAsync(
                Arg.Any<EmailMessage>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // SendAsync - Success
    // ==========================================================

    [Fact]
    public async Task SendAsync_Should_ReturnSuccess_When_ClientSucceeds()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
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

        await _client
            .Received(1)
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdIsEmpty()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
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

    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdIsWhitespace()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    messageId: "   "));

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
    // SendAsync - Cancellation
    // ==========================================================

    [Fact]
    public async Task SendAsync_Should_RethrowOperationCanceledException_When_ClientCancels()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
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

    [Fact]
    public async Task SendAsync_Should_ForwardCancellationToken()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        CancellationTokenSource cancellationTokenSource =
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
    // SendAsync - CommunicationException
    // ==========================================================

    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsCommunicationException()
    {
        // Arrange

        MicrosoftGraphEmailProvider provider =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        CommunicationException exception =
            new(
                "Graph failure.");

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
                "Graph failure.");
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    private MicrosoftGraphEmailProvider CreateSut()
    {
        return new MicrosoftGraphEmailProvider(
            _client,
            _logger);
    }

    private static EmailMessage CreateMessage()
    {
        return new EmailMessage(
            [
                new Platform.Communication.ValueObjects.EmailAddress(
                    "user@example.com")
            ],
            "Subject",
            "Body");
    }
}
