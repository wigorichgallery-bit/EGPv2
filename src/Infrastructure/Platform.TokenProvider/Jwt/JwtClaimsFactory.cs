// ===========================================
// File Location :
// src/Infrastructure/Platform.TokenProvider/
// Jwt/JwtClaimsFactory.cs
// ===========================================
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Platform.Identity.Application.Contracts.Authentication.Requests;

namespace Platform.TokenProvider.Jwt;

/// <summary>
/// Creates JWT claims from validated authentication data.
/// </summary>
public sealed class JwtClaimsFactory
{
    private const string SecurityStampClaimType = "security_stamp";
    private const string PermissionClaimType = "permission";

    /// <summary>
    /// Creates the claims required by the EGPv2 JWT contract.
    /// </summary>
    /// <param name="request">Validated token-generation data.</param>
    /// <returns>The generated JWT claims.</returns>
    public IReadOnlyCollection<Claim> Create(
        TokenGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claims =
            new List<Claim>
            {
                new(
                    JwtRegisteredClaimNames.Sub,
                    request.UserId.ToString()),
                new(
                    ClaimTypes.Name,
                    request.Username),
                new(
                    ClaimTypes.Email,
                    request.Email),
                new(
                    SecurityStampClaimType,
                    request.SecurityStamp)
            };

        foreach (var role in request.Roles)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }
        }

        foreach (var permission in request.Permissions)
        {
            if (!string.IsNullOrWhiteSpace(permission))
            {
                claims.Add(
                    new Claim(
                        PermissionClaimType,
                        permission));
            }
        }

        return claims;
    }
}
