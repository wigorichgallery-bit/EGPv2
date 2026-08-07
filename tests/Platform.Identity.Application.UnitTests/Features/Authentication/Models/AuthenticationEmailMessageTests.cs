using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Unit tests for <see cref="AuthenticationEmailMessage"/>.
/// </summary>
public sealed class AuthenticationEmailMessageTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        const string recipient = "john.doe@example.com";
        const string subject = "Verification Code";
        const string body = "<p>Your verification code is <strong>123456</strong>.</p>";

        // Act
        var message = new AuthenticationEmailMessage(
            recipient,
            subject,
            body,
            true);

        // Assert
        message.Recipient.Should().Be(recipient);
        message.Subject.Should().Be(subject);
        message.Body.Should().Be(body);
        message.IsHtml.Should().BeTrue();
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var left = new AuthenticationEmailMessage(
            "john.doe@example.com",
            "Verification",
            "Body",
            true);

        var right = new AuthenticationEmailMessage(
            "john.doe@example.com",
            "Verification",
            "Body",
            true);

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
        var left = new AuthenticationEmailMessage(
            "john.doe@example.com",
            "Verification",
            "Body",
            true);

        var right = new AuthenticationEmailMessage(
            "admin@example.com",
            "Verification",
            "Body",
            true);

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
        var message = new AuthenticationEmailMessage(
            "john.doe@example.com",
            "Verification",
            "Body",
            true);

        // Act
        var (
            recipient,
            subject,
            body,
            isHtml) = message;

        // Assert
        recipient.Should().Be("john.doe@example.com");
        subject.Should().Be("Verification");
        body.Should().Be("Body");
        isHtml.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var message = new AuthenticationEmailMessage(
            "john.doe@example.com",
            "Verification",
            "Body",
            true);

        // Act
        var text = message.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationEmailMessage.Recipient));
        text.Should().Contain(nameof(AuthenticationEmailMessage.Subject));
        text.Should().Contain(nameof(AuthenticationEmailMessage.Body));
        text.Should().Contain(nameof(AuthenticationEmailMessage.IsHtml));
    }
}