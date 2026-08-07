using Platform.Communication.Validation;

namespace Platform.Communication.UnitTests.Validation;

/// <summary>
/// Unit tests for <see cref="E164PhoneNumberValidator"/>.
/// </summary>
public sealed class E164PhoneNumberValidatorTests
{
    /// <summary>
    /// Verifies that <see cref="E164PhoneNumberValidator.IsValid(string)"/>
    /// returns <c>false</c> when the supplied value is null,
    /// empty, or whitespace.
    /// </summary>
    /// <param name="value">
    /// Invalid phone number.
    /// </param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void IsValid_Should_ReturnFalse_When_ValueIsNullOrWhiteSpace(
        string? value)
    {
        // Arrange

        // Act
        bool result = E164PhoneNumberValidator.IsValid(value!);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="E164PhoneNumberValidator.IsValid(string)"/>
    /// returns <c>true</c> when the supplied value
    /// is a valid E.164 phone number.
    /// </summary>
    /// <param name="value">
    /// Valid E.164 phone number.
    /// </param>
    [Theory]
    [InlineData("+12")]
    [InlineData("+628123456789")]
    [InlineData("+12025550123")]
    [InlineData("+447911123456")]
    [InlineData("+819012345678")]
    [InlineData("+123456789012345")]
    public void IsValid_Should_ReturnTrue_When_ValueIsValidE164(
        string value)
    {
        // Arrange

        // Act
        bool result = E164PhoneNumberValidator.IsValid(value);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="E164PhoneNumberValidator.IsValid(string)"/>
    /// returns <c>false</c> when the supplied value
    /// violates the E.164 format.
    /// </summary>
    /// <param name="value">
    /// Invalid E.164 phone number.
    /// </param>
    [Theory]
    [InlineData("+")]
    [InlineData("+0")]
    [InlineData("+01")]
    [InlineData("62123456789")]
    [InlineData("123456789")]
    [InlineData("++62123456789")]
    [InlineData("+62 8123456789")]
    [InlineData("+62-8123456789")]
    [InlineData("+62(8123456789)")]
    [InlineData("+abcdef")]
    [InlineData("+1234567890123456")]
    public void IsValid_Should_ReturnFalse_When_ValueViolatesE164Format(
        string value)
    {
        // Arrange

        // Act
        bool result = E164PhoneNumberValidator.IsValid(value);

        // Assert
        result.Should().BeFalse();
    }
}