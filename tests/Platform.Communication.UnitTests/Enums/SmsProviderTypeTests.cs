using Platform.Communication.Enums;

namespace Platform.Communication.UnitTests.Enums;

/// <summary>
/// Contains unit tests for <see cref="SmsProviderType"/>.
/// </summary>
public sealed class SmsProviderTypeTests
{
    /// <summary>
    /// Verifies that each enumeration member
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedNumericValues()
    {
        // Arrange / Act / Assert
        ((int)SmsProviderType.Twilio).Should().Be(0);
        ((int)SmsProviderType.Vonage).Should().Be(1);
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// all expected members.
    /// </summary>
    [Fact]
    public void Enum_Should_ContainExpectedMembers()
    {
        // Arrange
        SmsProviderType[] values = Enum.GetValues<SmsProviderType>();

        // Act / Assert
        values.Should().BeEquivalentTo(
        [
            SmsProviderType.Twilio,
            SmsProviderType.Vonage
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
        string[] names = Enum.GetNames<SmsProviderType>();

        // Act / Assert
        names.Should().Equal(
            nameof(SmsProviderType.Twilio),
            nameof(SmsProviderType.Vonage));
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// exactly two members.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedMemberCount()
    {
        // Arrange / Act
        SmsProviderType[] values = Enum.GetValues<SmsProviderType>();

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
    [InlineData(0, SmsProviderType.Twilio)]
    [InlineData(1, SmsProviderType.Vonage)]
    public void Cast_Should_ReturnExpectedEnum_When_ValueIsValid(
        int value,
        SmsProviderType expected)
    {
        // Arrange

        // Act
        SmsProviderType actual = (SmsProviderType)value;

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
    [InlineData(SmsProviderType.Twilio, 0)]
    [InlineData(SmsProviderType.Vonage, 1)]
    public void Cast_Should_ReturnExpectedInteger_When_EnumIsConverted(
        SmsProviderType value,
        int expected)
    {
        // Arrange

        // Act
        int actual = (int)value;

        // Assert
        actual.Should().Be(expected);
    }
}