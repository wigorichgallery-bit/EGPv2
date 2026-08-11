using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Platform.Communication.Channels.Email.Configuration;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;
using Platform.Communication.UnitTests.TestData;

using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;

using CommunicationGraphClient =
    Platform.Communication.Channels.Email.Clients.GraphClient;
using Platform.Communication.Channels.Email.Clients;

namespace Platform.Communication.UnitTests.Channels.Email.Clients;

/// <summary>
/// Contains unit tests for
/// <see cref="CommunicationGraphClient"/>.
/// </summary>
public sealed class GraphClientTests
{
    private readonly IGraphSdkClientFactory _factory;

    private readonly IGraphSdkClient _sdkClient;

    private readonly ILogger<CommunicationGraphClient> _logger;

    public GraphClientTests()
    {
        _factory =
            Substitute.For<IGraphSdkClientFactory>();

        _sdkClient =
            Substitute.For<IGraphSdkClient>();

        _logger =
            Substitute.For<ILogger<CommunicationGraphClient>>();

        _factory
            .Create(
                Arg.Any<MicrosoftGraphConfiguration>())
            .Returns(_sdkClient);
    }

    // ==========================================================
    // Constructor
    // ==========================================================

    [Fact]
    public void Constructor_Should_CreateInstance_When_ConfigurationIsValid()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        // Act

        CommunicationGraphClient sut =
            CreateSut(options);

        // Assert

        sut.Should()
            .NotBeNull();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FactoryIsNull()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        // Act

        Action action =
            () =>
                new CommunicationGraphClient(
                    null!,
                    Microsoft.Extensions.Options.Options.Create(
                        options),
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "factory");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_OptionsAreNull()
    {
        // Act

        Action action =
            () =>
                new CommunicationGraphClient(
                    _factory,
                    null!,
                    _logger);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "options");
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
                new CommunicationGraphClient(
                    _factory,
                    Microsoft.Extensions.Options.Options.Create(
                        options),
                    null!);

