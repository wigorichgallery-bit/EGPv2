// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Projections/RoleProjectionTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Projections;

namespace Platform.Persistence.UnitTests.Projections;

/// <summary>
/// Contains unit tests for the
/// <see cref="RoleProjection"/> class.
///
/// Responsibility:
/// - Verify null aggregate handling.
/// - Verify scalar role property mapping.
/// - Verify role scope value object mapping.
/// - Verify permission identifier mapping.
/// - Verify empty permission collection mapping.
///
/// Testing Strategy:
/// - Exercise the public static ToDto method.
/// - Use real domain aggregates.
/// - Use real value objects.
/// - Avoid EF Core.
/// - Avoid database connections.
/// - Avoid reflection.
/// - Avoid mocks because the projection has no dependencies.
///
/// Coverage Strategy:
/// - Cover null input branch.
/// - Cover valid aggregate path.
/// - Cover permission collection path.
/// - Cover empty permission collection path.
/// </summary>
public sealed class RoleProjectionTests
{
    /// <summary>
    /// Verifies that <see cref="RoleProjection.ToDto"/>
    /// throws <see cref="ArgumentNullException"/> when
    /// the supplied role is null.
    /// </summary>
    [Fact]
    public void ToDto_Should_ThrowArgumentNullException_When_RoleIsNull()
    {
        // Arrange
        Role role = null!;

        // Act
        Action act = () =>
            RoleProjection.ToDto(role);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that <see cref="RoleProjection.ToDto"/>
    /// maps all scalar role properties and the role scope
    /// value object into the corresponding DTO properties.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapRoleProperties_When_RoleIsValid()
    {
        // Arrange
        var roleId =
            Guid.NewGuid();

        var role =
            new Role(
                roleId,
                "Tenant Administrator",
                true,
                RoleScope.Tenant,
                DateTime.UtcNow);

        // Act
        var result =
            RoleProjection.ToDto(role);

        // Assert
        result.Should()
            .NotBeNull();

        result.RoleId
            .Should()
            .Be(roleId);

        result.Name
            .Should()
            .Be("Tenant Administrator");

        result.IsSystemRole
            .Should()
            .BeTrue();

        result.ScopeType
            .Should()
            .Be("TENANT");

        result.IsActive
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that
    /// <see cref="RoleProjection.ToDto"/>
    /// converts permission value objects into
    /// their string identifiers.
    /// </summary>
    [Fact]
    public void ToDto_Should_MapPermissionIds_When_RoleHasPermissions()
    {
        // Arrange
        var role =
            new Role(
                Guid.NewGuid(),
                "Permission Administrator",
                false,
                RoleScope.Global,
                DateTime.UtcNow);

        role.AddPermission(
            new PermissionId("USER.CREATE"));

        role.AddPermission(
            new PermissionId("USER.UPDATE"));

        role.AddPermission(
            new PermissionId("ROLE.DELETE"));

        // Act
        var result =
            RoleProjection.ToDto(role);

        // Assert
        result.PermissionIds
            .Should()
            .NotBeNull();

        result.PermissionIds
            .Should()
            .HaveCount(3);

        result.PermissionIds
            .Should()
            .Contain(
                "USER.CREATE");

        result.PermissionIds
            .Should()
            .Contain(
                "USER.UPDATE");

        result.PermissionIds
            .Should()
            .Contain(
                "ROLE.DELETE");
    }

    /// <summary>
    /// Verifies that
    /// <see cref="RoleProjection.ToDto"/>
    /// returns an empty permission collection when
    /// the role has no assigned permissions.
    /// </summary>
    [Fact]
    public void ToDto_Should_ReturnEmptyPermissionIds_When_RoleHasNoPermissions()
    {
        // Arrange
        var role =
            new Role(
                Guid.NewGuid(),
                "Read Only",
                false,
                RoleScope.Organization,
                DateTime.UtcNow);

        // Act
        var result =
            RoleProjection.ToDto(role);

        // Assert
        result.PermissionIds
            .Should()
            .NotBeNull();

        result.PermissionIds
            .Should()
            .BeEmpty();
    }
}