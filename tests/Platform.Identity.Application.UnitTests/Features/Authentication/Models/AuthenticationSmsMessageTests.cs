using FluentAssertions;
using Platform.Identity.Application.Features.Authentication.Models;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Models;

/// <summary>
/// Contains unit tests for <see cref="AuthenticationSmsMessage"/>.
/// </summary>
public sealed class AuthenticationSmsMessageTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        const string recipient = "+6281234567890";
        const string body = "Your verification code is 123456.";

        // Act
        var message = new AuthenticationSmsMessage(
            recipient,
            body);

        // Assert
        message.Recipient.Should().Be(recipient);
        message.Body.Should().Be(body);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var left = new AuthenticationSmsMessage(
            "+6281234567890",
            "Your verification code is 123456.");

        var right = new AuthenticationSmsMessage(
            "+6281234567890",
            "Your verification code is 123456.");

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
        var left = new AuthenticationSmsMessage(
            "+6281234567890",
            "Message A");

        var right = new AuthenticationSmsMessage(
            "+6281234567890",
            "Message B");

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
        var message = new AuthenticationSmsMessage(
            "+6281234567890",
            "Your verification code is 123456.");

        // Act
        var (
            recipient,
            body) = message;

        // Assert
        recipient.Should().Be("+6281234567890");
        body.Should().Be("Your verification code is 123456.");
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var message = new AuthenticationSmsMessage(
            "+6281234567890",
            "Your verification code is 123456.");

        // Act
        var text = message.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationSmsMessage.Recipient));
        text.Should().Contain(nameof(AuthenticationSmsMessage.Body));
    }
}