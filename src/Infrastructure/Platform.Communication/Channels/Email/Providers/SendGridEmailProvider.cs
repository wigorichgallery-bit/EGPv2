using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Exceptions;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Providers;

/// <summary>
/// Provides email delivery through SendGrid.
/// </summary>
internal sealed class SendGridEmailProvider : IEmailProvider
{
    private readonly ISendGridClient _client;

    private readonly ILogger<SendGridEmailProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SendGridEmailProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The SendGrid client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public SendGridEmailProvider(
        ISendGridClient client,
        ILogger<SendGridEmailProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(logger);

        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<DeliveryResult> SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Sending email via SendGrid provider.");

        try
        {
            var result = await _client
                .SendEmailAsync(
                    message,
                    cancellationToken)
                .ConfigureAwait(false);

            return DeliveryResult.Success(
                result.MessageId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "SendGrid email request was cancelled.");

            throw;
        }
        catch (CommunicationException exception)
        {
            _logger.LogError(
                exception,
                "Failed to send email through SendGrid.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}