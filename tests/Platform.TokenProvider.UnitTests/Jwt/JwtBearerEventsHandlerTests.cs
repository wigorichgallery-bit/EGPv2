// ===========================================
// File Location :
// tests/Platform.TokenProvider.UnitTests/Jwt/
// JwtBearerEventsHandlerTests.cs
// ===========================================
using Microsoft.Extensions.Logging;

using Platform.TokenProvider.Jwt;

namespace Platform.TokenProvider.UnitTests.Jwt;

public sealed class JwtBearerEventsHandlerTests
{
    [Fact]
    public void Constructor_Should_Throw_When_Logger_Is_Null()
    {
        Action action = () =>
            new JwtBearerEventsHandler(null!);

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    [Fact]
    public void Constructor_Should_Create_When_Logger_Is_Provided()
    {
        using var factory =
            LoggerFactory.Create(_ => { });

        var sut =
            new JwtBearerEventsHandler(
                factory.CreateLogger<JwtBearerEventsHandler>());

        sut.Should().NotBeNull();
    }
}
