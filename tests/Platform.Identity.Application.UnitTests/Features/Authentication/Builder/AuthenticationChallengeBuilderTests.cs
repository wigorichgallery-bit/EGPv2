
using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Features.Authentication.Builders;
using Platform.Identity.Application.Features.Authentication.Mapping;
using Platform.Identity.Application.Features.Authentication.Models;
using Platform.Identity.Application.UnitTests.Fixtures;
using Platform.Identity.Domain.Enums;
using Platform.SharedKernel.Abstractions;
using Xunit;
using Platform.Identity.Application.Abstractions.Common;
using Platform.Identity.Application.Configuration.Authentication;
namespace Platform.Identity.Application.UnitTests.Features.Authentication.Builders;

/// <summary>
/// Contains unit tests for
/// <see cref="AuthenticationChallengeBuilder"/>.
/// </summary>
public sealed class AuthenticationChallengeBuilderTests
{
    private readonly Mock<IGuidGenerator> _guidGenerator = new();

    private readonly Mock<IClock> _clock = new();

    private readonly Mock<IAuthenticationChallengeSecretFactory>
        _secretFactory = new();

    private readonly AuthenticationChallengeOptions _options =
        new()
        {
            LoginChallengeLifetime = TimeSpan.FromMinutes(5)
        };

    /// <summary>
    /// Verifies the constructor throws when the guid generator
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_GuidGenerator_Is_Null()
    {
        // Act
        Action action =
            () => new AuthenticationChallengeBuilder(
                null!,
                _clock.Object,
                _options,
                _secretFactory.Object);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("guidGenerator");
    }

    /// <summary>
    /// Verifies the constructor throws when the clock
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_Clock_Is_Null()
    {
        // Act
        Action action =
            () => new AuthenticationChallengeBuilder(
                _guidGenerator.Object,
                null!,
                _options,
                _secretFactory.Object);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("clock");
    }

    /// <summary>
    /// Verifies the constructor throws when the options
    /// are null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_Options_Are_Null()
    {
        // Act
        Action action =
            () => new AuthenticationChallengeBuilder(
                _guidGenerator.Object,
                _clock.Object,
                null!,
                _secretFactory.Object);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("options");
    }

    /// <summary>
    /// Verifies the constructor throws when the secret
    /// factory is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_SecretFactory_Is_Null()
    {
        // Act
        Action action =
            () => new AuthenticationChallengeBuilder(
                _guidGenerator.Object,
                _clock.Object,
                _options,
                null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("challengeSecretFactory");
    }

    /// <summary>
    /// Verifies Build throws when the user is null.
    /// </summary>
    [Fact]
    public void Build_Should_Throw_When_User_Is_Null()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Action action =
            () => sut.Build(
                null!,
                AuthenticationChallengePurpose.Login);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("user");
    }

    /// <summary>
    /// Verifies Build creates a fully initialized
    /// authentication challenge.
    /// </summary>
    [Fact]
    public void Build_Should_Create_Authentication_Challenge()
    {
        // Arrange
        var user =
            UserAccountFixture.CreateTotpUser();

        var challengeId =
            Guid.NewGuid();

        var now =
            new DateTime(
                2026,
                1,
                1,
                12,
                0,
                0,
                DateTimeKind.Utc);

        var secret =
            ChallengeSecretFixture.Create();

        var secretResult =
            new AuthenticationChallengeSecretResult(
                secret,
                "123456");

        _guidGenerator
            .Setup(x => x.Create())
            .Returns(challengeId);

        _clock
            .SetupGet(x => x.UtcNow)
            .Returns(now);

        // _secretFactory
        //     .Setup(x => x.Create(
        //         AuthenticationChallengeTypeResolver.Resolve(
        //             user.MFAMethod)))
        //     .Returns(secretResult);

        _secretFactory
            .Setup(x => x.Create(It.IsAny<AuthenticationChallengeType>()))
            .Returns(secretResult);

        var sut = CreateSut();

        // Act
        var result =
            sut.Build(
                user,
                AuthenticationChallengePurpose.Login);

        // Assert

        result.Should().NotBeNull();

        result.PlainTextSecret.Should().Be("123456");

        result.Challenge.Id.Should().Be(challengeId);

        result.Challenge.UserId.Should().Be(user.Id);

        result.Challenge.ChallengeType.Should().Be(
            AuthenticationChallengeTypeResolver.Resolve(
                user.MFAMethod));

        result.Challenge.Purpose.Should().Be(
            AuthenticationChallengePurpose.Login);

        result.Challenge.ChallengeSecret.Should().Be(secret);

        result.Challenge.Status.Should().Be(
            AuthenticationChallengeStatus.Pending);

        result.Challenge.CreatedAtUtc.Should().Be(now);

        result.Challenge.ExpiresAtUtc.Should().Be(
            now.Add(_options.LoginChallengeLifetime));

        _guidGenerator.Verify(
            x => x.Create(),
            Times.Once);

        _clock.VerifyGet(
            x => x.UtcNow,
            Times.Once);

        _secretFactory.Verify(
            x => x.Create(
                AuthenticationChallengeTypeResolver.Resolve(
                    user.MFAMethod)),
            Times.Once);

        _guidGenerator.VerifyNoOtherCalls();
        _clock.VerifyNoOtherCalls();
        _secretFactory.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private AuthenticationChallengeBuilder CreateSut()
    {
        return new AuthenticationChallengeBuilder(
            _guidGenerator.Object,
            _clock.Object,
            _options,
            _secretFactory.Object);
    }
}