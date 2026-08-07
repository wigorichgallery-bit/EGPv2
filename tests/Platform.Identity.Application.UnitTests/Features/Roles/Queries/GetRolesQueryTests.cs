using FluentAssertions;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Identity.Application.Features.Roles.Queries;
using Platform.Pipeline.Abstractions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Queries;

/// <summary>
/// Unit tests for <see cref="GetRolesQuery"/>.
/// </summary>
public sealed class GetRolesQueryTests
{
    /// <summary>
    /// Verifies query can be created.
    /// </summary>
    [Fact]
    public void Constructor_Should_Create_Instance()
    {
        // Act

        var query =
            new GetRolesQuery();

        // Assert

        query
            .Should()
            .NotBeNull();
    }

    /// <summary>
    /// Verifies query implements
    /// the expected query contract.
    /// </summary>
    [Fact]
    public void Should_Implement_IQuery()
    {
        // Act

        var query =
            new GetRolesQuery();

        // Assert

        query
            .Should()
            .BeAssignableTo<
                IQuery<IReadOnlyList<RoleDto>>>();
    }

    /// <summary>
    /// Verifies two empty queries
    /// are value-equal.
    /// </summary>
    [Fact]
    public void Equality_Should_Be_Value_Based()
    {
        // Arrange

        var left =
            new GetRolesQuery();

        var right =
            new GetRolesQuery();

        // Assert

        left
            .Should()
            .Be(right);

        left.GetHashCode()
            .Should()
            .Be(right.GetHashCode());
    }
}