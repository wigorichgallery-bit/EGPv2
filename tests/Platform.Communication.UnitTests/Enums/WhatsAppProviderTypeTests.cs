using Platform.Communication.Enums;

namespace Platform.Communication.UnitTests.Enums;

/// <summary>
/// Contains unit tests for <see cref="WhatsAppProviderType"/>.
/// </summary>
public sealed class WhatsAppProviderTypeTests
{
    /// <summary>
    /// Verifies that each enumeration member
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedNumericValues()
    {
        // Arrange / Act / Assert
        ((int)WhatsAppProviderType.MetaCloud).Should().Be(0);
        ((int)WhatsAppProviderType.Twilio).Should().Be(1);
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// all expected members.
    /// </summary>
    [Fact]
    public void Enum_Should_ContainExpectedMembers()
    {
        // Arrange
        WhatsAppProviderType[] values =
            Enum.GetValues<WhatsAppProviderType>();

        // Act / Assert
        values.Should().BeEquivalentTo(
        [
            WhatsAppProviderType.MetaCloud,
            WhatsAppProviderType.Twilio
        ],
        options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Verifies that the enumeration member names
    /// are exposed correctly.
    /// </summary>
    [Fact]
    public void Enum_Should_ReturnExpectedNames()
    {
        // Arrange
        string[] names =
            Enum.GetNames<WhatsAppProviderType>();

        // Act / Assert
        names.Should().Equal(
            nameof(WhatsAppProviderType.MetaCloud),
            nameof(WhatsAppProviderType.Twilio));
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// exactly two members.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedMemberCount()
    {
        // Arrange / Act
        WhatsAppProviderType[] values =
            Enum.GetValues<WhatsAppProviderType>();

        // Assert
        values.Should().HaveCount(2);
    }

    /// <summary>
    /// Verifies that a valid integer value
    /// can be cast to the corresponding enumeration.
    /// </summary>
    /// <param name="value">
    /// Numeric enumeration value.
    /// </param>
    /// <param name="expected">
    /// Expected enumeration member.
    /// </param>
    [Theory]
    [InlineData(0, WhatsAppProviderType.MetaCloud)]
    [InlineData(1, WhatsAppProviderType.Twilio)]
    public void Cast_Should_ReturnExpectedEnum_When_ValueIsValid(
        int value,
        WhatsAppProviderType expected)
    {
        // Arrange

        // Act
        WhatsAppProviderType actual =
            (WhatsAppProviderType)value;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that each enumeration member
    /// can be converted to its numeric value.
    /// </summary>
    /// <param name="value">
    /// Enumeration member.
    /// </param>
    /// <param name="expected">
    /// Expected numeric value.
    /// </param>
    [Theory]
    [InlineData(WhatsAppProviderType.MetaCloud, 0)]
    [InlineData(WhatsAppProviderType.Twilio, 1)]
    public void Cast_Should_ReturnExpectedInteger_When_EnumIsConverted(
        WhatsAppProviderType value,
        int expected)
    {
        // Arrange

        // Act
        int actual = (int)value;

        // Assert
        actual.Should().Be(expected);
    }
}