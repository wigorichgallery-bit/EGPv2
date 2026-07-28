// ===========================================
// File Location :
// src/Web/Platform.WebApi/Constants/MediaTypeNames.cs
// ===========================================
namespace Platform.WebApi.Constants;

/// <summary>
/// Provides well-known HTTP media types
/// used throughout Platform.WebApi.
///
/// Responsibility:
/// - Centralize HTTP media type constants.
/// - Prevent magic strings.
/// - Ensure consistency across middleware,
///   controllers, and filters.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class HttpMediaTypes
{
    /// <summary>
    /// RFC7807 Problem Details JSON media type.
    /// </summary>
    public const string ProblemJson =
        "application/problem+json";

    /// <summary>
    /// Standard JSON media type.
    /// </summary>
    public const string Json =
        "application/json";

    /// <summary>
    /// XML media type.
    /// </summary>
    public const string Xml =
        "application/xml";

    /// <summary>
    /// Plain text media type.
    /// </summary>
    public const string PlainText =
        "text/plain";

    /// <summary>
    /// Binary stream media type.
    /// </summary>
    public const string OctetStream =
        "application/octet-stream";

    /// <summary>
    /// Multipart form data media type.
    /// </summary>
    public const string MultipartFormData =
        "multipart/form-data";
}