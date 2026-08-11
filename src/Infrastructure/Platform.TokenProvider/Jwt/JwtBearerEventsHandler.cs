// ===========================================
// File Location :
// src/Infrastructure/Platform.TokenProvider/
// Jwt/JwtBearerEventsHandler.cs
// ===========================================
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace Platform.TokenProvider.Jwt;

/// <summary>
/// Handles JWT bearer authentication events without introducing
/// application or domain logic into the authentication pipeline.
/// </summary>
public sealed class JwtBearerEventsHandler : JwtBearerEvents
{
    private readonly ILogger<JwtBearerEventsHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="JwtBearerEventsHandler"/> class.
    /// </summary>
    public JwtBearerEventsHandler(
        ILogger<JwtBearerEventsHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    /// <inheritdoc />
    public override Task AuthenticationFailed(
        AuthenticationFailedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _logger.LogWarning(
            context.Exception,
            "JWT bearer authentication failed.");

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task TokenValidated(
        TokenValidatedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _logger.LogDebug(
            "JWT bearer token validated.");

        return Task.CompletedTask;
    }
}
