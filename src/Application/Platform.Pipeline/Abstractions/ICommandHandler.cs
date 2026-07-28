// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/ICommandHandler.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines the contract for handling
/// application commands that do not
/// return a value.
///
/// Responsibility:
/// - Execute command.
/// - Return Result.
/// - Remain independent from infrastructure.
///
/// Architectural Rules:
/// - Command side only.
/// - No infrastructure dependency.
/// - No persistence implementation.
///
/// Thread Safety:
/// - Implementations should be stateless.
///
/// Complexity:
/// - Implementation dependent.
/// </summary>
/// <typeparam name="TCommand">
/// Command type.
/// </typeparam>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Executes the specified command.
    /// </summary>
    /// <param name="command">
    /// Command instance.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Command execution result.
    /// </returns>
    Task<Result> ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken = default);
}