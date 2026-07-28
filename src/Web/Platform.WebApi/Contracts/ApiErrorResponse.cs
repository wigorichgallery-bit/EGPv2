// ===========================================
// File Location :
// src/Web/Platform.WebApi/Contracts/ApiErrorResponse.cs
// ===========================================
namespace Platform.WebApi.Contracts;

/// <summary>
/// Represents a standardized API error response.
///
/// Responsibility:
/// - Encapsulate error information.
/// - Provide consistent error payload.
/// - Support production diagnostics.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
///
/// Complexity:
/// O(1)
/// </summary>
/// <param name="Code">
/// Application error code.
/// </param>
/// <param name="Message">
/// Human-readable error message.
/// </param>
/// <param name="TraceId">
/// HTTP request trace identifier.
/// </param>
public sealed record ApiErrorResponse(
    string Code,
    string Message,
    string TraceId);