// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeWhatsAppFormatter.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Formats authentication challenge WhatsApp messages.
/// </summary>
public interface IAuthenticationChallengeWhatsAppFormatter
{
    /// <summary>
    /// Formats an authentication challenge WhatsApp message.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <returns>
    /// The formatted WhatsApp message.
    /// </returns>
    AuthenticationWhatsAppMessage Format(
        AuthenticationChallengeDeliveryRequest request);
}