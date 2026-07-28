// ===========================================
// File Location :
// src/Web/Platform.WebApi/Constants/HttpHeaderNames.cs
// ===========================================
namespace Platform.WebApi.Constants;

/// <summary>
/// Provides well-known HTTP header names
/// used throughout Platform.WebApi.
///
/// Responsibility:
/// - Centralize HTTP header constants.
/// - Prevent magic strings.
/// - Ensure consistency across middleware.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class HttpHeaderNames
{
    /// <summary>
    /// Correlation identifier header.
    /// </summary>
    public const string CorrelationId =
        "X-Correlation-ID";

    /// <summary>
    /// Response execution time header.
    /// </summary>
    public const string ResponseTime =
        "X-Response-Time";
}