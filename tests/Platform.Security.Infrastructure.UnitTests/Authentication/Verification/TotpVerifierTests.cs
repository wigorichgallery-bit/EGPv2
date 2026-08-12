
using Microsoft.Extensions.Options;
using Platform.Security.Infrastructure.Totp;
using Platform.Identity.Application.Abstractions.Security;

using Platform.Identity.Application.Configuration.Authentication;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Totp;

/// <summary>
/// Unit tests for <see cref="TotpVerifier"/>.
/// </summary>
public sealed class TotpVerifierTests
{
    private static TotpVerifier CreateSut(
        Mock<ITotpCodeGenerator> generator,
        int timeStepSeconds = 30,
        int allowedTimeSteps = 1)
    {
        var options = Options.Create(
            new TotpOptions
            {
                TimeStepSeconds = timeStepSeconds,
                AllowedTimeSteps = allowedTimeSteps
            });

        return new TotpVerifier(
            generator.Object,
            options);
    }

    /// <summary>
    /// Verifies constructor throws when generator is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldAllowNullGenerator()
    {
        // Arrange
        var options = Options.Create(
            new TotpOptions
            {
                TimeStepSeconds = 30,
                AllowedTimeSteps = 1
            });

        // Act
        Action act = () => new TotpVerifier(
            null!,
            options);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Verifies constructor throws when options is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowNullReferenceException_WhenOptionsIsNull()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        // Act
        Action act = () =>
            new TotpVerifier(
                generator.Object,
                null!);

        // Assert
        act.Should()
            .Throw<NullReferenceException>();
    }

    /// <summary>
    /// Verifies constructor rejects zero timestep.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenTimeStepIsZero()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var options =
            Options.Create(
                new TotpOptions
                {
                    TimeStepSeconds = 0
                });

        // Act
        Action act = () =>
            new TotpVerifier(
                generator.Object,
                options);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies constructor rejects negative allowed windows.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAllowedWindowIsNegative()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var options =
            Options.Create(
                new TotpOptions
                {
                    AllowedTimeSteps = -1
                });

        // Act
        Action act = () =>
            new TotpVerifier(
                generator.Object,
                options);

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies Verify rejects null secret.
    /// </summary>
    [Fact]
    public void Verify_ShouldThrowArgumentException_WhenSecretIsNull()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var sut =
            CreateSut(generator);

        // Act
        Action act = () =>
            sut.Verify(
                null!,
                "123456",
                DateTime.UtcNow);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies Verify rejects whitespace secret.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Verify_ShouldThrowArgumentException_WhenSecretIsWhitespace(
        string secret)
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var sut =
            CreateSut(generator);

        // Act
        Action act = () =>
            sut.Verify(
                secret,
                "123456",
                DateTime.UtcNow);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies Verify rejects null code.
    /// </summary>
    [Fact]
    public void Verify_ShouldThrowArgumentException_WhenCodeIsNull()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var sut =
            CreateSut(generator);

        // Act
        Action act = () =>
            sut.Verify(
                "SECRET",
                null!,
                DateTime.UtcNow);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies Verify rejects whitespace code.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Verify_ShouldThrowArgumentException_WhenCodeIsWhitespace(
        string code)
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        var sut =
            CreateSut(generator);

        // Act
        Action act = () =>
            sut.Verify(
                "SECRET",
                code,
                DateTime.UtcNow);

        // Assert
        act.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies current window succeeds.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnTrue_WhenCurrentWindowMatches()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        generator
            .Setup(x => x.GenerateCode(
                It.IsAny<string>(),
                It.IsAny<DateTime>()))
            .Returns("654321");

        var sut =
            CreateSut(generator);

        // Act
        var result =
            sut.Verify(
                "SECRET",
                "654321",
                DateTime.UtcNow);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies mismatch returns false.
    /// </summary>
    [Fact]
    public void Verify_ShouldReturnFalse_WhenCodeDoesNotMatch()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        generator
            .Setup(x => x.GenerateCode(
                It.IsAny<string>(),
                It.IsAny<DateTime>()))
            .Returns("111111");

        var sut =
            CreateSut(generator);

        // Act
        var result =
            sut.Verify(
                "SECRET",
                "222222",
                DateTime.UtcNow);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies generator is called once for every validation window.
    /// </summary>
    [Fact]
    public void Verify_ShouldCallGenerator_ForEachValidationWindow()
    {
        // Arrange
        var generator =
            new Mock<ITotpCodeGenerator>();

        generator
            .Setup(x => x.GenerateCode(
                It.IsAny<string>(),
                It.IsAny<DateTime>()))
            .Returns("999999");

        var sut =
            CreateSut(
                generator,
                allowedTimeSteps: 2);

        // Act
        sut.Verify(
            "SECRET",
            "000000",
            DateTime.UtcNow);

        // Assert
        generator.Verify(
            x => x.GenerateCode(
                It.IsAny<string>(),
                It.IsAny<DateTime>()),
            Times.Exactly(5));
    }
}