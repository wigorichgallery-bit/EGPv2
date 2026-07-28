// ===========================================
// File Location:
//
// src/Application/Platform.Identity.Application/
// Features/Authentication/Policies/Contracts/
// IAuthenticationPolicyEvaluator.cs
//
// Status : Extension
// ===========================================

using Platform.Identity.Application.Features.Authentication.Policies.Models;

namespace Platform.Identity.Application.Features.Authentication.Policies.Contracts;

/// <summary>
/// Defines the contract for evaluating authentication policies.
///
/// </summary>
/// <remarks>
/// Implementations evaluate one or more authentication policies using the
/// supplied <see cref="AuthenticationContext"/> and return the outcome of
/// the evaluation.
///
/// This abstraction isolates policy orchestration from the login use case,
/// allowing authentication requirements to evolve independently from the
/// authentication workflow.
///
/// Implementations should remain stateless and thread-safe.
/// </remarks>
public interface IAuthenticationPolicyEvaluator
{
    /// <summary>
    /// Evaluates the configured authentication policies.
    /// </summary>
    /// <param name="context">
    /// The authentication execution context.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="PolicyEvaluationResult"/> describing whether the
    /// authentication workflow should continue or stop, together with the
    /// corresponding authentication decision.
    /// </returns>
    Task<PolicyEvaluationResult> EvaluateAsync(
        AuthenticationContext context,
        CancellationToken cancellationToken = default);
}