using NSubstitute;

using Platform.Communication.Channels.Sms.Providers;
using Platform.Communication.Channels.Sms.Sender;
using Platform.Communication.Models;
using Platform.Communication.UnitTests.TestData;

namespace Platform.Communication.UnitTests.Channels.Sms.Sender;

/// <summary>
/// Contains unit tests for <see cref="SmsSender"/>.
/// </summary>
public sealed class SmsSenderTests
{
    private readonly ISmsProvider _provider;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SmsSenderTests"/> class.
    /// </summary>
    public SmsSenderTests()
    {
        _provider = Substitute.For<ISmsProvider>();
    }

    /// <summary>
    /// Verifies that the constructor throws an
    /// <see cref="ArgumentNullException"/>
    /// when the provider is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ProviderIsNull()
    {
        // Arrange / Act
        Action action = () =>
            _ = new SmsSender(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("provider");
    }

    /// <summary>
    /// Verifies that the constructor creates
    /// an instance when the provider is valid.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateInstance_When_ProviderIsValid()
    {
        // Arrange / Act
        SmsSender sender = new(_provider);

        // Assert
        sender.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that SendAsync forwards the
    /// message to the provider.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_InvokeProvider_When_MessageIsValid()
    {
        // Arrange
        SmsSender sender = new(_provider);

        SmsMessage message =
            SmsMessageTestData.CreateValid();

        _provider
            .SendAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(
                DeliveryResult.Success("MSG-001"));

        // Act
        await sender.SendAsync(message);

        // Assert
        await _provider
            .Received(1)
            .SendAsync(
                message,
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that SendAsync forwards the
    /// cancellation token to the provider.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ForwardCancellationToken_When_CancellationTokenProvided()
    {
        // Arrange
        SmsSender sender = new(_provider);

        SmsMessage message =
            SmsMessageTestData.CreateValid();

        CancellationToken cancellationToken = new();

        _provider
            .SendAsync(
                message,
                cancellationToken)
            .Returns(
                DeliveryResult.Success());

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
    /// Verifies that SendAsync returns the
    /// provider result unchanged.
    /// </summary>
    [Fact]
    public async Task SendAsync_Should_ReturnProviderResult_When_ProviderReturnsResult()
    {
        // Arrange
        SmsSender sender = new(_provider);

        SmsMessage message =
            SmsMessageTestData.CreateValid();

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
    }
}