// ===========================================
// File Location :
// tests/Platform.TokenProvider.UnitTests/Jwt/
// JwtTokenProviderTests.cs
// ===========================================
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Options;

using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.TokenProvider.Configuration;
using Platform.TokenProvider.Jwt;

namespace Platform.TokenProvider.UnitTests.Jwt;

public sealed class JwtTokenProviderTests
{
    private const string SecretKey =
        "EGPv2-Test-Secret-Key-Minimum-32-Bytes!!";

    [Fact]
    public async Task GenerateTokenAsync_Should_CreateJwtAccessToken()
    {
        var sut = CreateSut();

        var result =
            await sut.GenerateTokenAsync(
                CreateRequest());

        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.TokenType.Should().Be("Bearer");
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.ExpiresIn.Should().Be(900);
        result.ExpiresAtUtc.Should().BeAfter(DateTime.UtcNow);

        var jwt =
            new JwtSecurityTokenHandler()
                .ReadJwtToken(result.AccessToken);

        jwt.Issuer.Should().Be("EnterpriseGovernancePlatform");
        jwt.Audiences.Should()
            .ContainSingle()
            .Which.Should()
            .Be("EnterpriseGovernancePlatform");
    }

    [Fact]
    public async Task GenerateTokenAsync_Should_CreateExpectedClaims()
    {
        var request = CreateRequest();
        var sut = CreateSut();

        var result =
            await sut.GenerateTokenAsync(request);

        var jwt =
            new JwtSecurityTokenHandler()
                .ReadJwtToken(result.AccessToken);

        jwt.Claims.Should().Contain(
            x => x.Type == JwtRegisteredClaimNames.Sub &&
                 x.Value == request.UserId.ToString());

        jwt.Claims.Should().Contain(
            x => x.Type == "security_stamp" &&
                 x.Value == request.SecurityStamp);

        jwt.Claims.Should().Contain(
            x => x.Type == System.Security.Claims.ClaimTypes.Role &&
                 x.Value == "Administrator");

        jwt.Claims.Should().Contain(
            x => x.Type == "permission" &&
                 x.Value == "USER.READ");
    }

    [Fact]
    public async Task GenerateTokenAsync_Should_CreateUniqueOpaqueRefreshTokens()
    {
        var sut = CreateSut();
        var request = CreateRequest();

        var first = await sut.GenerateTokenAsync(request);
        var second = await sut.GenerateTokenAsync(request);

        first.RefreshToken.Should().NotBe(second.RefreshToken);
        first.RefreshToken.Length.Should().BeGreaterThan(40);
        first.RefreshToken.Should().NotContain("+");
        first.RefreshToken.Should().NotContain("/");
        first.RefreshToken.Should().NotContain("=");
    }

    [Fact]
    public async Task GenerateTokenAsync_Should_Throw_When_Request_Is_Null()
    {
        var sut = CreateSut();

        Func<Task> action = () =>
            sut.GenerateTokenAsync(null!);

        await action.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("request");
    }

    [Fact]
    public async Task GenerateTokenAsync_Should_Throw_When_CancellationRequested()
    {
        var sut = CreateSut();
        using var source = new CancellationTokenSource();
        source.Cancel();

        Func<Task> action = () =>
            sut.GenerateTokenAsync(
                CreateRequest(),
                source.Token);

        await action.Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void Constructor_Should_Throw_When_SecretKey_Is_TooShort()
    {
        var options = new JwtOptions
        {
            Issuer = "issuer",
            Audience = "audience",
            SecretKey = "short",
            AccessTokenLifetimeMinutes = 15,
            RefreshTokenLifetimeDays = 30
        };

        Action action = () =>
            new JwtTokenProvider(
                Options.Create(options),
                new JwtClaimsFactory());

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("JWT secret key must contain at least 32 bytes.");
    }

    private static JwtTokenProvider CreateSut()
    {
        return new JwtTokenProvider(
            Options.Create(
                new JwtOptions
                {
                    Issuer = "EnterpriseGovernancePlatform",
                    Audience = "EnterpriseGovernancePlatform",
                    SecretKey = SecretKey,
                    AccessTokenLifetimeMinutes = 15,
                    RefreshTokenLifetimeDays = 30
                }),
            new JwtClaimsFactory());
    }

    private static TokenGenerationRequest CreateRequest()
    {
        return new TokenGenerationRequest(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "john.doe",
            "john@example.com",
            "SECURITY-STAMP",
            new[] { "Administrator" },
            new[] { "USER.READ", "USER.WRITE" });
    }
}
