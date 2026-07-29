using FluentAssertions;
using Platform.SharedKernel.Validation;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Validation;

/// <summary>
/// Contains unit tests for the <see cref="EmailAddressValidator"/> class.
///
/// <remarks>
/// <para>
/// Purpose:
/// Verifies that <see cref="EmailAddressValidator"/> correctly validates
/// email address formats according to the application's validation rules.
/// </para>
///
/// <para>
/// Test Strategy:
/// <list type="bullet">
/// <item>
/// <description>
/// Verify valid email addresses are accepted.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify null and whitespace values are rejected.
/// </description>
/// </item>
/// <item>
/// <description>
/// Verify malformed email addresses are rejected.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Scope:
/// Unit tests for the <see cref="EmailAddressValidator"/> utility class only.
/// </para>
/// </remarks>
/// </summary>
public sealed class EmailAddressValidatorTests
{
    #region EmailAddressValidator.IsValid()

    /// <summary>
    /// Verifies that <see cref="EmailAddressValidator.IsValid(string)"/>
    /// returns <see langword="true"/> for valid email addresses.
    /// </summary>
    /// <param name="email">
    /// The email address to validate.
    /// </param>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The validation result is <see langword="true"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("john.doe@example.com")]
    [InlineData("user123@example.co.id")]
    [InlineData("user@mail.example.com")]
    public void IsValid_WithValidEmail_ShouldReturnTrue(string email)
    {
        // Arrange

        // Act
        var result = EmailAddressValidator.IsValid(email);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that <see cref="EmailAddressValidator.IsValid(string)"/>
    /// returns <see langword="false"/> for
    /// <see langword="null"/>, empty, or whitespace-only values.
    /// </summary>
    /// <param name="email">
    /// The email value to validate.
    /// </param>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The validation result is <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void IsValid_WithNullOrWhitespace_ShouldReturnFalse(string? email)
    {
        // Arrange

        // Act
        var result = EmailAddressValidator.IsValid(email!);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that <see cref="EmailAddressValidator.IsValid(string)"/>
    /// returns <see langword="false"/> for malformed email addresses.
    /// </summary>
    /// <param name="email">
    /// The invalid email address to validate.
    /// </param>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The validation result is <see langword="false"/>.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Theory]
    [InlineData("userexample.com")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user@example")]
    [InlineData("user example@example.com")]
    [InlineData("user@@example.com")]
    public void IsValid_WithInvalidEmail_ShouldReturnFalse(string email)
    {
        // Arrange

        // Act
        var result = EmailAddressValidator.IsValid(email);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}