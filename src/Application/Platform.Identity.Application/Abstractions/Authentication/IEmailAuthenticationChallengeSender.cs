// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IEmailAuthenticationChallengeSender.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Sends authentication challenges using email.
///
/// <para>
/// This abstraction is dedicated to authentication
/// workflows and must not be used as a generic email
/// service.
/// </para>
/// </summary>
public interface IEmailAuthenticationChallengeSender
{
    /// <summary>
    /// Sends an authentication challenge using email.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task SendAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default);
}