// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// ISmsAuthenticationChallengeSender.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Sends authentication challenges using SMS.
/// </summary>
public interface ISmsAuthenticationChallengeSender
{
    /// <summary>
    /// Sends an authentication challenge using SMS.
    /// </summary>
    Task SendAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default);
}