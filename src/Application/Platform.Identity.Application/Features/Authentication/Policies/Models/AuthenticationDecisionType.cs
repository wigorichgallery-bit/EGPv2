// ===========================================
// File Location :
// src/Application/
// Platform.Identity.Application/
// Features/Authentication/
// Policies/Models/
// AuthenticationDecisionType.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Policies.Models;

/// <summary>
/// Represents the final decision returned by the authentication policy
/// evaluation pipeline.
/// </summary>
/// <remarks>
/// This enumeration is consumed by the authentication policy evaluator and
/// interpreted by the <c>LoginUseCase</c> to determine the next action in the
/// authentication workflow.
///
/// The values defined in this enumeration are intentionally transport-agnostic
/// and do not perform any infrastructure-specific behavior.
/// </remarks>
public enum AuthenticationDecisionType
{
    /// <summary>
    /// The authentication request satisfies every configured policy and may
    /// continue to token generation.
    /// </summary>
    Allow = 0,

    /// <summary>
    /// The user must complete an identity verification step before the
    /// authentication process can continue.
    /// </summary>
    RequireVerification = 1,

    /// <summary>
    /// The user must complete an authentication challenge, such as MFA,
    /// before access can be granted.
    /// </summary>
    RequireChallenge = 2,

    /// <summary>
    /// The user must reset or change the current password before
    /// authentication can continue.
    /// </summary>
    RequirePasswordReset = 3,

    /// <summary>
    /// The authentication request must be denied.
    /// </summary>
    Deny = 4,

    /// <summary>
    /// The user account must be locked as the result of policy evaluation.
    /// </summary>
    LockAccount = 5
}