using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;
using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;
using Platform.Communication.Options;
using Platform.Communication.UnitTests.TestData;

using SendGrid;
using SendGrid.Helpers.Mail;

using CommunicationSendGridClient =
    Platform.Communication.Channels.Email.Clients.SendGridClient;

namespace Platform.Communication.UnitTests.Channels.Email.Clients;

/// <summary>
/// Contains unit tests for
/// <see cref="CommunicationSendGridClient"/>.
/// </summary>
public sealed class SendGridClientTests
{
    private readonly ISendGridSdkClientFactory _factory;

    private readonly ISendGridSdkClient _sdkClient;

    private readonly ILogger<CommunicationSendGridClient> _logger;

    public SendGridClientTests()
    {
        _factory =
            Substitute.For<ISendGridSdkClientFactory>();

        _sdkClient =
            Substitute.For<ISendGridSdkClient>();

        _logger =
            Substitute.For<ILogger<CommunicationSendGridClient>>();

        _factory
            .Create(
                Arg.Any<string>())
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

        CommunicationSendGridClient sut =
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
                new CommunicationSendGridClient(
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
                new CommunicationSendGridClient(
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
                new CommunicationSendGridClient(
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
    public void Constructor_Should_ThrowInvalidOperationException_When_ApiKeyIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.SendGrid.ApiKey =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SendGrid ApiKey is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderAddressIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.SendGrid.SenderAddress =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SendGrid SenderAddress is not configured.");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_When_SenderNameIsMissing()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        options.Email.Configuration.SendGrid.SenderName =
            string.Empty;

        // Act

        Action action =
            () =>
                CreateSut(options);

        // Assert

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "SendGrid SenderName is not configured.");
    }

    [Fact]
    public void Constructor_Should_CreateSdkClient_WithConfiguredApiKey()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        // Act

        CreateSut(options);

        // Assert

        _factory
            .Received(1)
            .Create(
                options.Email.Configuration.SendGrid.ApiKey);
    }

    // ==========================================================
    // SendEmailAsync - Guard Clauses
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ThrowArgumentNullException_When_MessageIsNull()
    {
        // Arrange

        CommunicationSendGridClient sut =
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
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_Should_ThrowOperationCanceledException_When_CancellationIsRequested()
    {
        // Arrange

        CommunicationSendGridClient sut =
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
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // SendEmailAsync - Success
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ReturnSuccess_When_SendGridAcceptsEmail()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        Response response =
            CreateSuccessfulResponse(
                "SG-MESSAGE-001");

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

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
            .Be("SG-MESSAGE-001");

        result.ProviderReference
            .Should()
            .Be("SG-MESSAGE-001");

        result.Status
            .Should()
            .Be(
                response.StatusCode.ToString());

        result.RawResponse
            .Should()
            .BeSameAs(response);
    }

    [Fact]
    public async Task SendEmailAsync_Should_CallSdkClientOnce()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        Response response =
            CreateSuccessfulResponse(
                "SG-MESSAGE-002");

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        await _sdkClient
            .Received(1)
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>());
    }

    // ==========================================================
    // Cancellation
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_ForwardCancellationToken()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        using CancellationTokenSource source =
            new();

        CancellationToken token =
            source.Token;

