using FluentAssertions;
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Contracts.Roles.Dtos;

/// <summary>
/// Unit tests for <see cref="RoleDto"/>.
/// </summary>
public sealed class RoleDtoTests
{
    /// <summary>
    /// Verifies constructor initializes every property.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_All_Properties()
    {
        // Arrange

        Guid roleId =
            Guid.NewGuid();

        IReadOnlyCollection<string> permissions =
            new[]
            {
                "USER.READ",
                "USER.WRITE"
            };

        // Act

        var dto =
            new RoleDto(
                roleId,
                "Administrator",
                true,
                "Global",
                true,
                permissions);

        // Assert

        dto.RoleId
            .Should()
            .Be(roleId);

        dto.Name
            .Should()
            .Be("Administrator");

        dto.IsSystemRole
            .Should()
            .BeTrue();

        dto.ScopeType
            .Should()
            .Be("Global");

        dto.IsActive
            .Should()
            .BeTrue();

        dto.PermissionIds
            .Should()
            .BeEquivalentTo(permissions);
    }

    /// <summary>
    /// Verifies record equality for identical values.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_True_For_Identical_Values()
    {
        // Arrange

        IReadOnlyCollection<string> permissions =
            new[]
            {
                "USER.READ",
                "USER.WRITE"
            };

        var first =
            new RoleDto(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Administrator",
                true,
                "Global",
                true,
                permissions);

        var second =
            new RoleDto(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Administrator",
                true,
                "Global",
                true,
                permissions);

        // Assert

        first.Should().Be(second);
    }

    /// <summary>
    /// Verifies record inequality when values differ.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_False_When_Values_Differ()
    {
        // Arrange

        var first =
            new RoleDto(
                Guid.NewGuid(),
                "Administrator",
                true,
                "Global",
                true,
                new[]
                {
                    "USER.READ"
                });

        var second =
            new RoleDto(
                Guid.NewGuid(),
                "Operator",
                false,
                "Tenant",
                false,
                new[]
                {
                    "USER.WRITE"
                });

        // Assert

        first.Should().NotBe(second);
    }

    /// <summary>
    /// Verifies permission identifiers are preserved.
    /// </summary>
    [Fact]
    public void Constructor_Should_Preserve_PermissionIds()
    {
        // Arrange

        IReadOnlyCollection<string> permissions =
            new[]
            {
                "USER.READ",
                "USER.WRITE",
                "ROLE.DELETE"
            };

        // Act

        var dto =
            new RoleDto(
                Guid.NewGuid(),
                "Administrator",
                false,
                "Global",
                true,
                permissions);

        // Assert

        dto.PermissionIds
            .Should()
            .HaveCount(3)
            .And
            .ContainInOrder(
                "USER.READ",
                "USER.WRITE",
                "ROLE.DELETE");
    }
}