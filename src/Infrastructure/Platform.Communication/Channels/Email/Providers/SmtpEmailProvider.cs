using MailKit;
using Microsoft.Extensions.Logging;

using Platform.Communication.Channels.Email.Clients;
using Platform.Communication.Models;

namespace Platform.Communication.Channels.Email.Providers;

/// <summary>
/// Provides email delivery through SMTP.
/// </summary>
internal sealed class SmtpEmailProvider : IEmailProvider
{
    private readonly IMailKitSmtpClient _client;

    private readonly ILogger<SmtpEmailProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SmtpEmailProvider"/> class.
    /// </summary>
    /// <param name="client">
    /// The SMTP client.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any dependency is
    /// <see langword="null"/>.
    /// </exception>
    public SmtpEmailProvider(
        IMailKitSmtpClient client,
        ILogger<SmtpEmailProvider> logger)
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
            "Sending email via SMTP provider.");

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
                "SMTP email request was cancelled.");

            throw;
        }
        catch (CommandException exception)
        {
            _logger.LogError(
                exception,
                "Failed to send email through SMTP.");

            return DeliveryResult.Failure(
                exception.Message);
        }
    }
}