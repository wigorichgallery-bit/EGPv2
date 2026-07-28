// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/Contracts/Authentications/Dtos/
// AuthenticationTokenDto.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Dtos;

/// <summary>   
/// Represents the result of a successful authentication operation.
///
/// <para>
/// This data transfer object is returned by the authentication subsystem
/// after a user has been successfully authenticated.
/// </para>
///
/// <para>
/// Responsibilities:
/// <list type="bullet">
/// <item><description>Carry the generated access token.</description></item>
/// <item><description>Carry the generated refresh token.</description></item>
/// <item><description>Provide the token type.</description></item>
/// <item><description>Provide the token lifetime in seconds.</description></item>
/// <item><description>Provide the UTC expiration timestamp.</description></item>
/// </list>
/// </para>
///
/// <para>
/// This record is an immutable application contract and contains
/// no business logic or validation logic.
/// </para>
/// </summary>
/// <param name="AccessToken">
/// The generated JWT access token.
/// </param>
/// <param name="RefreshToken">
/// The generated refresh token.
/// </param>
/// <param name="TokenType">
/// The token type returned to the client.
/// The default value is typically <c>Bearer</c>.
/// </param>
/// <param name="ExpiresIn">
/// The lifetime of the access token expressed in seconds.
/// </param>
/// <param name="ExpiresAtUtc">
/// The UTC date and time when the access token expires.
/// </param>
public sealed record AuthenticationTokenDto(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int ExpiresIn,
    DateTime ExpiresAtUtc);