// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Authentication/
// IAuthenticationChallengeBuilder.cs
// ===========================================

using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Builds authentication challenges for application
/// authentication workflows.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Construct fully initialized authentication challenge
/// aggregates.
/// </description>
/// </item>
/// <item>
/// <description>
/// Coordinate authentication challenge construction using
/// application services and domain factories.
/// </description>
/// </item>
/// <item>
/// <description>
/// Preserve the plaintext authentication secret required
/// for authentication challenge delivery.
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
/// Must create aggregates exclusively through
/// <see cref="AuthenticationChallenge.Create"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist aggregates.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not dispatch authentication challenges.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not orchestrate authentication workflows.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Design Notes:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Centralizes authentication challenge construction to
/// ensure consistent aggregate creation across all
/// authentication workflows.
/// </description>
/// </item>
/// <item>
/// <description>
/// Implementations are responsible for determining the
/// authentication challenge type, generating the
/// authentication secret, calculating expiration, invoking
/// the domain aggregate factory, and preserving the
/// plaintext secret required for delivery.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Thread Safety:
/// Implementations should be thread-safe when their
/// dependencies are thread-safe.
/// </para>
/// </summary>
public interface IAuthenticationChallengeBuilder
{
    /// <summary>
    /// Builds a new authentication challenge for the specified
    /// user and authentication purpose.
    /// </summary>
    /// <param name="user">
    /// The user requiring an authentication challenge.
    /// </param>
    /// <param name="purpose">
    /// The purpose for which the authentication challenge is
    /// being created.
    /// </param>
    /// <returns>
    /// An <see cref="AuthenticationChallengeBuildResult"/>
    /// containing the fully initialized
    /// <see cref="AuthenticationChallenge"/> aggregate and
    /// the plaintext authentication secret required for
    /// delivery.
    /// </returns>
    AuthenticationChallengeBuildResult Build(
        UserAccount user,
        AuthenticationChallengePurpose purpose);
}