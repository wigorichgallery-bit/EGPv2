// ===========================================
// File Location :
// src/Application/
// Platform.Identity.Application/
// Features/Authentication/
// Policies/Models/
// PolicyEvaluationResult.cs
// ===========================================

namespace Platform.Identity.Application.Features.Authentication.Policies.Models;

/// <summary>
/// Represents the outcome of evaluating an individual authentication policy.
/// </summary>
/// <remarks>
/// Each authentication policy produces a <see cref="PolicyEvaluationResult"/>
/// that is consumed by the authentication policy evaluator.
///
/// A successful policy evaluation may allow the pipeline to continue,
/// while a failed evaluation may stop the pipeline and return an
/// <see cref="AuthenticationDecision"/>.
/// </remarks>
public sealed record PolicyEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PolicyEvaluationResult"/> class.
    /// </summary>
    /// <param name="isSuccessful">
    /// Indicates whether the policy evaluation succeeded.
    /// </param>
    /// <param name="shouldContinue">
    /// Indicates whether the authentication pipeline should continue
    /// evaluating subsequent policies.
    /// </param>
    /// <param name="decision">
    /// Authentication decision returned by the policy.
    /// </param>
    public PolicyEvaluationResult(
        bool isSuccessful,
        bool shouldContinue,
        AuthenticationDecision decision)
    {
        IsSuccessful = isSuccessful;
        ShouldContinue = shouldContinue;
        Decision = decision;
    }

    /// <summary>
    /// Gets a value indicating whether the policy evaluation succeeded.
    /// </summary>
    public bool IsSuccessful { get; }

    /// <summary>
    /// Gets a value indicating whether the authentication pipeline should
    /// continue evaluating the remaining policies.
    /// </summary>
    public bool ShouldContinue { get; }

    /// <summary>
    /// Gets the authentication decision produced by the policy.
    /// </summary>
    public AuthenticationDecision Decision { get; }

    /// <summary>
    /// Creates a successful policy evaluation result that allows
    /// the authentication pipeline to continue.
    /// </summary>
    /// <returns>
    /// A successful policy evaluation result.
    /// </returns>
    public static PolicyEvaluationResult Continue()
    {
        return new(
            isSuccessful: true,
            shouldContinue: true,
            decision: AuthenticationDecision.Allow());
    }

    /// <summary>
    /// Creates a policy evaluation result that stops the authentication
    /// pipeline and returns the specified decision.
    /// </summary>
    /// <param name="decision">
    /// The authentication decision.
    /// </param>
    /// <returns>
    /// A policy evaluation result that terminates the pipeline.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decision"/> is <see langword="null"/>.
    /// </exception>
    public static PolicyEvaluationResult Stop(
        AuthenticationDecision decision)
    {
        ArgumentNullException.ThrowIfNull(decision);

        return new(
            isSuccessful: false,
            shouldContinue: false,
            decision: decision);
    }
}