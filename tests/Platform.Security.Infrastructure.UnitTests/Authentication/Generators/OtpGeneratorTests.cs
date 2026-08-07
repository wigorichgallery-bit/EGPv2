using FluentAssertions;
using Platform.Security.Infrastructure.Otp;
using Xunit;

namespace Platform.Security.Infrastructure.UnitTests.Authentication.Otp;

/// <summary>
/// Unit tests for <see cref="OtpGenerator"/>.
/// </summary>
public sealed class OtpGeneratorTests
{
    private readonly OtpGenerator _sut = new();

    /// <summary>
    /// Verifies Generate returns a non-empty OTP.
    /// </summary>
    [Fact]
    public void Generate_ShouldReturnNonEmptyValue()
    {
        // Act
        var otp = _sut.Generate();

        // Assert
        otp.Should().NotBeNullOrWhiteSpace();
    }

    /// <summary>
    /// Verifies Generate returns exactly six digits.
    /// </summary>
    [Fact]
    public void Generate_ShouldReturnSixDigits()
    {
        // Act
        var otp = _sut.Generate();

        // Assert
        otp.Should().HaveLength(6);
    }

    /// <summary>
    /// Verifies Generate returns numeric characters only.
    /// </summary>
    [Fact]
    public void Generate_ShouldContainOnlyNumericCharacters()
    {
        // Act
        var otp = _sut.Generate();

        // Assert
        otp.All(char.IsDigit).Should().BeTrue();
    }

    /// <summary>
    /// Verifies multiple generated OTPs always satisfy the expected format.
    /// </summary>
    [Fact]
    public void Generate_ShouldProduceValidOtpAcrossMultipleInvocations()
    {
        // Act
        var otps = Enumerable
            .Range(0, 100)
            .Select(_ => _sut.Generate());

        // Assert
        otps.All(otp =>
            otp.Length == 6 &&
            otp.All(char.IsDigit))
            .Should()
            .BeTrue();
    }
}