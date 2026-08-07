using FluentAssertions;
using Platform.Identity.Application.Contracts.Authentication.Requests;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Authentication.Requests;

/// <summary>
/// Unit tests for <see cref="TokenGenerationRequest"/>.
/// </summary>
public sealed class TokenGenerationRequestTests
{
    /// <summary>
    /// Verifies constructor assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_AssignProperties()
    {
        var id = Guid.NewGuid();

        IReadOnlyCollection<string> roles =
        [
            "Administrator",
            "Operator"
        ];

        IReadOnlyCollection<string> permissions =
        [
            "Users.Read",
            "Users.Write"
        ];

        var request = new TokenGenerationRequest(
            id,
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        request.UserId.Should().Be(id);
        request.Username.Should().Be("john");
        request.Email.Should().Be("john@example.com");
        request.SecurityStamp.Should().Be("stamp");
        request.Roles.Should().Equal(roles);
        request.Permissions.Should().Equal(permissions);
    }

    /// <summary>
    /// Verifies value equality.
    /// </summary>
    [Fact]
    public void Equal_Records_Should_BeEqual()
    {
        var id = Guid.NewGuid();

        IReadOnlyCollection<string> roles = ["Admin"];
        IReadOnlyCollection<string> permissions = ["Read"];

        var left = new TokenGenerationRequest(
            id,
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        var right = new TokenGenerationRequest(
            id,
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        left.Should().Be(right);
    }

    /// <summary>
    /// Verifies different records are not equal.
    /// </summary>
    [Fact]
    public void Different_Records_Should_Not_BeEqual()
    {
        IReadOnlyCollection<string> roles = ["Admin"];
        IReadOnlyCollection<string> permissions = ["Read"];

        var left = new TokenGenerationRequest(
            Guid.NewGuid(),
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        var right = new TokenGenerationRequest(
            Guid.NewGuid(),
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        left.Should().NotBe(right);
    }

    /// <summary>
    /// Verifies deconstruction.
    /// </summary>
    [Fact]
    public void Deconstruct_Should_ReturnExpectedValues()
    {
        var id = Guid.NewGuid();

        IReadOnlyCollection<string> roles = ["Admin"];
        IReadOnlyCollection<string> permissions = ["Read"];

        var request = new TokenGenerationRequest(
            id,
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        var (
            userId,
            username,
            email,
            securityStamp,
            assignedRoles,
            assignedPermissions) = request;

        userId.Should().Be(id);
        username.Should().Be("john");
        email.Should().Be("john@example.com");
        securityStamp.Should().Be("stamp");
        assignedRoles.Should().Equal(roles);
        assignedPermissions.Should().Equal(permissions);
    }

    /// <summary>
    /// Verifies string representation contains important values.
    /// </summary>
    [Fact]
    public void ToString_Should_ContainPropertyValues()
    {
        IReadOnlyCollection<string> roles = ["Admin"];
        IReadOnlyCollection<string> permissions = ["Read"];

        var request = new TokenGenerationRequest(
            Guid.Empty,
            "john",
            "john@example.com",
            "stamp",
            roles,
            permissions);

        var text = request.ToString();

        text.Should().Contain("john");
        text.Should().Contain("john@example.com");
        text.Should().Contain("stamp");
    }
}