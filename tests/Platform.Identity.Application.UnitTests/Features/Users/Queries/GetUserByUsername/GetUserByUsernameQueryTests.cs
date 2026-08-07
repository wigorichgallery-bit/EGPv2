using FluentAssertions;
using Platform.Identity.Application.Features.Users.Queries;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Queries;

/// <summary>
/// Unit tests for <see cref="GetUserByUsernameQuery"/>.
/// </summary>
public sealed class GetUserByUsernameQueryTests
{
    /// <summary>
    /// Verifies constructor assigns properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange

        const string username =
            "john.doe";

        // Act

        var query =
            new GetUserByUsernameQuery(
                username);

        // Assert

        query.Username
            .Should()
            .Be(username);
    }

    /// <summary>
    /// Verifies record supports value equality.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange

        var left =
            new GetUserByUsernameQuery(
                "john");

        var right =
            new GetUserByUsernameQuery(
                "john");

        // Assert

        left.Should()
            .Be(right);
    }

    /// <summary>
    /// Verifies records are not equal
    /// when usernames differ.
    /// </summary>
    [Fact]
    public void Record_Should_Not_Be_Equal_When_Username_Is_Different()
    {
        // Arrange

        var left =
            new GetUserByUsernameQuery(
                "john");

        var right =
            new GetUserByUsernameQuery(
                "jane");

        // Assert

        left.Should()
            .NotBe(right);
    }
}