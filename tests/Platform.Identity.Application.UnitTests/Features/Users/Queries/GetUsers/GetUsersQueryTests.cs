using FluentAssertions;
using Platform.Identity.Application.Features.Users.Queries;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Queries;

/// <summary>
/// Unit tests for <see cref="GetUsersQuery"/>.
/// </summary>
public sealed class GetUsersQueryTests
{
    /// <summary>
    /// Verifies query can be constructed.
    /// </summary>
    [Fact]
    public void Constructor_Should_Create_Instance()
    {
        // Act

        var query =
            new GetUsersQuery();

        // Assert

        query.Should()
            .NotBeNull();
    }

    /// <summary>
    /// Verifies record supports value equality.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange

        var left =
            new GetUsersQuery();

        var right =
            new GetUsersQuery();

        // Assert

        left.Should()
            .Be(right);
    }
}