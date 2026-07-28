// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Validation/ValidationFailureFactory.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Validation;

/// <summary>
/// Creates Result failure objects from validation failures.
///
/// Responsibility:
/// - Convert ValidationError collection into SharedKernel Error.
/// - Create Result failure.
/// - Create Result&lt;T&gt; failure.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
internal static class ValidationFailureFactory
{
    private const string ValidationFailedCode =
        "VALIDATION.FAILED";

    /// <summary>
    /// Creates Result failure from validation errors.
    /// </summary>
    /// <param name="errors">
    /// Validation errors.
    /// </param>
    /// <returns>
    /// Failure result.
    /// </returns>
    public static Result CreateFailure(
        IReadOnlyCollection<ValidationError> errors)
    {
        Guard.AgainstNull(
            errors,
            nameof(errors));

        if (errors.Count == 0)
        {
            throw new ArgumentException(
                "At least one validation error is required.",
                nameof(errors));
        }

        return Result.Failure(
            CreateError(errors));
    }

    /// <summary>
    /// Creates Result&lt;T&gt; failure from validation errors.
    /// </summary>
    /// <typeparam name="T">
    /// Result value type.
    /// </typeparam>
    /// <param name="errors">
    /// Validation errors.
    /// </param>
    /// <returns>
    /// Failure result.
    /// </returns>
    public static Result<T> CreateFailure<T>(
        IReadOnlyCollection<ValidationError> errors)
    {
        Guard.AgainstNull(
            errors,
            nameof(errors));

        if (errors.Count == 0)
        {
            throw new ArgumentException(
                "At least one validation error is required.",
                nameof(errors));
        }

        return Result<T>.Failure(
            CreateError(errors));
    }

    /// <summary>
    /// Creates SharedKernel error.
    /// </summary>
    private static Error CreateError(
        IReadOnlyCollection<ValidationError> errors)
    {
        var builder = new StringBuilder();

        foreach (var error in errors)
        {
            if (builder.Length > 0)
            {
                builder.Append("; ");
            }

            builder.Append(error.Message);
        }

        return new Error(
            ValidationFailedCode,
            builder.ToString());
    }
}