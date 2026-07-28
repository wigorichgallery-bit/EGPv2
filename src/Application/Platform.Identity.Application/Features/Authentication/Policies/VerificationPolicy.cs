// ===========================================
// File Location:
//
// src/Application/Platform.Identity.Application/
// Features/Authentication/Policies/
// VerificationPolicy.cs
//
// Status : Extension
// ===========================================

using Platform.Identity.Application.Features.Authentication.Policies.Contracts;
using Platform.Identity.Application.Features.Authentication.Policies.Models;

namespace Platform.Identity.Application.Features.Authentication.Policies;

/// <summary>
/// Represents the default contact verification policy.
///
/// </summary>
/// <remarks>
/// This policy ensures that the user has verified at least one
/// contact method before authentication is allowed to continue.
///
/// The policy follows the existing domain rule implemented by
/// <c>UserAccount</c>, where either a verified email address or
/// a verified phone number is sufficient.
///
/// This policy is read-only and never modifies the aggregate.
/// </remarks>
public sealed class VerificationPolicy : IAuthenticationPolicy
{
    /// <inheritdoc />
    public Task<PolicyEvaluationResult> EvaluateAsync(
        AuthenticationContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        var user = context.User;

        var isVerified =
            user.EmailVerified ||
            user.PhoneVerified;

        if (!isVerified)
        {
            return Task.FromResult(
                PolicyEvaluationResult.Stop(
                    AuthenticationDecision.RequireVerification(
                        "At least one verified contact method is required.")));
        }

        return Task.FromResult(
            PolicyEvaluationResult.Continue());
    }
}