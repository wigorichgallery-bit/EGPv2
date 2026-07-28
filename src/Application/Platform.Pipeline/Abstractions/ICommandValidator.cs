// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Validation/ICommandValidator.cs
//
// STEP-7B
// LOCKED
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines application command validation contract.
///
/// RESPONSIBILITY:
/// - Validate command shape.
/// - Return deterministic validation result.
/// - Remain independent from infrastructure.
///
/// INVARIANTS:
/// - Must never access repository.
/// - Must never access database.
/// - Must never perform domain validation.
///
/// SIDE EFFECTS:
/// - None.
///
/// COMPLEXITY:
/// - Implementation dependent.
/// </summary>
/// <typeparam name="TCommand">
/// Command type.
/// </typeparam>
public interface ICommandValidator<in TCommand> : IValidator<TCommand>
{
    // /// <summary>
    // /// Validates command input.
    // /// </summary>
    // /// <param name="command">
    // /// Command instance.
    // /// </param>
    // /// <returns>
    // /// Validation result.
    // /// </returns>
    // ValidationResult Validate(
    //     TCommand command);
}