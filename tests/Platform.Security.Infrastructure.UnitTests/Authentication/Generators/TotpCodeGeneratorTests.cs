using FluentAssertions;
using Microsoft.Extensions.Options;
using Platform.Identity.Application.Configuration.Authentication;
using Platform.Security.Infrastructure.Totp;
using Xunit;
using Platform.Security.Infrastructure.Authentication.Configuration;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Totp;

/// <summary>
/// Unit tests for <see cref="TotpCodeGenerator"/>.
/// </summary>
public sealed class TotpCodeGeneratorTests
{
    private static TotpCodeGenerator CreateSut(
        int digits = 6,
        int timeStepSeconds = 30)
    {
        var options = Options.Create(
            new TotpOptions
            {
                Digits = digits,
                TimeStepSeconds = timeStepSeconds
            });

        return new TotpCodeGenerator(options);
    }

    /// <summary>
    /// Verifies constructor throws when options is null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
    {
        // Arrange
        IOptions<TotpOptions> options = null!;

        // Act
        Action act = () => new TotpCodeGenerator(options);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    /// <summary>
    /// Verifies GenerateCode throws when secret is null.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldThrowArgumentException_WhenSecretIsNull()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Action act = () =>
            sut.GenerateCode(
                null!,
                DateTime.UtcNow);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies GenerateCode throws when secret is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void GenerateCode_ShouldThrowArgumentException_WhenSecretIsWhitespace(
        string secret)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Action act = () =>
            sut.GenerateCode(
                secret,
                DateTime.UtcNow);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies invalid Base32 secret throws FormatException.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldThrowFormatException_WhenSecretIsInvalid()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        Action act = () =>
            sut.GenerateCode(
                "INVALID***",
                DateTime.UtcNow);

        // Assert
        act.Should().Throw<FormatException>();
    }

    /// <summary>
    /// Verifies lowercase Base32 secrets are accepted.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldAcceptLowercaseSecret()
    {
        // Arrange
        var sut = CreateSut();

        const string secret =
            "jbswy3dpehpk3pxp";

        // Act
        var code =
            sut.GenerateCode(
                secret,
                new DateTime(
                    2026,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc));

        // Assert
        code.Should().HaveLength(6);
        code.All(char.IsDigit).Should().BeTrue();
    }

    /// <summary>
    /// Verifies padded Base32 secrets are accepted.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldAcceptPaddedSecret()
    {
        // Arrange
        var sut = CreateSut();

        const string secret =
            "JBSWY3DPEHPK3PXP====";

        // Act
        var code =
            sut.GenerateCode(
                secret,
                DateTime.UtcNow);

        // Assert
        code.Should().HaveLength(6);
        code.All(char.IsDigit).Should().BeTrue();
    }

    /// <summary>
    /// Verifies identical input produces identical codes.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldReturnSameCode_ForSameInput()
    {
        // Arrange
        var sut = CreateSut();

        const string secret =
            "JBSWY3DPEHPK3PXP";

        var now =
            new DateTime(
                2026,
                1,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc);

        // Act
        var first =
            sut.GenerateCode(secret, now);

        var second =
            sut.GenerateCode(secret, now);

        // Assert
        first.Should().Be(second);
    }

    /// <summary>
    /// Verifies different time steps produce different codes.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldReturnDifferentCode_WhenTimeStepChanges()
    {
        // Arrange
        var sut = CreateSut();

        const string secret =
            "JBSWY3DPEHPK3PXP";

        var firstTime =
            new DateTime(
                2026,
                1,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc);

        var secondTime =
            firstTime.AddSeconds(31);

        // Act
        var first =
            sut.GenerateCode(secret, firstTime);

        var second =
            sut.GenerateCode(secret, secondTime);

        // Assert
        first.Should().NotBe(second);
    }

    /// <summary>
    /// Verifies configured digit length is respected.
    /// </summary>
    [Fact]
    public void GenerateCode_ShouldRespectConfiguredDigitLength()
    {
        // Arrange
        var sut =
            CreateSut(digits: 8);

        const string secret =
            "JBSWY3DPEHPK3PXP";

        // Act
        var code =
            sut.GenerateCode(
                secret,
                DateTime.UtcNow);

        // Assert
        code.Should().HaveLength(8);
        code.All(char.IsDigit).Should().BeTrue();
    }
}