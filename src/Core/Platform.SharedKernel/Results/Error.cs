// ===========================================
// File Location : src/Core/Platform.SharedKernel/Results/Error.cs
// ===========================================

namespace Platform.SharedKernel.Results;

/// <summary>
/// Represents an application error.
/// 
/// Responsibility:
/// - Encapsulates error code and message.
/// - Used by Result pattern.
/// 
/// Invariants:
/// - Code must not be empty.
/// - Message must not be empty.
/// - None represents absence of error.
/// 
/// Side Effects:
/// - Immutable.
/// </summary>
public sealed class Error
{
    /// <summary>
    /// Gets error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes error instance.
    /// 
    /// Validation Logic:
    /// - Code must not be null or whitespace.
    /// - Message must not be null or whitespace.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="message">Error message.</param>
    /// <exception cref="ArgumentException">Thrown if invalid input.</exception>
    public Error(string code, string message)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Error code required.", nameof(code));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message required.", nameof(message));

        Code = code;
        Message = message;
    }

    /// <summary>
    /// Represents absence of error.
    /// 
    /// Business Rule:
    /// - Used only for successful Result.
    /// - Must not be used for failure.
    /// </summary>
    public static readonly Error None = new Error("NONE", "No error");
}