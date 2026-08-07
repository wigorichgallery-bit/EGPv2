using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NSubstitute;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Options;
using Platform.Communication.UnitTests.TestData;

namespace Platform.Communication.UnitTests.Channels.Email.Clients;

/// <summary>
/// Contains unit tests for <see cref="SmtpClient"/>.
/// </summary>
public sealed partial class SmtpClientTests
{
    private readonly IMailKitSmtpClientFactory _factory;

    private readonly IMailKitSmtpClient _mailKitClient;

    private readonly ILogger<Platform.Communication.Channels.Email.Clients.SmtpClient> _logger;

    public SmtpClientTests()
    {
        _factory =
            Substitute.For<IMailKitSmtpClientFactory>();

        _mailKitClient =
            Substitute.For<IMailKitSmtpClient>();

        _logger =
            Substitute.For<
                ILogger<Platform.Communication.Channels.Email.Clients.SmtpClient>>();

        _factory
            .Create()
            .Returns(_mailKitClient);
    }

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private Platform.Communication.Channels.Email.Clients.SmtpClient CreateSut(
        CommunicationOptions? options = null)
    {        
        return new Platform.Communication.Channels.Email.Clients.SmtpClient(
            _factory,
            Microsoft.Extensions.Options.Options.Create(
                options ??
                CreateOptions()),
            _logger);
    }

    /// <summary>
    /// Creates valid communication options.
    /// </summary>
    private static CommunicationOptions CreateOptions()
    {
        return CommunicationOptionsTestData.CreateSmtp();
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="ArgumentNullException"/>
    /// when the factory is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FactoryIsNull()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        // Act
        Action action =
            () => new Platform.Communication.Channels.Email.Clients.SmtpClient(
                null!,
                Microsoft.Extensions.Options.Options.Create(options),
                _logger);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("factory");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="ArgumentNullException"/>
    /// when options are null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_OptionsAreNull()
    {
        // Act
        Action action =
            () => new Platform.Communication.Channels.Email.Clients.SmtpClient(
                _factory,
                null!,
                _logger);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("options");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="ArgumentNullException"/>
    /// when logger is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_LoggerIsNull()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        // Act
        Action action =
            () => new Platform.Communication.Channels.Email.Clients.SmtpClient(
                _factory,
                Microsoft.Extensions.Options.Options.Create(options),
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="InvalidOperationException"/>
    /// when the SMTP host is missing.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_HostIsMissing()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Host = string.Empty;

        // Act
        Action action =
            () => CreateSut(options);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("SMTP Host is not configured.");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="InvalidOperationException"/>
    /// when the SMTP port is invalid.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_PortIsInvalid()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Port = 0;

        // Act
        Action action =
            () => CreateSut(options);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("SMTP Port is not configured.");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="InvalidOperationException"/>
    /// when the sender address is missing.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderAddressIsMissing()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.SenderAddress = string.Empty;

        // Act
        Action action =
            () => CreateSut(options);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("SMTP SenderAddress is not configured.");
    }

    /// <summary>
    /// Verifies that the constructor throws
    /// <see cref="InvalidOperationException"/>
    /// when the sender name is missing.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderNameIsMissing()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.SenderName = string.Empty;

        // Act
        Action action =
            () => CreateSut(options);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("SMTP SenderName is not configured.");
    }

    /// <summary>
    /// Verifies that the constructor succeeds
    /// when the configuration is valid.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateInstance_When_ConfigurationIsValid()
    {
        // Arrange

        // Act
        var sut = CreateSut();

        // Assert
        sut.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that SendEmailAsync throws
    /// <see cref="ArgumentNullException"/>
    /// when message is null.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(null!);

        // Assert
        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("message");
    }

    /// <summary>
    /// Verifies that SendEmailAsync throws
    /// <see cref="OperationCanceledException"/>
    /// when cancellation has already been requested.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_ThrowOperationCanceledException_When_Cancelled()
    {
        // Arrange
        var sut = CreateSut();

        var token =
            new CancellationToken(true);

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid(),
                token);

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Verifies that a plain text email
    /// is sent successfully.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_SendPlainTextEmail()
    {
        // Arrange
        var sut = CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("MSG-001"));

        // Act
        VendorDeliveryResult result =
            await sut.SendEmailAsync(message);

        // Assert
        result.Should().NotBeNull();

        result.MessageId.Should().NotBeNullOrWhiteSpace();

        result.MessageId.Should().Be("MSG-001");        

        result.ProviderReference.Should().Be("MSG-001");

        result.Status.Should().Be("Accepted");

        await _mailKitClient
            .Received(1)
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>());

