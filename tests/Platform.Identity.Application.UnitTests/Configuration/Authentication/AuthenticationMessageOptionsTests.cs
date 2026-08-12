
using Platform.Identity.Application.Configuration.Authentication;

namespace Platform.Identity.Application.UnitTests.Configuration;

/// <summary>
/// Unit tests for <see cref="AuthenticationMessageOptions"/>.
/// </summary>
public sealed class AuthenticationMessageOptionsTests
{
    /// <summary>
    /// Verifies the section name remains stable.
    /// </summary>
    [Fact]
    public void SectionName_Should_Be_Stable()
    {
        AuthenticationMessageOptions.SectionName
            .Should()
            .Be("Authentication:Messages");
    }

    /// <summary>
    /// Verifies default values.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_Default_Values()
    {
        // Act
        var options = new AuthenticationMessageOptions();

        // Assert
        options.ApplicationName.Should().BeEmpty();
        options.VerificationCodeEmailSubject.Should().Be("Your verification code");
        options.VerificationCodeSmsPrefix.Should().Be("Your verification code is");
        options.VerificationCodeWhatsAppPrefix.Should().Be("Your verification code is");
        options.IgnoreMessage.Should().Be(
            "If you did not request this authentication challenge, you can safely ignore this message.");
    }

    /// <summary>
    /// Verifies init properties can be assigned.
    /// </summary>
    [Fact]
    public void Init_Properties_Should_Be_Assignable()
    {
        // Arrange & Act
        var options = new AuthenticationMessageOptions
        {
            ApplicationName = "EGPv2",
            VerificationCodeEmailSubject = "Email Subject",
            VerificationCodeSmsPrefix = "SMS Prefix",
            VerificationCodeWhatsAppPrefix = "WA Prefix",
            IgnoreMessage = "Ignore"
        };

        // Assert
        options.ApplicationName.Should().Be("EGPv2");
        options.VerificationCodeEmailSubject.Should().Be("Email Subject");
        options.VerificationCodeSmsPrefix.Should().Be("SMS Prefix");
        options.VerificationCodeWhatsAppPrefix.Should().Be("WA Prefix");
        options.IgnoreMessage.Should().Be("Ignore");
    }
}