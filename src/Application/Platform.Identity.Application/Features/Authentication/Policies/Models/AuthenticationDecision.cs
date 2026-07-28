// ===========================================
// File Location :
// src/Application/
// Platform.Identity.Application/
// Features/Authentication/
// Policies/Models/
// AuthenticationDecision.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Policies.Models;

/// <summary>
/// Represents the result returned by the authentication policy evaluation
/// pipeline.
/// </summary>
/// <remarks>
/// This model is immutable and contains the final decision produced by the
/// authentication policy evaluator. The <c>LoginUseCase</c> consumes this
/// result to determine the next authentication action.
/// </remarks>
public sealed record AuthenticationDecision
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationDecision"/> class.
    /// </summary>
    /// <param name="decision">
    /// The authentication decision.
    /// </param>
    /// <param name="reason">
    /// Optional human-readable reason describing the decision.
    /// </param>
    public AuthenticationDecision(
        AuthenticationDecisionType decision,
        string? reason = null)
    {
        Decision = decision;
        Reason = reason;
    }

    /// <summary>
    /// Gets the authentication decision.
    /// </summary>
    public AuthenticationDecisionType Decision { get; }

    /// <summary>
    /// Gets the optional reason associated with the decision.
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    /// Creates an allow decision.
    /// </summary>
    /// <returns>
    /// A successful authentication decision.
    /// </returns>
    public static AuthenticationDecision Allow()
        => new(AuthenticationDecisionType.Allow);

    /// <summary>
    /// Creates a verification-required decision.
    /// </summary>
    /// <param name="reason">
    /// Optional reason.
    /// </param>
    /// <returns>
    /// A verification-required decision.
    /// </returns>
    public static AuthenticationDecision RequireVerification(
        string? reason = null)
        => new(AuthenticationDecisionType.RequireVerification, reason);

    /// <summary>
    /// Creates a challenge-required decision.
    /// </summary>
    /// <param name="reason">
    /// Optional reason.
    /// </param>
    /// <returns>
    /// A challenge-required decision.
    /// </returns>
    public static AuthenticationDecision RequireChallenge(
        string? reason = null)
        => new(AuthenticationDecisionType.RequireChallenge, reason);

    /// <summary>
    /// Creates a password-reset-required decision.
    /// </summary>
    /// <param name="reason">
    /// Optional reason.
    /// </param>
    /// <returns>
    /// A password-reset-required decision.
    /// </returns>
    public static AuthenticationDecision RequirePasswordReset(
        string? reason = null)
        => new(AuthenticationDecisionType.RequirePasswordReset, reason);

    /// <summary>
    /// Creates a deny decision.
    /// </summary>
    /// <param name="reason">
    /// Optional reason.
    /// </param>
    /// <returns>
    /// A denied authentication decision.
    /// </returns>
    public static AuthenticationDecision Deny(
        string? reason = null)
        => new(AuthenticationDecisionType.Deny, reason);

    /// <summary>
    /// Creates a lock-account decision.
    /// </summary>
    /// <param name="reason">
    /// Optional reason.
    /// </param>
    /// <returns>
    /// A lock-account decision.
    /// </returns>
    public static AuthenticationDecision LockAccount(
        string? reason = null)
        => new(AuthenticationDecisionType.LockAccount, reason);
}