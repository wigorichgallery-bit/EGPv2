// ===========================================
// File Location :
// src/Web/Platform.WebApi/Logging/NullExecutionLogger.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.Pipeline.Models;

namespace Platform.WebApi.Logging;

/// <summary>
/// Default no-op implementation of
/// <see cref="IExecutionLogger"/>.
///
/// Responsibility:
/// - Satisfy pipeline logging dependency.
/// - Ignore execution log entries.
/// - Allow pipeline execution without
///   a concrete logging provider.
///
/// Usage:
/// - Development environments.
/// - Testing environments.
/// - Initial platform bootstrap.
/// - Placeholder until operational
///   logging implementation exists.
///
/// Architectural Rules:
/// - No persistence.
/// - No infrastructure dependency.
/// - No external logging framework.
/// - No exception throwing.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent usage.
///
/// Notes:
/// This implementation intentionally
/// discards all log entries.
///
/// Future implementations may include:
/// - SerilogExecutionLogger
/// - DatabaseExecutionLogger
/// - OpenTelemetryExecutionLogger
/// - ElasticExecutionLogger
/// - SeqExecutionLogger
/// </summary>
public sealed class NullExecutionLogger
    : IExecutionLogger
{
    /// <summary>
    /// Writes an execution log entry.
    ///
    /// Behavior:
    /// - Intentionally performs no action.
    /// - Never throws exceptions.
    /// - Always completes successfully.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="entry">
    /// Execution log entry.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Completed task.
    /// </returns>
    public Task LogAsync(
        ExecutionLogEntry entry,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}