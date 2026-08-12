// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Configuration/
// AuthenticationChallengeOptions.cs
// ===========================================

namespace Platform.Identity.Application.Configuration.Authentication;

/// <summary>
/// Represents configuration options used when creating and
/// validating authentication challenges.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Defines challenge expiration policies.
/// </description>
/// </item>
/// <item>
/// <description>
/// Defines retry limitations for challenge verification.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provides configurable authentication challenge behavior
/// without modifying application logic.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Contains configuration only.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not contain business logic.
/// </description>
/// </item>
/// <item>
/// <description>
/// Intended to be bound using the Options pattern.
/// </description>
/// </item>
/// </list>
/// </summary>
public sealed class AuthenticationChallengeOptions
{
    /// <summary>
    /// Gets or initializes the lifetime of an authentication
    /// challenge created for the login workflow.
    /// </summary>
    public TimeSpan LoginChallengeLifetime { get; init; }
        = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or initializes the maximum number of failed
    /// verification attempts allowed before a challenge
    /// becomes locked.
    /// </summary>
    public int MaximumFailedAttempts { get; init; } = 5;
}