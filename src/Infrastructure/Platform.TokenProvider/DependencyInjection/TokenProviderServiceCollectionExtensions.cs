// ===========================================
// File Location :
// src/Infrastructure/Platform.TokenProvider/
// DependencyInjection/
// TokenProviderServiceCollectionExtensions.cs
// ===========================================
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Platform.Identity.Application.Abstractions.Security;
using Platform.TokenProvider.Configuration;
using Platform.TokenProvider.Jwt;

namespace Platform.TokenProvider.DependencyInjection;

/// <summary>
/// Provides dependency-injection registration for the JWT token provider.
/// </summary>
public static class TokenProviderServiceCollectionExtensions
{
    /// <summary>
    /// Registers JWT token generation and bearer authentication.
    /// </summary>
    /// <param name="services">The application service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddTokenProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var jwtSection =
            configuration.GetSection(
                JwtOptions.SectionName);

        services.Configure<JwtOptions>(
            jwtSection);

        var options =
            jwtSection.Get<JwtOptions>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{JwtOptions.SectionName}' is required.");

        ValidateOptions(options);

        services.AddSingleton<JwtClaimsFactory>();

        services.AddScoped<
            ITokenService,
            JwtTokenProvider>();

        services.AddSingleton<
            JwtBearerEventsHandler>();

        var signingKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    options.SecretKey));

        services
            .AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = options.Issuer,

                            ValidateAudience = true,
                            ValidAudience = options.Audience,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey,

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,

                            NameClaimType =
                                System.Security.Claims.ClaimTypes.Name,

                            RoleClaimType =
                                System.Security.Claims.ClaimTypes.Role
                        };

                    jwtOptions.EventsType =
                        typeof(JwtBearerEventsHandler);
                });

        return services;
    }

    private static void ValidateOptions(
        JwtOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

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
    }
}
