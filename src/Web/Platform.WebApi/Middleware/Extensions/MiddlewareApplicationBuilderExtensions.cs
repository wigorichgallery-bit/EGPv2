// ===========================================
// File Location :
// src/Web/Platform.WebApi/Middleware/
// MiddlewareApplicationBuilderExtensions.cs
// ===========================================
//
// REFACTOR BLOCK
//
// Reason:
// Centralize enterprise middleware pipeline.
//
// Affected Module:
// Platform.WebApi
//
// Breaking Change:
// NO
//
// Version:
// 1.0.0
// ===========================================
namespace Platform.WebApi.Middleware;

/// <summary>
/// Provides enterprise middleware pipeline
/// registration.
///
/// Responsibility:
/// - Register enterprise middleware.
/// - Preserve middleware execution order.
/// - Keep Program.cs minimal.
///
/// Architectural Rules:
/// - Composition Root only.
/// - No business logic.
/// - No infrastructure logic.
/// - No framework configuration.
///
/// Thread Safety:
/// - Stateless.
/// </summary>
public static class MiddlewareApplicationBuilderExtensions
{
    /// <summary>
    /// Registers all enterprise middleware.
    ///
    /// Registration Order:
    /// 1. Exception handling.
    /// 2. Correlation identifier.
    /// 3. Request logging.
    /// 4. Request timing.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="app">
    /// Application builder.
    /// </param>
    /// <returns>
    /// Updated application builder.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="app"/>
    /// is null.
    /// </exception>
    public static IApplicationBuilder
        UseEnterpriseMiddleware(
            this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(
            app);

        app.UseMiddleware<
            ExceptionHandlingMiddleware>();

        app.UseMiddleware<
            CorrelationIdMiddleware>();

        app.UseMiddleware<
            RequestLoggingMiddleware>();

        app.UseMiddleware<
            RequestTimingMiddleware>();

        return app;
    }
}