        Response response =
            CreateSuccessfulResponse(
                "SG-MESSAGE-003");

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                token)
            .Returns(response);

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid(),
            token);

        // Assert

        await _sdkClient
            .Received(1)
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                token);
    }

    [Fact]
    public async Task SendEmailAsync_Should_PropagateOperationCanceledException()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        OperationCanceledException exception =
            new();

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<Response>(
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
    public async Task SendEmailAsync_Should_MapSender()
    {
        // Arrange

        CommunicationOptions options =
            CreateOptions();

        CommunicationSendGridClient sut =
            CreateSut(options);

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-004"));

        // Act

        await sut.SendEmailAsync(
            EmailMessageTestData.CreateValid());

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .From
            .Email
            .Should()
            .Be(
                options
                    .Email
                    .Configuration
                    .SendGrid
                    .SenderAddress);

        capturedMessage
            .From
            .Name
            .Should()
            .Be(
                options
                    .Email
                    .Configuration
                    .SendGrid
                    .SenderName);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapSubject()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateValid();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-005"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Subject
            .Should()
            .Be(
                email.Subject);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapPlainTextBody()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateValid();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-006"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .PlainTextContent
            .Should()
            .Be(
                email.Body);
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapHtmlBody()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateHtml();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-007"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .HtmlContent
            .Should()
            .Be(
                email.Body);
    }

    // ==========================================================
    // Recipients
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapToRecipients()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateMultipleRecipients();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-008"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Personalizations
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapCcRecipients()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithCc();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-009"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Personalizations
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task SendEmailAsync_Should_MapBccRecipients()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithBcc();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-010"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Personalizations
            .Should()
            .NotBeEmpty();
    }

    // ==========================================================
    // Attachments
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_MapAttachments()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        EmailMessage email =
            EmailMessageTestData.CreateWithAttachment();

        SendGridMessage? capturedMessage =
            null;

        _sdkClient
            .SendEmailAsync(
                Arg.Do<SendGridMessage>(
                    value =>
                        capturedMessage = value),
                Arg.Any<CancellationToken>())
            .Returns(
                CreateSuccessfulResponse(
                    "SG-MESSAGE-011"));

        // Act

        await sut.SendEmailAsync(
            email);

        // Assert

        capturedMessage
            .Should()
            .NotBeNull();

        capturedMessage!
            .Attachments
            .Should()
            .NotBeNull();

        capturedMessage
            .Attachments
            .Should()
            .NotBeEmpty();
    }

    // ==========================================================
    // Provider Reference
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_UseXMessageIdAsProviderReference()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        Response response =
            CreateSuccessfulResponse(
                "SG-HEADER-ID");

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

        // Act

        VendorDeliveryResult result =
            await sut.SendEmailAsync(
                EmailMessageTestData.CreateValid());

        // Assert

        result.MessageId
            .Should()
            .Be(
                "SG-HEADER-ID");

        result.ProviderReference
            .Should()
            .Be(
                "SG-HEADER-ID");
    }

    [Fact]
    public async Task SendEmailAsync_Should_ReturnEmptyProviderReference_When_XMessageIdIsMissing()
    {
        // Arrange

        CommunicationSendGridClient client =
            CreateSut();

        EmailMessage message =
            EmailMessageTestData.CreateValid();

        Response response =
            CreateSuccessfulResponse(
                messageId: null);

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(
                response);

        // Act

        VendorDeliveryResult result =
            await client.SendEmailAsync(
                message);

        // Assert

        result.IsSuccess
            .Should()
            .BeTrue();

        result.MessageId
            .Should()
            .BeNull();

        result.ProviderReference
            .Should()
            .BeNull();

        result.Status
            .Should()
            .Be("Accepted");

        result.RawResponse
            .Should()
            .BeSameAs(response);
    }
    
    // ==========================================================
    // Exception Handling
    // ==========================================================

    [Fact]
    public async Task SendEmailAsync_Should_WrapSdkException_In_CommunicationException()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        InvalidOperationException expectedException =
            new(
                "SendGrid SDK failure.");

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<Response>(
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

    [Fact]
    public async Task SendEmailAsync_Should_WrapInvalidResponseException_In_CommunicationException()
    {
        // Arrange

        CommunicationSendGridClient sut =
            CreateSut();

        Response response =
            CreateFailureResponse();

        _sdkClient
            .SendEmailAsync(
                Arg.Any<SendGridMessage>(),
                Arg.Any<CancellationToken>())
            .Returns(response);

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
            .BeOfType<InvalidOperationException>();
    }

    // ==========================================================
    // Helpers
    // ==========================================================

    private CommunicationSendGridClient CreateSut(
        CommunicationOptions? options = null)
    {
        CommunicationOptions communicationOptions =
            options ??
            CreateOptions();

        return new CommunicationSendGridClient(
            _factory,
            Microsoft.Extensions.Options.Options.Create(
                communicationOptions),
            _logger);
    }

    private static CommunicationOptions CreateOptions()
    {
        return CommunicationOptionsTestData.CreateSendGrid();
    }

    private static Response CreateSuccessfulResponse(
        string? messageId)
    {
        using var httpResponse =
            new HttpResponseMessage(
                System.Net.HttpStatusCode.Accepted);

        if (!string.IsNullOrWhiteSpace(messageId))
        {
            httpResponse.Headers.Add(
                "X-Message-Id",
                messageId);
        }

        return new Response(
            httpResponse.StatusCode,
            httpResponse.Content,
            httpResponse.Headers);
    }

    private static Response CreateFailureResponse()
    {
        return new Response(
            System.Net.HttpStatusCode.BadRequest,
            null,
            null);
    }
}