// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationChallengeBuildResult.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents the result of building an authentication
/// challenge.
///
/// <para>
/// Encapsulates the fully initialized
/// <see cref="AuthenticationChallenge"/> aggregate together
/// with the plaintext authentication secret required for
/// challenge delivery.
/// </para>
///
/// <para>
/// The plaintext secret exists only for the duration of the
/// current application workflow and must never be persisted.
/// </para>
/// </summary>
/// <param name="Challenge">
/// Fully initialized authentication challenge aggregate.
/// </param>
/// <param name="PlainTextSecret">
/// Plaintext authentication secret to be delivered to the
/// user.
/// </param>
public sealed record AuthenticationChallengeBuildResult(
    AuthenticationChallenge Challenge,
    string PlainTextSecret);