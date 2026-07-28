// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeEmailFormatter.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Formats authentication challenge email messages.
///
/// <para>
/// Responsible for transforming authentication challenge
/// delivery requests into email messages suitable for
/// delivery.
/// </para>
///
/// <para>
/// Implementations must not send email.
/// </para>
/// </summary>
public interface IAuthenticationChallengeEmailFormatter
{
    /// <summary>
    /// Formats an authentication challenge email.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <returns>
    /// The formatted authentication email message.
    /// </returns>
    AuthenticationEmailMessage Format(
        AuthenticationChallengeDeliveryRequest request);
}