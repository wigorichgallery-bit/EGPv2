using FluentAssertions;
using Platform.Identity.Application.Features.Users.Queries;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Users.Queries;

/// <summary>
/// Unit tests for <see cref="GetUserByIdQuery"/>.
/// </summary>
public sealed class GetUserByIdQueryTests
{
    /// <summary>
    /// Verifies constructor assigns properties.
    /// </summary>
    [Fact]
    public void Constructor_Should_Assign_All_Properties()
    {
        // Arrange

        var userId =
            Guid.NewGuid();

        // Act

        var query =
            new GetUserByIdQuery(
                userId);

        // Assert

        query.UserId
            .Should()
            .Be(userId);
    }

    /// <summary>
    /// Verifies record supports value equality.
    /// </summary>
    [Fact]
    public void Record_Should_Support_Value_Equality()
    {
        // Arrange

        var id =
            Guid.NewGuid();

        var left =
            new GetUserByIdQuery(
                id);

        var right =
            new GetUserByIdQuery(
                id);

        // Assert

        left.Should()
            .Be(right);
    }

    /// <summary>
    /// Verifies records are not equal when
    /// identifiers differ.
    /// </summary>
    [Fact]
    public void Record_Should_Not_Be_Equal_When_UserId_Is_Different()
    {
        // Arrange

        var left =
            new GetUserByIdQuery(
                Guid.NewGuid());

        var right =
            new GetUserByIdQuery(
                Guid.NewGuid());

        // Assert

        left.Should()
            .NotBe(right);
    }
}