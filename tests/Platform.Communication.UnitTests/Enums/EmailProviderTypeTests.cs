using Platform.Communication.Enums;

namespace Platform.Communication.UnitTests.Enums;

/// <summary>
/// Contains unit tests for <see cref="EmailProviderType"/>.
/// </summary>
public sealed class EmailProviderTypeTests
{
    /// <summary>
    /// Verifies that each enumeration member
    /// has the expected numeric value.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedNumericValues()
    {
        // Arrange / Act / Assert
        ((int)EmailProviderType.Smtp).Should().Be(0);
        ((int)EmailProviderType.MicrosoftGraph).Should().Be(1);
        ((int)EmailProviderType.SendGrid).Should().Be(2);
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// all expected members.
    /// </summary>
    [Fact]
    public void Enum_Should_ContainExpectedMembers()
    {
        // Arrange
        EmailProviderType[] values = Enum.GetValues<EmailProviderType>();

        // Act / Assert
        values.Should().BeEquivalentTo(
        [
            EmailProviderType.Smtp,
            EmailProviderType.MicrosoftGraph,
            EmailProviderType.SendGrid
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
        string[] names = Enum.GetNames<EmailProviderType>();

        // Act / Assert
        names.Should().Equal(
            nameof(EmailProviderType.Smtp),
            nameof(EmailProviderType.MicrosoftGraph),
            nameof(EmailProviderType.SendGrid));
    }

    /// <summary>
    /// Verifies that the enumeration contains
    /// exactly three members.
    /// </summary>
    [Fact]
    public void Enum_Should_HaveExpectedMemberCount()
    {
        // Arrange / Act
        EmailProviderType[] values = Enum.GetValues<EmailProviderType>();

        // Assert
        values.Should().HaveCount(3);
    }

    /// <summary>
    /// Verifies that a valid integer value
    /// can be cast to the corresponding enumeration.
    /// </summary>
    [Theory]
    [InlineData(0, EmailProviderType.Smtp)]
    [InlineData(1, EmailProviderType.MicrosoftGraph)]
    [InlineData(2, EmailProviderType.SendGrid)]
    public void Cast_Should_ReturnExpectedEnum_When_ValueIsValid(
        int value,
        EmailProviderType expected)
    {
        // Arrange

        // Act
        EmailProviderType actual = (EmailProviderType)value;

        // Assert
        actual.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that each enumeration member
    /// can be converted to its numeric value.
    /// </summary>
    [Theory]
    [InlineData(EmailProviderType.Smtp, 0)]
    [InlineData(EmailProviderType.MicrosoftGraph, 1)]
    [InlineData(EmailProviderType.SendGrid, 2)]
    public void Cast_Should_ReturnExpectedInteger_When_EnumIsConverted(
        EmailProviderType value,
        int expected)
    {
        // Arrange

        // Act
        int actual = (int)value;

        // Assert
        actual.Should().Be(expected);
    }
}