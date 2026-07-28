// ===========================================
// File Location:
//
// src/Application/Platform.Identity.Application/
// Features/Authentication/Policies/
// DefaultAuthenticationPolicyEvaluator.cs
//
// Status : Extension
// ===========================================

using System.Linq;
using Platform.Identity.Application.Features.Authentication.Policies.Contracts;
using Platform.Identity.Application.Features.Authentication.Policies.Models;

namespace Platform.Identity.Application.Features.Authentication.Policies;

/// <summary>
/// Default implementation of <see cref="IAuthenticationPolicyEvaluator"/>.
///
/// </summary>
/// <remarks>
/// This evaluator executes registered authentication policies sequentially.
/// Evaluation stops immediately when a policy returns a result indicating
/// that authentication processing should not continue.
///
/// When all policies allow processing to continue,
/// an <see cref="AuthenticationDecision.Allow()"/> decision is returned.
///
/// This class contains no authentication business rules. Its responsibility
/// is limited to orchestrating policy execution.
/// </remarks>
public sealed class DefaultAuthenticationPolicyEvaluator
    : IAuthenticationPolicyEvaluator
{
    private readonly IReadOnlyList<IAuthenticationPolicy> _policies;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DefaultAuthenticationPolicyEvaluator"/> class.
    /// </summary>
    /// <param name="policies">
    /// The authentication policies to execute.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="policies"/> is null.
    /// </exception>
    public DefaultAuthenticationPolicyEvaluator(
        IEnumerable<IAuthenticationPolicy> policies)
    {
        ArgumentNullException.ThrowIfNull(policies);

        _policies = policies.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<PolicyEvaluationResult> EvaluateAsync(
        AuthenticationContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var policy in _policies)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await policy.EvaluateAsync(
                context,
                cancellationToken);

            if (!result.ShouldContinue)
            {
                return result;
            }
        }

        return PolicyEvaluationResult.Continue();
    }
}