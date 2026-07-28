// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// ITotpProvisioningService.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Provisions Time-based One-Time Password (TOTP)
/// authenticators.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate authenticator provisioning information.
/// </description>
/// </item>
/// <item>
/// <description>
/// Build enrollment artifacts required by authenticator
/// applications.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Implementations must not deliver messages or verify
/// authentication codes.
/// </para>
/// </summary>
public interface ITotpProvisioningService
{
    /// <summary>
    /// Creates provisioning information for a TOTP
    /// authenticator.
    /// </summary>
    /// <param name="request">
    /// Authentication challenge delivery request.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A task containing the provisioning result.
    /// </returns>
    Task<TotpProvisioningResult> ProvisionAsync(
        AuthenticationChallengeDeliveryRequest request,
        CancellationToken cancellationToken = default);
}