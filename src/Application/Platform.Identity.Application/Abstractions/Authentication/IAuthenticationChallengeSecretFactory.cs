// ===========================================
// File Location:
// src/Application/
// Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeSecretFactory.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Creates authentication challenge secrets.
///
/// <para>
/// Responsible for generating challenge secrets for
/// authentication workflows.
/// </para>
///
/// <para>
/// Implementations may generate one-time passwords,
/// shared secrets, or other authentication secrets
/// depending on the challenge type.
/// </para>
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
/// Does not persist data.
/// </description>
/// </item>
/// <item>
/// <description>
/// Does not deliver challenges.
/// </description>
/// </item>
/// <item>
/// <description>
/// Does not construct aggregates.
/// </description>
/// </item>
/// </list>
/// </summary>
public interface IAuthenticationChallengeSecretFactory
{
    /// <summary>
    /// Creates an authentication challenge secret.
    /// </summary>
    /// <param name="type">
    /// Authentication challenge type.
    /// </param>
    /// <returns>
    /// Generated authentication challenge secret.
    /// </returns>
    AuthenticationChallengeSecretResult Create(
        AuthenticationChallengeType type);
}