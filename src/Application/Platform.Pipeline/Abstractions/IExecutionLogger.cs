// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IExecutionLogger.cs
// ===========================================
using Platform.Pipeline.Models;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines execution logging contract.
///
/// Responsibility:
/// - Persist execution log entries.
/// - Support operational monitoring.
/// - Support audit and diagnostics.
///
/// Side Effects:
/// - Implementation dependent.
/// </summary>
public interface IExecutionLogger
{
    /// <summary>
    /// Writes execution log entry.
    /// </summary>
    /// <param name="entry">
    /// Log entry.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    Task LogAsync(
        ExecutionLogEntry entry,
        CancellationToken cancellationToken);
}