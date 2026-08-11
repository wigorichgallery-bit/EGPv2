// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Enums/MFAMethodTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Enums;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Enums;

/// <summary>
/// Contains unit tests for
/// <see cref="MFAMethod"/>.
/// </summary>
public sealed class MFAMethodTests
{
    #region Enum Value Tests

    [Theory]
    [InlineData(MFAMethod.None, 0)]
    [InlineData(MFAMethod.TOTP, 1)]
    [InlineData(MFAMethod.Email, 2)]
    [InlineData(MFAMethod.SMS, 3)]
    [InlineData(MFAMethod.WhatsApp, 4)]
    public void EnumMember_ShouldHaveExpectedValue(
        MFAMethod value,
        int expected)
    {
        ((int)value).Should().Be(expected);
    }

    #endregion

    #region Enum Definition Tests

    [Fact]
    public void Enum_ShouldContainExpectedNumberOfValues()
    {
        Enum.GetValues<MFAMethod>()
            .Should()
            .HaveCount(5);
    }

    [Fact]
    public void EnumValues_ShouldBeUnique()
    {
        var values =
            Enum.GetValues<MFAMethod>()
                .Cast<int>()
                .ToArray();

        values.Distinct().Should().HaveCount(values.Length);
    }

    [Fact]
    public void EnumNames_ShouldMatchProductionDefinition()
    {
        var expected = new[]
        {
            nameof(MFAMethod.None),
            nameof(MFAMethod.TOTP),
            nameof(MFAMethod.Email),
            nameof(MFAMethod.SMS),
            nameof(MFAMethod.WhatsApp)
        };

        Enum.GetNames<MFAMethod>()
            .Should()
            .Equal(expected);
    }

    [Fact]
    public void EnumValues_ShouldBeSequential()
    {
        Enumerable.Range(0, 5)
            .Should()
            .Equal(
                Enum.GetValues<MFAMethod>()
                    .Cast<int>());
    }

    #endregion
}