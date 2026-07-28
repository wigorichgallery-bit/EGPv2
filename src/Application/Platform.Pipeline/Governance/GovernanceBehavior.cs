// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Governance/GovernanceBehavior.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Governance;

/// <summary>
/// Governance pipeline behavior for requests
/// returning Result.
///
/// Responsibility:
/// - Detect governed requests.
/// - Execute governance evaluation.
/// - Stop pipeline on governance failure.
/// - Continue pipeline on governance success.
///
/// Side Effects:
/// - None.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
public sealed class GovernanceBehavior<TRequest>
    : IPipelineBehavior<TRequest>, IPipelineOrdered
{
    private readonly IGovernanceEvaluator<TRequest>?
        _evaluator;
    public int Order => 200;
    /// <summary>
    /// Initializes behavior.
    /// </summary>
    /// <param name="evaluator">
    /// Governance evaluator.
    /// </param>
    public GovernanceBehavior(
        IGovernanceEvaluator<TRequest>? evaluator)
    {
        _evaluator = evaluator;
    }

    /// <inheritdoc />
    public Task<Result> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result>> next)
    {
        Guard.AgainstNull(
            request,
            nameof(request));

        Guard.AgainstNull(
            next,
            nameof(next));

        if (request is not IGovernanceRequest)
        {
            return next();
        }

        if (_evaluator is null)
        {
            return next();
        }

        var result =
            _evaluator.Evaluate(request);

        if (result.IsFailure)
        {
            return Task.FromResult(result);
        }

        return next();
    }
}