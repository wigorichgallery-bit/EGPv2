using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.WhatsApp.Providers;
using Platform.Communication.Channels.WhatsApp.Sender;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;

namespace Platform.Communication.UnitTests.Channels.WhatsApp.Sender;

/// <summary>
/// Contains unit tests for <see cref="WhatsAppSender"/>.
/// </summary>
public sealed class WhatsAppSenderTests
{
    private readonly IWhatsAppProvider _provider;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="WhatsAppSenderTests"/> class.
    /// </summary>
    public WhatsAppSenderTests()
    {
        _provider =
            Substitute.For<IWhatsAppProvider>();
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the provider is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ProviderIsNull()
    {
        // Arrange

        // Act
        Action action = () =>
            _ = new WhatsAppSender(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("provider");
    }

    /// <summary>
    /// Verifies that the constructor creates
    /// the sender successfully.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateInstance_When_ProviderIsValid()
    {
        // Arrange

        // Act
        WhatsAppSender sender =
            new(_provider);

        // Assert
        sender.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that SendAsync delegates
    /// the request to the provider.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_InvokeProvider()
    {
        // Arrange
        WhatsAppSender sender =
            new(_provider);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        DeliveryResult expected =
            DeliveryResult.Success("MSG-001");

        _provider
            .SendAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        DeliveryResult result =
            await sender.SendAsync(message);

        // Assert
        result.Should().BeSameAs(expected);

        await _provider
            .Received(1)
            .SendAsync(
                message,
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that the cancellation token
    /// is forwarded to the provider.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ForwardCancellationToken()
    {
        // Arrange
        WhatsAppSender sender =
            new(_provider);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        CancellationToken cancellationToken =
            new();

        _provider
            .SendAsync(
                message,
                cancellationToken)
            .Returns(
                DeliveryResult.Success("MSG-001"));

        // Act
        await sender.SendAsync(
            message,
            cancellationToken);

        // Assert
        await _provider
            .Received(1)
            .SendAsync(
                message,
                cancellationToken);
    }

    /// <summary>
    /// Verifies that exceptions from the
    /// provider are propagated.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_PropagateException_When_ProviderThrows()
    {
        // Arrange
        WhatsAppSender sender =
            new(_provider);

        WhatsAppMessage message =
            WhatsAppMessageTestData.CreateValid();

        _provider
            .SendAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns( 
                Task.FromException<DeliveryResult>(
                    new InvalidOperationException("Provider failure.")));    

        // Act
        Func<Task> action =
            () => sender.SendAsync(message);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Provider failure.");
    }
}