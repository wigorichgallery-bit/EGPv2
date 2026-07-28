// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/ICommandHandlerT.cs
// ===========================================
using Platform.SharedKernel.Results;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines the contract for handling
/// application commands returning a value.
///
/// Responsibility:
/// - Execute command.
/// - Return strongly typed Result.
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
/// <typeparam name="TResult">
/// Result value type.
/// </typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
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
    Task<Result<TResult>> ExecuteAsync(
        TCommand command,
        CancellationToken cancellationToken = default);
}