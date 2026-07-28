// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/IQueryHandler.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines the contract for executing
/// application query handlers.
///
/// Responsibility:
/// - Execute read-only queries.
/// - Return strongly typed Result.
/// - Remain independent from infrastructure.
///
/// Architectural Rules:
/// - Query only.
/// - No domain mutation.
/// - No transaction.
/// - No UnitOfWork.
/// - No persistence implementation.
///
/// Thread Safety:
/// - Implementations should be stateless.
///
/// Complexity:
/// - Implementation dependent.
/// </summary>
/// <typeparam name="TQuery">
/// Query contract.
/// </typeparam>
/// <typeparam name="TResult">
/// Result value type.
/// </typeparam>
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Executes the specified query.
    /// </summary>
    /// <param name="query">
    /// Query instance.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Query execution result.
    /// </returns>
    Task<Result<TResult>> ExecuteAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}