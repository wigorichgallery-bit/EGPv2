// ===========================================
// File Location :
// tests/Platform.TokenProvider.UnitTests/
// DependencyInjection/TokenProviderServiceCollectionExtensionsTests.cs
// ===========================================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Platform.Identity.Application.Abstractions.Security;
using Platform.TokenProvider.DependencyInjection;
using Platform.TokenProvider.Jwt;

namespace Platform.TokenProvider.UnitTests.DependencyInjection;

public sealed class TokenProviderServiceCollectionExtensionsTests
{
    [Fact]
    public void AddTokenProvider_Should_RegisterExpectedServices()
    {
        var configuration =
            CreateConfiguration();

        var services =
            new ServiceCollection();

        services.AddTokenProvider(
            configuration);

        using var provider =
            services.BuildServiceProvider();

        provider
            .GetRequiredService<JwtClaimsFactory>()
            .Should()
            .NotBeNull();

        provider
            .GetRequiredService<ITokenService>()
            .Should()
            .BeOfType<JwtTokenProvider>();

        services.Should().ContainSingle(
            x => x.ServiceType == typeof(JwtBearerEventsHandler));
    }

    [Fact]
    public void AddTokenProvider_Should_Throw_When_Configuration_Is_Null()
    {
        var services = new ServiceCollection();

        Action action = () =>
            services.AddTokenProvider(null!);

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("configuration");
    }

    private static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Jwt:Issuer"] = "EnterpriseGovernancePlatform",
                    ["Jwt:Audience"] = "EnterpriseGovernancePlatform",
                    ["Jwt:SecretKey"] =
                        "EGPv2-Test-Secret-Key-Minimum-32-Bytes!!",
                    ["Jwt:AccessTokenLifetimeMinutes"] = "15",
                    ["Jwt:RefreshTokenLifetimeDays"] = "30"
                })
            .Build();
    }
}
