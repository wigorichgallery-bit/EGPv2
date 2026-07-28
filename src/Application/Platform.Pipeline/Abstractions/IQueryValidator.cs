// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/IQueryValidator.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines validation contract for
/// application queries.
///
/// Responsibility:
/// - Validate query input.
/// - Prevent invalid read requests.
/// - Return deterministic validation results.
///
/// Architectural Rules:
/// - Validation only.
/// - No repository access.
/// - No database access.
/// - No infrastructure dependency.
/// - No business logic.
/// - No domain mutation.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Implementations should be stateless.
///
/// Complexity:
/// - Implementation dependent.
/// </summary>
/// <typeparam name="TQuery">
/// Query type.
/// </typeparam>
public interface IQueryValidator<in TQuery> : IValidator<TQuery>
{
    // /// <summary>
    // /// Validates the specified query.
    // /// </summary>
    // /// <param name="query"> 
    // /// Query instance.
    // /// </param>
    // /// <returns>
    // /// Validation result.
    // /// </returns>
    // ValidationResult Validate(
    //     TQuery query);
}