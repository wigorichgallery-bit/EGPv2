// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IPipelineExecutor.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Executes requests through the configured
/// pipeline chain.
///
/// Responsibility:
/// - Execute request pipeline.
/// - Invoke registered behaviors.
/// - Invoke final handler.
/// - Return operation result.
///
/// Architectural Rules:
/// - Pipeline orchestration only.
/// - No business logic.
/// - No infrastructure logic.
/// - No persistence logic.
///
/// Side Effects:
/// - None.
/// </summary>
public interface IPipelineExecutor
{
    /// <summary>
    /// Executes a request returning Result.
    /// </summary>
    /// <typeparam name="TRequest">
    /// Request type.
    /// </typeparam>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="handler">
    /// Final request handler.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    Task<Result> ExecuteAsync<TRequest>(
        TRequest request,
        Func<Task<Result>> handler,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes a request returning Result&lt;TValue&gt;.
    /// </summary>
    /// <typeparam name="TRequest">
    /// Request type.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Result value type.
    /// </typeparam>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <param name="handler">
    /// Final request handler.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Operation result.
    /// </returns>
    Task<Result<TValue>> ExecuteAsync<TRequest, TValue>(
        TRequest request,
        Func<Task<Result<TValue>>> handler,
        CancellationToken cancellationToken);
}