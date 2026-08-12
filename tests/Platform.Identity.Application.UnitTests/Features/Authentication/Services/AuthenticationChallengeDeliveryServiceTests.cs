
using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.Features.Authentication.Services;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Services;

/// <summary>
/// Unit tests for
/// <see cref="AuthenticationChallengeDeliveryService"/>.
/// </summary>
public sealed class AuthenticationChallengeDeliveryServiceTests
{
    private readonly Mock<IEmailAuthenticationChallengeSender>
        _emailSender = new();

    private readonly Mock<ISmsAuthenticationChallengeSender>
        _smsSender = new();

    private readonly Mock<IWhatsAppAuthenticationChallengeSender>
        _whatsAppSender = new();

    private readonly Mock<ITotpProvisioningService>
        _totpProvisioningService = new();

    /// <summary>
    /// Verifies the constructor throws when the email sender
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_EmailSender_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeDeliveryService(
                null!,
                _smsSender.Object,
                _whatsAppSender.Object,
                _totpProvisioningService.Object);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("emailSender");
    }

    /// <summary>
    /// Verifies the constructor throws when the SMS sender
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_SmsSender_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeDeliveryService(
                _emailSender.Object,
                null!,
                _whatsAppSender.Object,
                _totpProvisioningService.Object);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("smsSender");
    }

    /// <summary>
    /// Verifies the constructor throws when the WhatsApp sender
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_WhatsAppSender_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeDeliveryService(
                _emailSender.Object,
                _smsSender.Object,
                null!,
                _totpProvisioningService.Object);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("whatsAppSender");
    }

    /// <summary>
    /// Verifies the constructor throws when the provisioning
    /// service is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_TotpProvisioningService_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeDeliveryService(
                _emailSender.Object,
                _smsSender.Object,
                _whatsAppSender.Object,
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("totpProvisioningService");
    }

    /// <summary>
    /// Verifies DeliverAsync throws when the request is null.
    /// </summary>
    [Fact]
    public async Task DeliverAsync_Should_Throw_When_Request_Is_Null()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Func<Task> act =
            () => sut.DeliverAsync(null!);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("request");
    }

    /// <summary>
    /// Verifies email challenges are delivered using the email sender.
    /// </summary>
    [Fact]
    public async Task DeliverAsync_Should_Send_Email_Challenge()
    {
        // Arrange
        var request =
            CreateRequest(AuthenticationChallengeType.EmailOtp);

        var sut = CreateSut();

        // Act
        await sut.DeliverAsync(request);

        // Assert
        _emailSender.Verify(
            x => x.SendAsync(
                request,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _smsSender.VerifyNoOtherCalls();
        _whatsAppSender.VerifyNoOtherCalls();
        _totpProvisioningService.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies SMS challenges are delivered using the SMS sender.
    /// </summary>
    [Fact]
    public async Task DeliverAsync_Should_Send_Sms_Challenge()
    {
        // Arrange
        var request =
            CreateRequest(AuthenticationChallengeType.SmsOtp);

        var sut = CreateSut();

        // Act
        await sut.DeliverAsync(request);

        // Assert
        _smsSender.Verify(
            x => x.SendAsync(
                request,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _emailSender.VerifyNoOtherCalls();
        _whatsAppSender.VerifyNoOtherCalls();
        _totpProvisioningService.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies WhatsApp challenges are delivered using the WhatsApp sender.
    /// </summary>
    [Fact]
    public async Task DeliverAsync_Should_Send_WhatsApp_Challenge()
    {
        // Arrange
        var request =
            CreateRequest(AuthenticationChallengeType.WhatsAppOtp);

        var sut = CreateSut();

        // Act
        await sut.DeliverAsync(request);

        // Assert
        _whatsAppSender.Verify(
            x => x.SendAsync(
                request,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _emailSender.VerifyNoOtherCalls();
        _smsSender.VerifyNoOtherCalls();
        _totpProvisioningService.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies TOTP challenges are provisioned using the
    /// provisioning service.
    /// </summary>
    [Fact]
    public async Task DeliverAsync_Should_Provision_Totp_Challenge()
    {
        // Arrange
        var request =
            CreateRequest(AuthenticationChallengeType.Totp);

        var sut = CreateSut();

        // Act
        await sut.DeliverAsync(request);

        // Assert
        _totpProvisioningService.Verify(
            x => x.ProvisionAsync(
                request,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _emailSender.VerifyNoOtherCalls();
        _smsSender.VerifyNoOtherCalls();
        _whatsAppSender.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Creates a delivery request.
    /// </summary>
    private static AuthenticationChallengeDeliveryRequest CreateRequest(
        AuthenticationChallengeType challengeType)
    {
        return new AuthenticationChallengeDeliveryRequest(
            AuthenticationChallengeFixture.Create(
                ChallengeSecretFixture.Create(),
                challengeType: challengeType),
            UserAccountFixture.Create(),
            "123456");
    }

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private AuthenticationChallengeDeliveryService CreateSut()
    {
        return new AuthenticationChallengeDeliveryService(
            _emailSender.Object,
            _smsSender.Object,
            _whatsAppSender.Object,
            _totpProvisioningService.Object);
    }
}