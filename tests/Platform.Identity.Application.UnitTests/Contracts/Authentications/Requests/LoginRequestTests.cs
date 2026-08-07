using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Requests;

/// <summary>
/// Unit tests for <see cref="LoginRequest"/>.
/// </summary>
public sealed class LoginRequestTests
{
    /// <summary>
    /// Verifies constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_AssignProperties()
    {
        var request = new LoginRequest(
            "john.doe",
            "Password123!");

        request.Identity.Should().Be("john.doe");
        request.Password.Should().Be("Password123!");
    }

    /// <summary>
    /// Verifies value equality.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_BeEqual()
    {
        var left = new LoginRequest("user", "pass");
        var right = new LoginRequest("user", "pass");

        left.Should().Be(right);
        (left == right).Should().BeTrue();
    }

    /// <summary>
    /// Verifies different values are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_BeEqual()
    {
        var left = new LoginRequest("user1", "pass");
        var right = new LoginRequest("user2", "pass");

        left.Should().NotBe(right);
    }

    /// <summary>
    /// Verifies deconstruction.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_ReturnValues()
    {
        var request = new LoginRequest(
            "john",
            "secret");

        var (identity, password) = request;

        identity.Should().Be("john");
        password.Should().Be("secret");
    }

    /// <summary>
    /// Verifies string representation contains property values.
    /// </summary>
    [Fact]
    public void ToString_Should_ContainPropertyValues()
    {
        var request = new LoginRequest(
            "john",
            "secret");

        var text = request.ToString();

        text.Should().Contain("john");
        text.Should().Contain("secret");
    }
}