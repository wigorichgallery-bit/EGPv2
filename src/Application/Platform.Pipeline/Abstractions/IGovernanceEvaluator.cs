// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IGovernanceEvaluator.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines governance evaluation contract.
///
/// Responsibility:
/// - Evaluate governance policies.
/// - Evaluate approval requirements.
/// - Evaluate risk controls.
/// - Return governance outcome.
///
/// Side Effects:
/// - None.
/// </summary>
/// <typeparam name="TRequest">
/// Governance request type.
/// </typeparam>
public interface IGovernanceEvaluator<in TRequest>    
{
    /// <summary>
    /// Evaluates governance rules.
    /// </summary>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <returns>
    /// Evaluation result.
    /// </returns>
    Result Evaluate(
        TRequest request);
}