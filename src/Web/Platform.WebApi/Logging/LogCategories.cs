namespace Platform.WebApi.Logging;

/// <summary>
/// Provides standardized logging categories
/// used throughout Platform.WebApi.
///
/// Responsibility:
/// - Centralize logger category names.
/// - Prevent duplicated category strings.
/// - Ensure logging consistency.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
internal static class LogCategories
{
    /// <summary>
    /// HTTP pipeline category.
    /// </summary>
    public const string HttpPipeline =
        "Platform.WebApi.HttpPipeline";

    /// <summary>
    /// Authentication category.
    /// </summary>
    public const string Authentication =
        "Platform.WebApi.Authentication";

    /// <summary>
    /// Authorization category.
    /// </summary>
    public const string Authorization =
        "Platform.WebApi.Authorization";

    /// <summary>
    /// Controller category.
    /// </summary>
    public const string Controller =
        "Platform.WebApi.Controller";
}