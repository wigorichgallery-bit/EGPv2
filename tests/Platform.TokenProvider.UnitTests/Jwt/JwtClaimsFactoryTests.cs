// ===========================================
// File Location :
// tests/Platform.TokenProvider.UnitTests/Jwt/
// JwtClaimsFactoryTests.cs
// ===========================================
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.TokenProvider.Jwt;

namespace Platform.TokenProvider.UnitTests.Jwt;

public sealed class JwtClaimsFactoryTests
{
    [Fact]
    public void Create_Should_CreateCoreClaims()
    {
        var request = CreateRequest();
        var sut = new JwtClaimsFactory();

        var claims = sut.Create(request);

        claims.Should().ContainSingle(
            x => x.Type == JwtRegisteredClaimNames.Sub &&
                 x.Value == request.UserId.ToString());

        claims.Should().ContainSingle(
            x => x.Type == ClaimTypes.Name &&
                 x.Value == request.Username);

        claims.Should().ContainSingle(
            x => x.Type == ClaimTypes.Email &&
                 x.Value == request.Email);

        claims.Should().ContainSingle(
            x => x.Type == "security_stamp" &&
                 x.Value == request.SecurityStamp);
    }

    [Fact]
    public void Create_Should_CreateOneClaimPerRole()
    {
        var request = CreateRequest();
        var sut = new JwtClaimsFactory();

        var claims = sut.Create(request);

        claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .Should()
            .BeEquivalentTo(request.Roles);
    }

    [Fact]
    public void Create_Should_CreateOneClaimPerPermission()
    {
        var request = CreateRequest();
        var sut = new JwtClaimsFactory();

        var claims = sut.Create(request);

        claims
            .Where(x => x.Type == "permission")
            .Select(x => x.Value)
            .Should()
            .BeEquivalentTo(request.Permissions);
    }

    [Fact]
    public void Create_Should_IgnoreBlankRolesAndPermissions()
    {
        var request = new TokenGenerationRequest(
            Guid.NewGuid(),
            "john.doe",
            "john@example.com",
            "STAMP",
            new[] { "Administrator", "", "   " },
            new[] { "USER.READ", "", "   " });

        var sut = new JwtClaimsFactory();

        var claims = sut.Create(request);

        claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Should()
            .ContainSingle()
            .Which.Value
            .Should()
            .Be("Administrator");

        claims
            .Where(x => x.Type == "permission")
            .Should()
            .ContainSingle()
            .Which.Value
            .Should()
            .Be("USER.READ");
    }

    [Fact]
    public void Create_Should_Throw_When_Request_Is_Null()
    {
        var sut = new JwtClaimsFactory();

        Action action = () => sut.Create(null!);

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("request");
    }

    private static TokenGenerationRequest CreateRequest()
    {
        return new TokenGenerationRequest(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "john.doe",
            "john@example.com",
            "SECURITY-STAMP",
            new[] { "Administrator", "Auditor" },
            new[] { "USER.READ", "USER.WRITE" });
    }
}