        await _mailKitClient
            .Received(1)
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>());

        await _mailKitClient
            .Received(1)
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that HTML emails are supported.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_SendHtmlEmail()
    {
        // Arrange
        var sut = CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateHtml();

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult("MSG-002"));

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        await _mailKitClient
            .Received(1)
            .SendAsync(
                Arg.Is<MimeMessage>(
                    x => x!.Subject == message.Subject),
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that attachments are included.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_SendAttachments()
    {
        // Arrange
        var sut = CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateWithAttachment();

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-003"));

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        await _mailKitClient
            .Received(1)
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() );
    }

    /// <summary>
    /// Verifies that AuthenticateAsync
    /// is invoked when credentials exist.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_Authenticate_When_UsernameExists()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username =
            "user";

        options.Email.Configuration.Smtp.Password =
            "password";

        var sut =
            CreateSut(options);

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-004"));

        // Act
        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert
        await _mailKitClient
            .Received(1)
            .AuthenticateAsync(
                "user",
                "password",
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that AuthenticateAsync
    /// is skipped when username is empty.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_NotAuthenticate_When_UsernameIsEmpty()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username =
            string.Empty;

        var sut =
            CreateSut(options);

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-005"));

        // Act
        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert
        await _mailKitClient
            .DidNotReceive()
            .AuthenticateAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that exceptions thrown by ConnectAsync
    /// are propagated.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_PropagateException_When_ConnectFails()
    {
        // Arrange
        var sut = CreateSut();

        _mailKitClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromException(
                new InvalidOperationException(
                    "Connect failed.")));

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Connect failed.");
    }

    /// <summary>
    /// Verifies that AuthenticateAsync exceptions
    /// are propagated.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_PropagateException_When_AuthenticateFails()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username = "user";
        options.Email.Configuration.Smtp.Password = "password";

        var sut = CreateSut(options);

        _mailKitClient
            .AuthenticateAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromException(
                new InvalidOperationException(
                    "Authentication failed.")));

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Authentication failed.");
    }

    /// <summary>
    /// Verifies that SendAsync exceptions
    /// are propagated.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_PropagateException_When_SendFails()
    {
        // Arrange
        var sut = CreateSut();

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromException<string>(
                new InvalidOperationException(
                    "Send failed.")));

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Send failed.");
    }

    /// <summary>
    /// Verifies that DisconnectAsync exceptions
    /// are propagated.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_PropagateException_When_DisconnectFails()
    {
        // Arrange
        var sut = CreateSut();

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-001"));

        _mailKitClient
            .DisconnectAsync(
                true,
                Arg.Any<CancellationToken>())
            .Returns(Task.FromException(
                new InvalidOperationException(
                    "Disconnect failed.")));

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Disconnect failed.");
    }

    /// <summary>
    /// Verifies that OperationCanceledException
    /// from MailKit is propagated.
    /// </summary>
    [Fact]
    public async Task SendEmailAsync_Should_PropagateOperationCanceledException_When_MailKitCancels()
    {
        // Arrange
        var sut = CreateSut();

        _mailKitClient
            .ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromCanceled(
                new CancellationToken(true)));

        // Act
        Func<Task> action =
            () => sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert
        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task SendEmailAsync_Should_CreatePlainTextMimeMessage()
    {
        // Arrange
        var sut = CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        MimeMessage? captured = null;

        _mailKitClient
            .SendAsync(
                Arg.Do<MimeMessage>(x => captured = x),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-001"));

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        captured.Should().NotBeNull();

        captured!.Subject.Should().Be(message.Subject);

        captured.To.Mailboxes
        .Should()
        .HaveCount(1);

        captured.To.Mailboxes
            .Single()
            .Address
            .Should()
            .Be(message.To.Single().Value);

        captured.TextBody.Should().Be(message.Body);
    }

    [Fact]
    public async Task SendEmailAsync_Should_SetFromAddress()
    {
        // Arrange
        var sut = CreateSut();

        MimeMessage? captured = null;

        _mailKitClient
            .SendAsync(
                Arg.Do<MimeMessage>(x => captured = x),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-001"));

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        captured.Should().NotBeNull();

        MailboxAddress from =
            captured!
                .From
                .Mailboxes
                .Single();

        from.Name.Should()
            .Be(CreateOptions()
                .Email
                .Configuration
                .Smtp
                .SenderName);

        from.Address.Should()
            .Be(CreateOptions()
                .Email
                .Configuration
                .Smtp
                .SenderAddress);
    }

    [Fact]
    public async Task SendEmailAsync_Should_SetSubject()
    {
        // Arrange
        var sut = CreateSut();

        MimeMessage? captured = null;

        _mailKitClient
            .SendAsync(
                Arg.Do<MimeMessage>(x => captured = x),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-001"));

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        captured!
            .Subject
            .Should()
            .Be(message.Subject);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapAllRecipients()
    {
        // Arrange
        var sut = CreateSut();

        MimeMessage? captured = null;

        _mailKitClient
            .SendAsync(
                Arg.Do<MimeMessage>(x => captured = x),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG-001"));

        EmailMessage message =
            EmailMessageTestData.CreateMultipleRecipients();

        // Act
        await sut.SendEmailAsync(message);

        // Assert
        captured!
            .To
            .Mailboxes
            .Should()
            .HaveCount(message.To.Count);
    }

    [Fact]
    public async Task SendEmailAsync_Should_CallConnectBeforeAuthenticate()
    {
        // Arrange
        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.Smtp.Username = "user";
        options.Email.Configuration.Smtp.Password = "password";

        var sut =
            CreateSut(options);

        _mailKitClient
            .SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>() )
            .Returns(Task.FromResult("MSG"));

        // Act
        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert
        NSubstitute.Received.InOrder(() =>
        {
            _mailKitClient.ConnectAsync(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<SecureSocketOptions>(),
                Arg.Any<CancellationToken>());

            _mailKitClient.AuthenticateAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());

            _mailKitClient.SendAsync(
                Arg.Any<MimeMessage>(),
                Arg.Any<CancellationToken>());

            _mailKitClient.DisconnectAsync(
                true,
                Arg.Any<CancellationToken>());
        });
    }
}