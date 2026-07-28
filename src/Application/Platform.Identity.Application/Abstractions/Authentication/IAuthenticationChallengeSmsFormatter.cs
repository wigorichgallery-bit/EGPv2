// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeSmsFormatter.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Formats authentication challenge SMS messages.
/// </summary>
public interface IAuthenticationChallengeSmsFormatter
{
    /// <summary>
    /// Formats an authentication challenge SMS message.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <returns>
    /// The formatted SMS message.
    /// </returns>
    AuthenticationSmsMessage Format(
        AuthenticationChallengeDeliveryRequest request);
}