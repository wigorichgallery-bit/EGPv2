// ===========================================
// File Location :
// src/Web/Platform.WebApi/Constants/HttpItemKeys.cs
// ===========================================
namespace Platform.WebApi.Constants;

/// <summary>
/// Provides HttpContext.Items keys
/// shared across middleware.
///
/// Responsibility:
/// - Centralize HttpContext item names.
/// - Prevent magic strings.
/// - Ensure middleware consistency.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class HttpItemKeys
{
    /// <summary>
    /// Correlation identifier item key.
    /// </summary>
    public const string CorrelationId =
        "CorrelationId";

    /// <summary>
    /// Elapsed request execution time.
    /// </summary>
    public const string ElapsedMilliseconds =
        "__ElapsedMilliseconds";

}