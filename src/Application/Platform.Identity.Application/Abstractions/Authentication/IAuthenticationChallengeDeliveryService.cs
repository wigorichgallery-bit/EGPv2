// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeDeliveryService.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Delivers authentication challenges to end users.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Route authentication challenges to the appropriate
/// authentication delivery channel.
/// </description>
/// </item>
/// <item>
/// <description>
/// Abstract authentication delivery infrastructure from
/// application workflows.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not modify aggregates.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist data.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not generate authentication secrets.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface IAuthenticationChallengeDeliveryService
{
    /// <summary>
    /// Delivers an authentication challenge.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous delivery
    /// operation.
    /// </returns>
    Task DeliverAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default);
}