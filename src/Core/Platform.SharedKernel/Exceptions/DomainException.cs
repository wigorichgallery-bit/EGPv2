// ===========================================
// File Location : src/Core/Platform.SharedKernel/Exceptions/DomainException.cs
// ===========================================

namespace Platform.SharedKernel.Exceptions;

/// <summary>
/// Represents domain invariant violation.
/// 
/// Responsibility:
/// - Thrown only when aggregate invariant is broken.
/// - Never used for validation or authorization.
/// 
/// Policy:
/// - Application layer converts to Result.Failure.
/// </summary>
public sealed class DomainException : Exception
{
    /// <summary>
    /// Gets domain error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes new DomainException.
    /// </summary>
    /// <param name="errorCode">Domain error code.</param>
    /// <param name="message">Error message.</param>
    public DomainException(string errorCode, string message)
        : base(message)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("ErrorCode required.", nameof(errorCode));

        ErrorCode = errorCode;
    }
}