        // Assert

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(
                "logger");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_TenantIdIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email
            .Configuration
            .MicrosoftGraph
            .TenantId =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Microsoft Graph TenantId is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_ClientIdIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email
            .Configuration
            .MicrosoftGraph
            .ClientId =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Microsoft Graph ClientId is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_ClientSecretIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email
            .Configuration
            .MicrosoftGraph
            .ClientSecret =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Microsoft Graph ClientSecret is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_UserIdIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email
            .Configuration
            .MicrosoftGraph
            .UserId =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Microsoft Graph UserId is not configured.");
    }

    [Fact]
    public void Constructor_Should_CreateSdkClient_WithConfiguredConfiguration()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        MicrosoftGraphConfiguration configuration =
            options.Email
                .Configuration
                .MicrosoftGraph;

        // Act

        CreateSut(options);

        // Assert

        _factory
            .Received(1)
            .Create(
                configuration);
    }

    // ==========================================================
    // SendEmailAsync - Guard Clauses
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    null!);

        // Assert

        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName(
                "message");

        await _sdkClient
            .DidNotReceive()
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowOperationCanceledException_When_CancellationIsRequested()
    {
        // Arrange

        CommunicationGraphClient sut =
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

        await _sdkClient
            .DidNotReceive()
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // SendEmailAsync - Success
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ReturnSuccess_When_GraphAcceptsEmail()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        CommunicationGraphClient sut =
            CreateSut(options);

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        VendorDeliveryResult result =
            await sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert

        result.Should()
            .NotBeNull();

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .NotBeNullOrWhiteSpace();

        result.ProviderReference
            .Should()
            .Be(
                options.Email
                    .Configuration
                    .MicrosoftGraph
                    .UserId);

        result.Status
            .Should()
            .Be(
                "Accepted");
    }

    [Fact]
    public async Task SendEmailAsync_Should_CallSdkClientOnce()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_UseConfiguredUserId()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        CommunicationGraphClient sut =
            CreateSut(options);

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .SendMailAsync(
                options.Email
                    .Configuration
                    .MicrosoftGraph
                    .UserId,
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // Cancellation
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ForwardCancellationToken()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        using CancellationTokenSource source =
            new();

        CancellationToken token =
            source.Token;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                token)
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid(),
            token);

        // Assert

        await _sdkClient
            .Received(1)
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                token);
    }

    [Fact]
    public async Task SendEmailAsync_Should_PropagateOperationCanceledException()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        OperationCanceledException exception =
            new();

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
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

        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    // ==========================================================
    // Message Mapping
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapSubject()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateValid();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest
            .Should()
            .NotBeNull();

        capturedRequest!
            .Message
            .Should()
            .NotBeNull();

        capturedRequest
            .Message!
            .Subject
            .Should()
            .Be(
                email.Subject);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapPlainTextBody()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateValid();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .Body!
            .Content
            .Should()
            .Be(
                email.Body);

        capturedRequest
            .Message!
            .Body!
            .ContentType
            .Should()
            .Be(
                BodyType.Text);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapHtmlBody()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateHtml();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .Body!
            .Content
            .Should()
            .Be(
                email.Body);

        capturedRequest
            .Message!
            .Body!
            .ContentType
            .Should()
            .Be(
                BodyType.Html);
    }

    // ==========================================================
    // Recipients
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapToRecipients()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateValid();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .ToRecipients
            .Should()
            .NotBeNull();

        capturedRequest
            .Message!
            .ToRecipients!
            .Should()
            .HaveCount(
                email.To.Count);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapCcRecipients()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithCc();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .CcRecipients
            .Should()
            .NotBeNull();

        capturedRequest
            .Message!
            .CcRecipients!
            .Should()
            .HaveCount(
                email.Cc!.Count);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapBccRecipients()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithBcc();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .BccRecipients
            .Should()
            .NotBeNull();

        capturedRequest
            .Message!
            .BccRecipients!
            .Should()
            .HaveCount(
                email.Bcc!.Count);
    }

    // ==========================================================
    // Attachments
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapAttachments()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithAttachment();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedRequest!
            .Message!
            .Attachments
            .Should()
            .NotBeNull();

        capturedRequest
            .Message!
            .Attachments!
            .Should()
            .NotBeEmpty();

        capturedRequest
            .Message!
            .Attachments!
            .Count
            .Should()
            .Be(
                email.Attachments!.Count);
    }

    [Fact]
    public async Task SendEmailAsync_Should_SaveToSentItems()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        SendMailPostRequestBody? capturedRequest =
            null;

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Do<SendMailPostRequestBody>(
                    value =>
                        capturedRequest = value),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.CompletedTask);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        capturedRequest!
            .SaveToSentItems
            .Should()
            .BeTrue();
    }

    // ==========================================================
    // Exception Handling
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_WrapSdkException_In_CommunicationException()
    {
        // Arrange

        CommunicationGraphClient sut =
            CreateSut();

        InvalidOperationException expectedException =
            new(
                "Microsoft Graph SDK failure.");

        _sdkClient
            .SendMailAsync(
                Arg.Any<string>(),
                Arg.Any<SendMailPostRequestBody>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException(
                    expectedException));

        // Act

        Func<Task> action =
            () =>
                sut.SendEmailAsync(
                    EmailMessageTestData.CreateValid());

        // Assert

        var exception =
            await action.Should()
                .ThrowAsync<CommunicationException>();

        exception.Which.InnerException
            .Should()
            .Be(
                expectedException);
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    private CommunicationGraphClient CreateSut(
        CommunicationOptions? options = null)
    {
        CommunicationOptions communicationOptions =
            options ??
            CreateOptions();

        return new CommunicationGraphClient(
            _factory,
            Microsoft.Extensions.Options.Options.Create(
                communicationOptions),
            _logger);
    }

    private static CommunicationOptions CreateOptions()
    {
        return CommunicationOptionsTestData.CreateMicrosoftGraph();
    }
}