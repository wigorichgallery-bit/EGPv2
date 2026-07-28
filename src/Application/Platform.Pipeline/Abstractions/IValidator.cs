// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/IValidator.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines the base validation contract
/// for application requests.
///
/// Responsibility:
/// - Validate request objects.
/// - Return deterministic validation results.
/// - Remain independent from infrastructure.
///
/// Architectural Rules:
/// - Validation only.
/// - No repository access.
/// - No database access.
/// - No business logic.
/// - Stateless.
///
/// Thread Safety:
/// - Implementations should be stateless.
///
/// Complexity:
/// - Implementation dependent.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
public interface IValidator<in TRequest>
{
    /// <summary>
    /// Validates the specified request.
    /// </summary>
    /// <param name="request">
    /// Request instance.
    /// </param>
    /// <returns>
    /// Validation result.
    /// </returns>
    ValidationResult Validate(
        TRequest request);
}