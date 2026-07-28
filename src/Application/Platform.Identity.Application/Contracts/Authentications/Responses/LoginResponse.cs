// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentication/Responses/
// LoginResponse.cs
// ===========================================

using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Platform.Identity.Application.Contracts.Authentication.Enums;

namespace Platform.Identity.Application.Contracts.Authentication.Responses;

/// <summary>
/// Represents the result of an authentication attempt.
///
/// <para>
/// This immutable application contract is returned by the
/// login use case after the authentication workflow has
/// completed.
///
/// Depending on the authentication outcome, the response
/// may either:
/// </para>
///
/// <list type="bullet">
/// <item>
/// <description>
/// Return a successfully generated authentication token.
/// </description>
/// </item>
/// <item>
/// <description>
/// Indicate that an additional authentication challenge
/// must be completed before authentication can continue.
/// </description>
/// </item>
/// <item>
/// <description>
/// Indicate that authentication cannot continue because of
/// account state or security policy.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This record represents an application contract only.
/// It contains no business logic or authentication rules.
/// </para>
///
/// <para>
/// State Contract:
/// </para>
///
/// <list type="bullet">
/// <item>
/// <description>
/// When <see cref="Status"/> is
/// <see cref="AuthenticationStatus.Success"/>,
/// <see cref="Token"/> is populated and every challenge
/// property is <see langword="null"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// When <see cref="Status"/> is
/// <see cref="AuthenticationStatus.ChallengeRequired"/>,
/// <see cref="Token"/> is <see langword="null"/> and the
/// challenge-related properties are populated.
/// </description>
/// </item>
/// </list>
/// </summary>
/// <param name="Status">
/// Indicates the authentication workflow result.
/// </param>
/// <param name="Token">
/// The generated authentication token.
///
/// This value is populated only when
/// <see cref="Status"/> equals
/// <see cref="AuthenticationStatus.Success"/>.
/// </param>
/// <param name="ChallengeId">
/// The identifier of the authentication challenge.
///
/// This value is populated only when
/// <see cref="Status"/> equals
/// <see cref="AuthenticationStatus.ChallengeRequired"/>.
/// </param>
/// <param name="ChallengeType">
/// The authentication challenge mechanism required to
/// continue authentication.
///
/// This value is populated only when
/// <see cref="Status"/> equals
/// <see cref="AuthenticationStatus.ChallengeRequired"/>.
/// </param>
/// <param name="ChallengePurpose">
/// Indicates the business purpose of the authentication
/// challenge.
///
/// This value is populated only when
/// <see cref="Status"/> equals
/// <see cref="AuthenticationStatus.ChallengeRequired"/>.
/// </param>
/// <param name="ChallengeExpiresAtUtc">
/// Specifies the UTC timestamp when the authentication
/// challenge expires.
///
/// This value is populated only when
/// <see cref="Status"/> equals
/// <see cref="AuthenticationStatus.ChallengeRequired"/>.
/// </param>
public sealed record LoginResponse(
    AuthenticationStatus Status,
    AuthenticationTokenDto? Token,
    Guid? ChallengeId,
    AuthenticationChallengeType? ChallengeType,
    AuthenticationChallengePurpose? ChallengePurpose,
    DateTime? ChallengeExpiresAtUtc);