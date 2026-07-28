// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Delivery/
// EmailAuthenticationChallengeSender.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Delivery;

/// <summary>
/// Sends authentication challenges using email.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Format authentication challenge email messages.
/// </description>
/// </item>
/// <item>
/// <description>
/// Act as the infrastructure adapter between the
/// Authentication module and the email communication
/// provider.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This implementation intentionally does not contain a
/// concrete email transport. Email transport will be
/// provided by the Platform.Communication module.
/// </para>
/// </summary>
public sealed class EmailAuthenticationChallengeSender
    : IEmailAuthenticationChallengeSender
{
    private readonly IAuthenticationChallengeEmailFormatter
        _formatter;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="EmailAuthenticationChallengeSender"/>
    /// class.
    /// </summary>
    /// <param name="formatter">
    /// Authentication email formatter.
    /// </param>
    public EmailAuthenticationChallengeSender(
        IAuthenticationChallengeEmailFormatter formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        _formatter = formatter;
    }

    /// <inheritdoc />
    public Task SendAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        AuthenticationEmailMessage message =
            _formatter.Format(request);

        return SendCoreAsync(
            message,
            cancellationToken);
    }

    /// <summary>
    /// Sends the formatted authentication email message.
    /// </summary>
    /// <param name="message">
    /// Formatted authentication email message.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when no email communication provider has been
    /// configured.
    /// </exception>
    private static Task SendCoreAsync(
        AuthenticationEmailMessage message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        throw new NotSupportedException(
            "No email communication provider has been configured.");
    }
}