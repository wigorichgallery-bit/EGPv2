using FluentAssertions;
using Moq;
using Platform.Identity.Application.Abstractions.Security;
using Platform.Identity.Application.Features.Authentication.Factories;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Factories;

/// <summary>
/// Unit tests for
/// <see cref="AuthenticationChallengeSecretFactory"/>.
/// </summary>
public sealed class AuthenticationChallengeSecretFactoryTests
{
    private readonly Mock<IOtpGenerator>
        _otpGenerator = new();

    private readonly Mock<ITotpSecretGenerator>
        _totpSecretGenerator = new();

    private readonly Mock<IPasswordHasher>
        _passwordHasher = new();

    /// <summary>
    /// Verifies the constructor throws when the OTP generator
    /// is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_OtpGenerator_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeSecretFactory(
                null!,
                _totpSecretGenerator.Object,
                _passwordHasher.Object);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("otpGenerator");
    }

    /// <summary>
    /// Verifies the constructor throws when the TOTP secret
    /// generator is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_TotpSecretGenerator_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeSecretFactory(
                _otpGenerator.Object,
                null!,
                _passwordHasher.Object);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("totpSecretGenerator");
    }

    /// <summary>
    /// Verifies the constructor throws when the password
    /// hasher is null.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_PasswordHasher_Is_Null()
    {
        // Act
        Action act = () =>
            new AuthenticationChallengeSecretFactory(
                _otpGenerator.Object,
                _totpSecretGenerator.Object,
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("passwordHasher");
    }

    /// <summary>
    /// Verifies OTP challenge types create hashed challenge
    /// secrets.
    /// </summary>
    [Theory]
    [InlineData(AuthenticationChallengeType.EmailOtp)]
    [InlineData(AuthenticationChallengeType.SmsOtp)]
    [InlineData(AuthenticationChallengeType.WhatsAppOtp)]
    public void Create_Should_Create_OtpSecret(
        AuthenticationChallengeType challengeType)
    {
        // Arrange
        const string otp = "123456";
        const string hash = "HASHED_OTP";

        _otpGenerator
            .Setup(x => x.Generate())
            .Returns(otp);

        _passwordHasher
            .Setup(x => x.Hash(otp))
            .Returns(hash);

        var sut = CreateSut();

        // Act
        var result =
            sut.Create(challengeType);

        // Assert
        result.Should().NotBeNull();

        result.PlainTextSecret
            .Should()
            .Be(otp);

        result.Secret.Value
            .Should()
            .Be(hash);

        _otpGenerator.Verify(
            x => x.Generate(),
            Times.Once);

        _passwordHasher.Verify(
            x => x.Hash(otp),
            Times.Once);

        _totpSecretGenerator.Verify(
            x => x.GenerateSecret(),
            Times.Never);

        _otpGenerator.VerifyNoOtherCalls();
        _passwordHasher.VerifyNoOtherCalls();
        _totpSecretGenerator.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies TOTP challenge types create shared secrets.
    /// </summary>
    [Fact]
    public void Create_Should_Create_TotpSecret()
    {
        // Arrange
        const string secret =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        _totpSecretGenerator
            .Setup(x => x.GenerateSecret())
            .Returns(secret);

        var sut = CreateSut();

        // Act
        var result =
            sut.Create(AuthenticationChallengeType.Totp);

        // Assert
        result.Should().NotBeNull();

        result.PlainTextSecret
            .Should()
            .Be(secret);

        result.Secret.Value
            .Should()
            .Be(secret);

        _totpSecretGenerator.Verify(
            x => x.GenerateSecret(),
            Times.Once);

        _otpGenerator.Verify(
            x => x.Generate(),
            Times.Never);

        _passwordHasher.Verify(
            x => x.Hash(It.IsAny<string>()),
            Times.Never);

        _otpGenerator.VerifyNoOtherCalls();
        _passwordHasher.VerifyNoOtherCalls();
        _totpSecretGenerator.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies unsupported challenge types throw an
    /// exception.
    /// </summary>
    [Fact]
    public void Create_Should_Throw_When_ChallengeType_Is_Unsupported()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Action act = () =>
            sut.Create(
                (AuthenticationChallengeType)999);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithParameterName("challengeType");
    }

    /// <summary>
    /// Creates the system under test.
    /// </summary>
    private AuthenticationChallengeSecretFactory CreateSut()
    {
        return new AuthenticationChallengeSecretFactory(
            _otpGenerator.Object,
            _totpSecretGenerator.Object,
            _passwordHasher.Object);
    }
}