using NSubstitute;

using Platform.Communication.Channels.Email.Providers;
using Platform.Communication.Channels.Email.Sender;
using Platform.Communication.Models;
using Platform.Communication.ValueObjects;

namespace Platform.Communication.UnitTests.Channels.Email.Sender;

/// <summary>
/// Contains unit tests for <see cref="EmailSender"/>.
/// </summary>
public sealed class EmailSenderTests
{
    private readonly IEmailProvider _provider;

    public EmailSenderTests()
    {
        _provider = Substitute.For<IEmailProvider>();
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
            _ = new EmailSender(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("provider");
    }

    /// <summary>
    /// Verifies that the constructor succeeds
    /// when the provider is valid.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateInstance_When_ProviderIsValid()
    {
        // Arrange / Act
        EmailSender sender = new(_provider);

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
        EmailSender sender = new(_provider);

        EmailMessage message = CreateMessage();

        _provider
            .SendAsync(
                message,
                Arg.Any<CancellationToken>())
            .Returns(DeliveryResult.Success("MSG-001"));

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
    public async Task SendAsync_Should_ForwardCancellationToken_When_Provided()
    {
        // Arrange
        EmailSender sender = new(_provider);

        EmailMessage message = CreateMessage();

        CancellationToken cancellationToken = new();

        _provider
            .SendAsync(
                message,
                cancellationToken)
            .Returns(DeliveryResult.Success());

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
        EmailSender sender = new(_provider);

        EmailMessage message = CreateMessage();

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