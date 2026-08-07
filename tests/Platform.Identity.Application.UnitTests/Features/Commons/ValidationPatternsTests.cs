using System.Text.RegularExpressions;
using FluentAssertions;
using Platform.Identity.Application.Features.Common;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Common;

/// <summary>
/// Unit tests for <see cref="ValidationPatterns"/>.
/// </summary>
public sealed class ValidationPatternsTests
{
    private static readonly Regex E164Regex =
        new(
            ValidationPatterns.E164Phone,
            RegexOptions.CultureInvariant);

    /// <summary>
    /// Verifies the E.164 pattern accepts valid phone numbers.
    /// </summary>
    [Theory]
    [InlineData("+6281234567890")]
    [InlineData("+14155552671")]
    [InlineData("+447911123456")]
    [InlineData("+819012345678")]
    [InlineData("+33123456789")]
    public void E164Phone_Should_Match_Valid_Phone_Numbers(
        string phoneNumber)
    {
        // Act
        var result = E164Regex.IsMatch(phoneNumber);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the E.164 pattern rejects invalid phone numbers.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("6281234567890")]
    [InlineData("081234567890")]
    [InlineData("+")]
    [InlineData("+0")]
    [InlineData("+0123456789")]
    [InlineData("+62-81234567890")]
    [InlineData("+62 81234567890")]
    [InlineData("+62abc123")]
    [InlineData("+12345678901234567")] // > 15 digits
    public void E164Phone_Should_Not_Match_Invalid_Phone_Numbers(
        string phoneNumber)
    {
        // Act
        var result = E164Regex.IsMatch(phoneNumber);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the E.164 pattern constant is unchanged.
    /// </summary>
    [Fact]
    public void E164Phone_Should_Have_Expected_Pattern()
    {
        ValidationPatterns.E164Phone
            .Should()
            .Be(@"^\+[1-9]\d{1,14}$");
    }
}