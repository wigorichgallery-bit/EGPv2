// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Configuration/Authentication/AuthenticationOptions.cs
// AuthenticationChallengePurpose.cs
// ===========================================

namespace Platform.Identity.Application.Configuration.Authentication;

/// <summary>
/// Authentication workflow configuration.
/// </summary>
public sealed class AuthenticationOptions
{
    /// <summary>
    /// Maximum failed login attempts before account lockout.
    /// </summary>
    public int LockoutThreshold { get; init; } = 5;

    /// <summary>
    /// Account lockout duration.
    /// </summary>
    public TimeSpan LockoutDuration { get; init; }
        = TimeSpan.FromMinutes(15);
}