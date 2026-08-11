using FluentAssertions;

using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;
using Platform.Communication.UnitTests.TestData;

using Xunit;

namespace Platform.Communication.UnitTests.Channels.Email.Clients;

/// <summary>
/// Contains unit tests for
/// <see cref="MailKitSmtpClient"/>.
/// </summary>
public sealed class MailKitSmtpClientTests
{
    private readonly IMailKitSmtpSdkClientFactory _factory;

    private readonly IMailKitSmtpSdkClient _sdkClient;

    private readonly ILogger<MailKitSmtpClient> _logger;

    public MailKitSmtpClientTests()
    {
        _factory =
            Substitute.For<IMailKitSmtpSdkClientFactory>();

        _sdkClient =
            Substitute.For<IMailKitSmtpSdkClient>();

        _logger =
            Substitute.For<ILogger<MailKitSmtpClient>>();

        _factory
            .Create()
            .Returns(_sdkClient);
    }

    // ==========================================================
    // Constructor
    // ==========================================================

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FactoryIsNull()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        // Act

        Action action =
            () =>
                new MailKitSmtpClient(
                    null!,
                    Microsoft.Extensions.Options.Options.Create(options),
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("factory");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_OptionsAreNull()
    {
        // Act

        Action action =
            () =>
                new MailKitSmtpClient(
                    _factory,
                    null!,
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("options");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        // Act

        Action action =
            () =>
                new MailKitSmtpClient(
                    _factory,
                     Microsoft.Extensions.Options.Options.Create(options),
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_HostIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Host =
            string.Empty;

        // Act

        Action action =
            () => CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SMTP Host is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_PortIsInvalid()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Port =
            0;

        // Act

        Action action =
            () => CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SMTP Port is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderAddressIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.SenderAddress =
            string.Empty;

        // Act

        Action action =
            () => CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SMTP SenderAddress is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderNameIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.SenderName =
            string.Empty;

        // Act

        Action action =
            () => CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SMTP SenderName is not configured.");
    }

    [Fact]
    public void Constructor_Should_CreateInstance_When_ConfigurationIsValid()
    {
        // Act

        MailKitSmtpClient sut =
            CreateSut();

        // Assert

        sut.Should()
            .NotBeNull();
    }

    // ==========================================================
    // SendEmailAsync - Guards
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    null!);

        // Assert

        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("message");

        _factory
            .DidNotReceive()
            .Create();
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowOperationCanceledException_When_CancellationIsRequested()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        using CancellationTokenSource source =
            new();

        source.Cancel();

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    EmailMessageTestData.CreateValid(),
                    source.Token);

        // Assert

        await action.Should()
            .ThrowAsync<OperationCanceledException>();

        _factory
            .DidNotReceive()
            .Create();
    }

    // ==========================================================
    // SendEmailAsync - Success
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_SendEmailSuccessfully()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns("MSG-001");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        VendorDeliveryResult result =
            await sut.SendEmailAsync(
                message);

        // Assert

        result.Should()
            .NotBeNull();

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .Be("MSG-001");

        result.ProviderReference
            .Should()
            .Be("MSG-001");

        result.Status
            .Should()
            .Be("Accepted");

        _factory
            .Received(1)
            .Create();

        await _sdkClient
            .Received(1)
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>());

        await _sdkClient
            .Received(1)
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>());

        await _sdkClient
            .Received(1)
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_Authenticate_When_UsernameIsConfigured()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username =
            "smtp-user";

        options.Email.Configuration.Smtp.Password =
            "smtp-password";

        MailKitSmtpClient sut =
            CreateSut(options);

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .AuthenticateAsync(
                "smtp-user",
                "smtp-password",
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns("MSG-002");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .AuthenticateAsync(
                "smtp-user",
                "smtp-password",
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_NotAuthenticate_When_UsernameIsNotConfigured()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username =
            string.Empty;

        MailKitSmtpClient sut =
            CreateSut(options);

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns("MSG-003");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .DidNotReceive()
            .AuthenticateAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // SMTP Configuration
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_UseSslOnConnect_When_EnableSslIsTrue()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.EnableSsl =
            true;

        MailKitSmtpClient sut =
            CreateSut(options);

        _sdkClient
            .ConnectAsync(
                options.Email.Configuration.Smtp.Host,
                options.Email.Configuration.Smtp.Port,
                SecureSocketOptions.SslOnConnect,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns("MSG-004");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .ConnectAsync(
                options.Email.Configuration.Smtp.Host,
                options.Email.Configuration.Smtp.Port,
                SecureSocketOptions.SslOnConnect,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_UseStartTls_When_EnableSslIsFalse()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.EnableSsl =
            false;

        MailKitSmtpClient sut =
            CreateSut(options);

        _sdkClient
            .ConnectAsync(
                options.Email.Configuration.Smtp.Host,
                options.Email.Configuration.Smtp.Port,
                SecureSocketOptions.StartTlsWhenAvailable,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns("MSG-005");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .ConnectAsync(
                options.Email.Configuration.Smtp.Host,
                options.Email.Configuration.Smtp.Port,
                SecureSocketOptions.StartTlsWhenAvailable,
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // MIME Message
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapMimeMessage()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        MimeMessage? capturedMessage =
            null;

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Do<MimeMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns("MSG-006");

        _sdkClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            message);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Subject
            .Should()
            .Be(message.Subject);

        capturedMessage
            .To
            .Count
            .Should()
            .Be(message.To.Count);
    }

    // ==========================================================
    // Cancellation
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_PropagateOperationCanceledException()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromCanceled(
                    new CancellationToken(
                        canceled: true)));

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    EmailMessageTestData.CreateValid());

        // Assert

        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    // ==========================================================
    // Exceptions
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_WrapException_In_CommunicationException()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        InvalidOperationException exception =
            new("SMTP failure.");

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException(
                    exception));

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    EmailMessageTestData.CreateValid());

        // Assert

        CommunicationException result =
           (
               await action.Should()
                   .ThrowAsync<CommunicationException>()
           ).Which;

        result.InnerException
            .Should()
            .Be(exception);

    }

    [Fact]
    public async Task SendEmailAsync_Should_WrapEmptyMessageId_In_CommunicationException()
    {
        // Arrange

        MailKitSmtpClient sut =
            CreateSut();

        _sdkClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _sdkClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(string.Empty);

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    EmailMessageTestData.CreateValid());

        // Assert

        await action.Should()
            .ThrowAsync<CommunicationException>()
            .WithMessage(
                "Failed to send email using SMTP.");
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    private MailKitSmtpClient CreateSut(
        CommunicationOptions? options = null)
    {
        CommunicationOptions communicationOptions =
            options ??
            CreateOptions();

        return new MailKitSmtpClient(
            _factory,
             Microsoft.Extensions.Options.Options.Create(
                communicationOptions),
            _logger);
    }

    private static CommunicationOptions CreateOptions()
    {
        return CommunicationOptionsTestData.CreateSmtp();
    }
}