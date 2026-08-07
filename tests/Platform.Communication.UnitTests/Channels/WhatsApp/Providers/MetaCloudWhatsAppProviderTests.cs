using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.WhatsApp.Clients;
using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Providers;

/// <summary>
/// Contains unit tests for
/// <see cref="MetaCloudWhatsAppProvider"/>.
/// </summary>
public sealed class MetaCloudWhatsAppProviderTests
{
    private readonly IMetaCloudClient _client;

    private readonly ILogger<MetaCloudWhatsAppProvider> _logger;

    public MetaCloudWhatsAppProviderTests()
    {
        _client =
            Substitute.For<IMetaCloudClient>();

        _logger =
            Substitute.For<
                ILogger<MetaCloudWhatsAppProvider>>();
    }

    /// <summary>
    /// Verifies constructor throws when
    /// client is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ClientIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new MetaCloudWhatsAppProvider(
                null!,
                _logger);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    /// <summary>
    /// Verifies constructor throws when
    /// logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new MetaCloudWhatsAppProvider(
                _client,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    /// <summary>
    /// Verifies constructor creates instance.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateInstance_When_DependenciesValid()
    {
        // Arrange / Act
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        // Assert
        provider.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies SendAsync throws when
    /// message is null.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        // Act
        Func<Task> action =
            () => provider.SendAsync(null!);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies successful delivery.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnSuccess_When_ClientSucceeds()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    "MSG-001"));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeTrue();

        result.ProviderMessageId
            .Should()
            .Be("MSG-001");
    }

    /// <summary>
    /// Verifies multiple recipients
    /// invoke client once per recipient.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_InvokeClientForEachRecipient_When_MultipleRecipients()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData
                .CreateMultipleRecipients();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    "MSG"));

        // Act
        await provider.SendAsync(message);

        // Assert
        await _client
            .Received(2)
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies failure is returned when
    /// provider does not return a message identifier.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_MessageIdMissing()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success(
                    null));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();

        result.ErrorMessage
            .Should()
            .Be(
                "The provider did not return a message identifier.");
    }

    /// <summary>
    /// Verifies provider exceptions
    /// become failed delivery results.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnFailure_When_ClientThrowsException()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        _client
            .When(x =>
                x.SendMessageAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<CancellationToken>()))
            .Do(_ =>
            {
                throw new InvalidOperationException(
                    "Failure");
            });

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.Succeeded.Should().BeFalse();

        result.ErrorMessage
            .Should()
            .Be("Failure");
    }

    /// <summary>
    /// Verifies cancellation
    /// is propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_Cancelled()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        CancellationToken cancellationToken =
            new(canceled: true);

        // Act
        Func<Task> action =
            () => provider.SendAsync(
                message,
                cancellationToken);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies cancellation from client
    /// is propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ThrowOperationCanceledException_When_ClientCancels()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        _client
            .When(x =>
                x.SendMessageAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<CancellationToken>()))
            .Do(_ => throw new OperationCanceledException());

        // Act
        Func<Task> action =
            () => provider.SendAsync(message);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies last recipient message identifier
    /// is returned.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnLastMessageId_When_MultipleRecipients()
    {
        // Arrange
        MetaCloudWhatsAppProvider provider =
            new(
                _client,
                _logger);

        WhatsAppMessage message =
            WhatsAppMessageTestData
                .CreateMultipleRecipients();

        _client
            .SendMessageAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(
                VendorDeliveryResult.Success("MSG-1"),
                VendorDeliveryResult.Success("MSG-2"));

        // Act
        DeliveryResult result =
            await provider.SendAsync(message);

        // Assert
        result.ProviderMessageId
            .Should()
            .Be("MSG-2");
    }
}