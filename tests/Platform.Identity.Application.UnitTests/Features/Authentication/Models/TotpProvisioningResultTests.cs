using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Contains unit tests for <see cref="TotpProvisioningResult"/>.
/// </summary>
public sealed class TotpProvisioningResultTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        const string provisioningUri =
            "otpauth://totp/Platform:john.doe@example.com?secret=ABCDEF123456&issuer=Platform";

        const string manualEntryKey =
            "ABCDEF123456";

        // Act
        var result = new TotpProvisioningResult(
            provisioningUri,
            manualEntryKey);

        // Assert
        result.ProvisioningUri.Should().Be(provisioningUri);
        result.ManualEntryKey.Should().Be(manualEntryKey);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var left = new TotpProvisioningResult(
            "otpauth://totp/example",
            "ABCDEF123456");

        var right = new TotpProvisioningResult(
            "otpauth://totp/example",
            "ABCDEF123456");

        // Assert
        left.Should().Be(right);
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies records with different values are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var left = new TotpProvisioningResult(
            "otpauth://totp/example-1",
            "ABCDEF123456");

        var right = new TotpProvisioningResult(
            "otpauth://totp/example-2",
            "ZYXWVU654321");

        // Assert
        left.Should().NotBe(right);
        (left == right).Should().BeFalse();
    }

    /// <summary>
    /// Verifies deconstruction returns all property values.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_Return_All_Property_Values()
    {
        // Arrange
        var result = new TotpProvisioningResult(
            "otpauth://totp/example",
            "ABCDEF123456");

        // Act
        var (
            provisioningUri,
            manualEntryKey) = result;

        // Assert
        provisioningUri.Should().Be("otpauth://totp/example");
        manualEntryKey.Should().Be("ABCDEF123456");
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var result = new TotpProvisioningResult(
            "otpauth://totp/example",
            "ABCDEF123456");

        // Act
        var text = result.ToString();

        // Assert
        text.Should().Contain(nameof(TotpProvisioningResult.ProvisioningUri));
        text.Should().Contain(nameof(TotpProvisioningResult.ManualEntryKey));
    }
}