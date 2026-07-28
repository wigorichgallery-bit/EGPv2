// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IPipelineBehavior.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines pipeline behavior contract for
/// requests returning Result.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
public interface IPipelineBehavior<in TRequest>
{
    /// <summary>
    /// Executes behavior.
    /// </summary>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <param name="next">
    /// Next pipeline delegate.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    Task<Result> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result>> next);
}

/// <summary>
/// Defines pipeline behavior contract for
/// requests returning Result&lt;TValue&gt;.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
/// <typeparam name="TValue">
/// Result value type.
/// </typeparam>
public interface IPipelineBehavior<in TRequest, TValue>
{
    /// <summary>
    /// Executes behavior.
    /// </summary>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <param name="next">
    /// Next pipeline delegate.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    Task<Result<TValue>> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result<TValue>>> next);
}