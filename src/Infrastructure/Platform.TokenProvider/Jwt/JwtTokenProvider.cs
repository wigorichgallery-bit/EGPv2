// ===========================================
// File Location :
// src/Infrastructure/Platform.TokenProvider/
// Jwt/JwtTokenProvider.cs
// ===========================================
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.TokenProvider.Configuration;

namespace Platform.TokenProvider.Jwt;

/// <summary>
/// Generates JWT access tokens and opaque refresh tokens.
/// </summary>
public sealed class JwtTokenProvider : ITokenService
{
    private const string TokenType = "Bearer";

    private readonly JwtOptions _options;
    private readonly JwtClaimsFactory _claimsFactory;
    private readonly SigningCredentials _signingCredentials;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenProvider"/> class.
    /// </summary>
    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        JwtClaimsFactory claimsFactory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(claimsFactory);

        _options =
            ValidateOptions(
                options.Value);

        _claimsFactory =
            claimsFactory;

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _options.SecretKey));

        _signingCredentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
    }

    /// <inheritdoc />
    public Task<AuthenticationTokenDto> GenerateTokenAsync(
        TokenGenerationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        cancellationToken.ThrowIfCancellationRequested();

        var nowUtc =
            DateTime.UtcNow;

        var expiresAtUtc =
            nowUtc.AddMinutes(
                _options.AccessTokenLifetimeMinutes);

        var claims =
            _claimsFactory.Create(
                request);

        var token =
            new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: nowUtc,
                expires: expiresAtUtc,
                signingCredentials: _signingCredentials);

        var accessToken =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        var refreshToken =
            CreateRefreshToken();

        var expiresIn =
            checked(
                (int)
                TimeSpan.FromMinutes(
                    _options.AccessTokenLifetimeMinutes)
                    .TotalSeconds);

        return Task.FromResult(
            new AuthenticationTokenDto(
                accessToken,
                refreshToken,
                TokenType,
                expiresIn,
                expiresAtUtc));
    }

    private static string CreateRefreshToken()
    {
        var bytes =
            RandomNumberGenerator.GetBytes(
                32);

        return
            Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
    }

    private static JwtOptions ValidateOptions(
        JwtOptions options)
    {
        ArgumentNullException.ThrowIfNull(
            options);

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            throw new InvalidOperationException(
                "JWT issuer is required.");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            throw new InvalidOperationException(
                "JWT audience is required.");
        }

        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            throw new InvalidOperationException(
                "JWT secret key is required.");
        }

        if (
            Encoding.UTF8.GetByteCount(
                options.SecretKey) < 32)
        {
            throw new InvalidOperationException(
                "JWT secret key must contain at least 32 bytes.");
        }

        if (options.AccessTokenLifetimeMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT access token lifetime must be greater than zero.");
        }

        if (options.RefreshTokenLifetimeDays <= 0)
        {
            throw new InvalidOperationException(
                "JWT refresh token lifetime must be greater than zero.");
        }

        return options;
    }
}
