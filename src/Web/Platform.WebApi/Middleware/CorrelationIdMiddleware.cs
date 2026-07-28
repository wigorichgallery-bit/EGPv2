// ===========================================
// File Location :
// src/Web/Platform.WebApi/Middleware/
// CorrelationIdMiddleware.cs
// ===========================================
// REFACTOR BLOCK
//
// Reason:
// Introduce enterprise-grade correlation identifier middleware.
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
using Platform.WebApi.Constants;

namespace Platform.WebApi.Middleware;

/// <summary>
/// Provides centralized correlation identifier
/// management for every HTTP request.
///
/// Responsibility:
/// - Resolve correlation identifier.
/// - Validate incoming correlation identifier.
/// - Store correlation identifier.
/// - Add correlation identifier to response.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No logging.
/// - No authentication.
/// - No authorization.
/// - No infrastructure implementation.
///
/// Side Effects:
/// - Stores correlation identifier inside
///   <see cref="HttpContext.Items"/>.
/// - Adds correlation identifier to the
///   response header.
///
/// Thread Safety:
/// - Stateless.
/// - Safe for concurrent requests.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    /// <summary>
    /// Maximum allowed correlation identifier
    /// length.
    /// </summary>
    private const int MaximumCorrelationIdLength =
        128;

    /// <summary>
    /// Pipeline continuation delegate.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of
    /// <see cref="CorrelationIdMiddleware"/>.
    ///
    /// Responsibility:
    /// - Store immutable dependencies.
    ///
    /// Failure:
    /// - Throws when dependency is null.
    /// </summary>
    /// <param name="next">
    /// Next middleware.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="next"/>
    /// is null.
    /// </exception>
    public CorrelationIdMiddleware(
        RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(
            next);

        _next = next;
    }

    /// <summary>
    /// Executes the correlation identifier
    /// middleware.
    ///
    /// Algorithm:
    /// 1. Resolve correlation identifier.
    /// 2. Store into HttpContext.Items.
    /// 3. Add response header.
    /// 4. Continue pipeline.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <returns>
    /// Asynchronous operation.
    /// </returns>
    public async Task InvokeAsync(
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        var correlationId =
            ResolveCorrelationId(
                context);

        StoreCorrelationId(
            context,
            correlationId);

        await _next(context)
            .ConfigureAwait(false);

        SetResponseHeader(
            context,
            correlationId);
    }

    /// <summary>
    /// Resolves the correlation identifier
    /// from the current request.
    ///
    /// Algorithm:
    /// 1. Read request header.
    /// 2. Validate identifier.
    /// 3. Reuse if valid.
    /// 4. Otherwise generate new identifier.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <returns>
    /// Correlation identifier.
    /// </returns>
    private static string ResolveCorrelationId(
        HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        if (context.Request.Headers.TryGetValue(
                HttpHeaderNames.CorrelationId,
                out var values))
        {
            var correlationId =
                values.ToString();

            if (IsValidCorrelationId(
                correlationId))
            {
                return correlationId;
            }
        }

        return Guid.NewGuid()
            .ToString("N");
    }

    /// <summary>
    /// Determines whether the supplied
    /// correlation identifier is valid.
    ///
    /// Validation Rules:
    /// - Must not be null.
    /// - Must not be empty.
    /// - Must not contain only whitespace.
    /// - Length must not exceed the maximum
    ///   supported length.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="correlationId">
    /// Correlation identifier.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the
    /// identifier is valid; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    private static bool IsValidCorrelationId(
        string? correlationId)
    {
        if (string.IsNullOrWhiteSpace(
            correlationId))
        {
            return false;
        }

        return correlationId.Length <=
               MaximumCorrelationIdLength;
    }

    /// <summary>
    /// Stores the correlation identifier
    /// into <see cref="HttpContext.Items"/>.
    ///
    /// Responsibility:
    /// - Make the correlation identifier
    ///   available to downstream middleware.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="correlationId">
    /// Correlation identifier.
    /// </param>
    private static void StoreCorrelationId(
        HttpContext context,
        string correlationId)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            correlationId);

        context.Items[
            HttpItemKeys.CorrelationId] =
            correlationId;
    }

    /// <summary>
    /// Writes the correlation identifier
    /// to the HTTP response header.
    ///
    /// Responsibility:
    /// - Ensure every response exposes the
    ///   same correlation identifier.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="correlationId">
    /// Correlation identifier.
    /// </param>
    private static void SetResponseHeader(
        HttpContext context,
        string correlationId)
    {
        ArgumentNullException.ThrowIfNull(
            context);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            correlationId);

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Headers[
            HttpHeaderNames.CorrelationId] =
            correlationId;
    }
}

