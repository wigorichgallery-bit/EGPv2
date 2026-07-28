// ===========================================
// File Location:
//
// src/Application/Platform.Identity.Application/
// Features/Authentication/Policies/Contracts/
// IAuthenticationPolicy.cs
//
// Status : Extension
// ===========================================

using Platform.Identity.Application.Features.Authentication.Policies.Models;

namespace Platform.Identity.Application.Features.Authentication.Policies.Contracts;

/// <summary>
/// Defines the contract for a single authentication policy.
///
/// </summary>
/// <remarks>
/// An authentication policy evaluates one aspect of the authentication
/// process, such as account verification, account lockout,
/// password expiration, multi-factor authentication requirements,
/// or adaptive risk assessment.
///
/// Policies are executed by
/// <see cref="IAuthenticationPolicyEvaluator"/> in a deterministic order.
///
/// Implementations should remain stateless and thread-safe.
/// </remarks>
public interface IAuthenticationPolicy
{
    /// <summary>
    /// Evaluates the current authentication policy.
    /// </summary>
    /// <param name="context">
    /// The authentication execution context.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="PolicyEvaluationResult"/> indicating whether
    /// authentication processing should continue or stop.
    /// </returns>
    Task<PolicyEvaluationResult> EvaluateAsync(
        AuthenticationContext context,
        CancellationToken cancellationToken = default);
}