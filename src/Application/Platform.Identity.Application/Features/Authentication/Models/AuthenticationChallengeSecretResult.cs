// ===========================================
// File Location:
// src/Application/
// Platform.Identity.Application/
// Features/
// Authentication/
// Models/
// AuthenticationChallengeSecretResult.cs
// ===========================================

using Platform.Identity.Domain.ValueObjects;

namespace Platform.Identity.Application.Features.Authentication.Models;

/// <summary>
/// Represents the generated authentication challenge secret.
///
/// <para>
/// Encapsulates both the protected secret persisted within
/// the <c>AuthenticationChallenge</c> aggregate and the
/// corresponding plaintext secret required for challenge
/// delivery or provisioning.
/// </para>
///
/// <para>
/// Usage:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// For OTP-based challenges (Email, SMS, WhatsApp),
/// <see cref="Secret"/> contains the hashed OTP while
/// <see cref="PlainTextSecret"/> contains the OTP that
/// must be delivered to the user.
/// </description>
/// </item>
/// <item>
/// <description>
/// For TOTP challenges,
/// <see cref="Secret"/> and
/// <see cref="PlainTextSecret"/> both represent the
/// generated shared secret.
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
/// Belongs to the Authentication feature.
/// </description>
/// </item>
/// <item>
/// <description>
/// Not a Domain model.
/// </description>
/// </item>
/// <item>
/// <description>
/// Not an API contract.
/// </description>
/// </item>
/// <item>
/// <description>
/// Used internally during authentication workflows.
/// </description>
/// </item>
/// </list>
/// </summary>
/// <param name="Secret">
/// The protected secret stored by the
/// <c>AuthenticationChallenge</c> aggregate.
/// </param>
/// <param name="PlainTextSecret">
/// The plaintext secret required for challenge delivery
/// or TOTP provisioning.
/// </param>
public sealed record AuthenticationChallengeSecretResult(
    ChallengeSecret Secret,
    string PlainTextSecret);