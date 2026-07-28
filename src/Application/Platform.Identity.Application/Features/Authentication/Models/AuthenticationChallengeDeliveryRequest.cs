// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationChallengeDeliveryRequest.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents a request to deliver an authentication
/// challenge to an end user.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Encapsulate all information required to deliver an
/// authentication challenge.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provide a stable contract between the application layer
/// and authentication delivery implementations.
/// </description>
/// </item>
/// </list>
/// </summary>
/// <param name="Challenge">
/// Authentication challenge.
/// </param>
/// <param name="User">
/// User receiving the authentication challenge.
/// </param>
/// <param name="PlainTextSecret">
/// Plaintext authentication secret to deliver.
/// </param>
public sealed record AuthenticationChallengeDeliveryRequest(
    AuthenticationChallenge Challenge,
    UserAccount User,
    string PlainTextSecret);