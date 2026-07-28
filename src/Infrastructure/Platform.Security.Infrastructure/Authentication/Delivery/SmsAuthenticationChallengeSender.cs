// ===========================================
// File Location:
// src/Infrastructure/
// Platform.Security.Infrastructure/
// Authentication/
// Delivery/
// SmsAuthenticationChallengeSender.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Security.Infrastructure.Authentication.Delivery;

/// <summary>
/// Sends authentication challenges using SMS.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Format authentication challenge SMS messages.
/// </description>
/// </item>
/// <item>
/// <description>
/// Act as the infrastructure adapter between the
/// Authentication module and the SMS communication
/// provider.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This implementation intentionally does not contain a
/// concrete SMS transport. SMS transport will be provided
/// by the Platform.Communication module.
/// </para>
/// </summary>
public sealed class SmsAuthenticationChallengeSender
    : ISmsAuthenticationChallengeSender
{
    private readonly IAuthenticationChallengeSmsFormatter
        _formatter;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SmsAuthenticationChallengeSender"/>
    /// class.
    /// </summary>
    /// <param name="formatter">
    /// Authentication SMS formatter.
    /// </param>
    public SmsAuthenticationChallengeSender(
        IAuthenticationChallengeSmsFormatter formatter)
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

        AuthenticationSmsMessage message =
            _formatter.Format(request);

        return SendCoreAsync(
            message,
            cancellationToken);
    }

    /// <summary>
    /// Sends the formatted authentication SMS message.
    /// </summary>
    /// <param name="message">
    /// Formatted authentication SMS message.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when no SMS communication provider has been
    /// configured.
    /// </exception>
    private static Task SendCoreAsync(
        AuthenticationSmsMessage message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        throw new NotSupportedException(
            "No SMS communication provider has been configured.");
    }
}