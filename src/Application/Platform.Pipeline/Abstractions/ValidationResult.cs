// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Validation/ValidationResult.cs
//
// STEP-7B
// LOCKED
// ===========================================
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Abstractions;
/// <summary>
/// Represents validator execution result.
///
/// RESPONSIBILITY:
/// - Encapsulate validation success state.
/// - Store validation errors.
/// - Provide deterministic validation outcome.
///
/// INVARIANTS:
/// - Success contains zero errors.
/// - Failure contains one or more errors.
///
/// SIDE EFFECTS:
/// - None.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
public sealed class ValidationResult
{
    private ValidationResult(
        bool isValid,
        IReadOnlyCollection<ValidationError> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    /// <summary>
    /// Validation success indicator.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Validation errors.
    /// </summary>
    public IReadOnlyCollection<ValidationError> Errors { get; }

    /// <summary>
    /// Creates successful validation result.
    /// </summary>
    /// <returns>
    /// Successful validation result.
    /// </returns>
    public static ValidationResult Success()
    {
        return new ValidationResult(
            true,
            Array.Empty<ValidationError>());
    }

    /// <summary>
    /// Creates failed validation result.
    /// </summary>
    /// <param name="errors">
    /// Validation errors.
    /// </param>
    /// <returns>
    /// Failed validation result.
    /// </returns>
    public static ValidationResult Failure(
        IReadOnlyCollection<ValidationError> errors)
    {
        Guard.AgainstNull(
            errors,
            nameof(errors));

        if (errors.Count == 0)
        {
            throw new ArgumentException(
                "Validation failure requires at least one error.",
                nameof(errors));
        }

        return new ValidationResult(
            false,
            errors);
    }
}