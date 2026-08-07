using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Email.Providers;

/// <summary>
/// Contains unit tests for <see cref="MicrosoftGraphEmailProvider"/>.
/// </summary>
public sealed class MicrosoftGraphEmailProviderTests
{
    private readonly IGraphClient _client;

    private readonly ILogger<MicrosoftGraphEmailProvider> _logger;

    public MicrosoftGraphEmailProviderTests()
    {
        _client = Substitute.For<IGraphClient>();
        _logger = Substitute.For<ILogger<MicrosoftGraphEmailProvider>>();
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
            _ = new MicrosoftGraphEmailProvider(
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
            _ = new MicrosoftGraphEmailProvider(
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
        MicrosoftGraphEmailProvider provider = new(
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
        MicrosoftGraphEmailProvider provider = new(
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
        MicrosoftGraphEmailProvider provider = new(
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
    /// Verifies that SendAsync returns a failed
    /// delivery result when the provider does not
    /// return a message identifier.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdIsEmpty()
    {
        // Arrange
        MicrosoftGraphEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
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
        MicrosoftGraphEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
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
    /// delivery result when the client throws
    /// an exception.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsException()
    {
        // Arrange
        MicrosoftGraphEmailProvider provider = new(
            _client,
            _logger);

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _client
            .SendEmailAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns<Task<VendorDeliveryResult>>(
                _ => throw new InvalidOperationException("Graph failure."));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be("Graph failure.");
    }

    /// <summary>
    /// Creates a valid email message for testing.
    /// </summary>
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