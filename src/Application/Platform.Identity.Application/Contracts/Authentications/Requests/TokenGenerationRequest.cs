// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentications/Requests/
// TokenGenerationRequest.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Requests;

/// <summary>
/// Represents the information required to generate authentication tokens.
///
/// <para>
/// This request is consumed by <c>ITokenService</c>
/// to generate an authentication token for a successfully
/// authenticated user.
/// </para>
///
/// <para>
/// Responsibilities:
/// <list type="bullet">
/// <item><description>Provide the authenticated user's unique identifier.</description></item>
/// <item><description>Provide the authenticated user's username.</description></item>
/// <item><description>Provide the authenticated user's email address.</description></item>
/// <item><description>Provide the authenticated user's assigned roles.</description></item>/// 
/// <item><description>Provide the authenticated user's effective permissions.</description></item>
/// </list>
/// </para>
///
/// <para>
/// This record is an immutable application contract.
/// It contains no business logic, validation logic,
/// or infrastructure dependency.
/// </para>
/// </summary>
/// <param name="UserId">
/// The unique identifier of the authenticated user.
/// </param>
/// <param name="Username">
/// The username of the authenticated user.
/// </param>
/// <param name="Email">
/// The email address of the authenticated user.
/// </param>
/// <param name="Roles">
/// The ordered read-only collection of roles assigned
/// to the authenticated user.
/// </param>
/// <param name="Permissions">
/// The ordered read-only collection of effective permissions
/// of the authenticated user.
/// </param>
public sealed record TokenGenerationRequest(
    Guid UserId,
    string Username,
    string Email,
    string SecurityStamp,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);