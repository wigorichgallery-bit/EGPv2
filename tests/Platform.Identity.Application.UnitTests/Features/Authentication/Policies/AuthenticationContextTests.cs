using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Application.Features.Authentication.Policies.Models;
using Platform.Identity.Application.UnitTests.Fixtures.Builders;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Authentication.Policies.Models;

/// <summary>
/// Contains unit tests for <see cref="AuthenticationContext"/>.
/// </summary>
public sealed class AuthenticationContextTests
{
    /// <summary>
    /// Verifies the constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange
        var user = UserAccountBuilder.Default.Build();

        var request = new LoginRequest(
            "john.doe",
            "Password123!");

        var now = DateTimeOffset.UtcNow;

        // Act
        var context = new AuthenticationContext(
            user,
            request,
            now);

        // Assert
        context.User.Should().BeSameAs(user);
        context.Request.Should().BeSameAs(request);
        context.CurrentUtc.Should().Be(now);
    }

    /// <summary>
    /// Verifies identical records are equal.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_Be_Equal()
    {
        // Arrange
        var user = UserAccountBuilder.Default.Build();

        var request = new LoginRequest(
            "john.doe",
            "Password123!");

        var now = DateTimeOffset.UtcNow;

        var left = new AuthenticationContext(
            user,
            request,
            now);

        var right = new AuthenticationContext(
            user,
            request,
            now);

        // Assert
        left.Should().Be(right);
        left.Equals(right).Should().BeTrue();
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies different records are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_Be_Equal()
    {
        // Arrange
        var user = UserAccountBuilder.Default.Build();

        var left = new AuthenticationContext(
            user,
            new LoginRequest("john", "Password1"),
            DateTimeOffset.UtcNow);

        var right = new AuthenticationContext(
            user,
            new LoginRequest("admin", "Password2"),
            DateTimeOffset.UtcNow);

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
        var user = UserAccountBuilder.Default.Build();

        var request = new LoginRequest(
            "john",
            "Password");

        var now = DateTimeOffset.UtcNow;

        var context = new AuthenticationContext(
            user,
            request,
            now);

        // Act
        var (
            actualUser,
            actualRequest,
            actualNow) = context;

        // Assert
        actualUser.Should().BeSameAs(user);
        actualRequest.Should().BeSameAs(request);
        actualNow.Should().Be(now);
    }

    /// <summary>
    /// Verifies the generated string representation contains property names.
    /// </summary>
    [Fact]
    public void ToString_Should_Contain_Property_Names()
    {
        // Arrange
        var context = new AuthenticationContext(
            UserAccountBuilder.Default.Build(),
            new LoginRequest(
                "john",
                "Password"),
            DateTimeOffset.UtcNow);

        // Act
        var text = context.ToString();

        // Assert
        text.Should().Contain(nameof(AuthenticationContext.User));
        text.Should().Contain(nameof(AuthenticationContext.Request));
        text.Should().Contain(nameof(AuthenticationContext.CurrentUtc));
    }
}