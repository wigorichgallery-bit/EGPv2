// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Validation/ValidationError.cs
//
// STEP-7B
// LOCKED
// ===========================================
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Represents a single validation failure.
///
/// RESPONSIBILITY:
/// - Carry validation error code.
/// - Carry validation error message.
/// - Immutable validation failure representation.
///
/// INVARIANTS:
/// - Code must not be empty.
/// - Message must not be empty.
///
/// SIDE EFFECTS:
/// - None.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// Initializes a new validation error.
    /// </summary>
    /// <param name="code">
    /// Unique validation error code.
    /// </param>
    /// <param name="message">
    /// Human-readable validation message.
    /// </param>
    public ValidationError(
        string code,
        string message)
    {
        Guard.AgainstNullOrWhiteSpace(
            code,
            nameof(code));

        Guard.AgainstNullOrWhiteSpace(
            message,
            nameof(message));

        Code = code;
        Message = message;
    }

    /// <summary>
    /// Validation error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Validation error message.
    /// </summary>
    public string Message { get; }
}