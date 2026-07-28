// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IWhatsAppAuthenticationChallengeSender.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Sends authentication challenges using WhatsApp.
/// </summary>
public interface IWhatsAppAuthenticationChallengeSender
{
    /// <summary>
    /// Sends an authentication challenge using WhatsApp.
    /// </summary>
    Task SendAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default);
}