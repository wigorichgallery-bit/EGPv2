// ===========================================
// File Location :
// src/Core/Platform.SharedKernel/Results/ErrorType.cs
// ===========================================
namespace Platform.SharedKernel.Results;

/// <summary>
/// Represents the semantic classification
/// of an application error.
///
/// Responsibility:
/// - Classify application failures.
/// - Decouple application errors from
///   transport protocols.
/// - Support HTTP mapping in presentation layer.
///
/// Architectural Rules:
/// - SharedKernel abstraction.
/// - No ASP.NET dependency.
/// - No infrastructure dependency.
/// - No transport-specific behavior.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable enumeration.
///
/// Complexity:
/// O(1)
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// No error.
    /// </summary>
    None = 0,

    /// <summary>
    /// Input validation failure.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// Authentication failure.
    /// </summary>
    Unauthorized = 2,

    /// <summary>
    /// Authorization failure.
    /// </summary>
    Forbidden = 3,

    /// <summary>
    /// Requested resource was not found.
    /// </summary>
    NotFound = 4,

    /// <summary>
    /// Business conflict.
    /// </summary>
    Conflict = 5,

    /// <summary>
    /// Unexpected internal error.
    /// </summary>
    Internal = 6
}