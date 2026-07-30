// ===========================================
// File Location :
// tests/Platform.Identity.Domain.UnitTests/
// Aggregates/RoleTests.cs
// ===========================================

using FluentAssertions;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Exceptions;
using Xunit;

namespace Platform.Identity.Domain.UnitTests.Aggregates;

/// <summary>
/// Contains unit tests for
/// <see cref="Role"/>.
/// </summary>
public sealed partial class RoleTests
{    
    #region Constructor Tests

    /// <summary>
    /// Verifies that the constructor initializes
    /// every property correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var scope = RoleScope.Global;
        var createdAt = DateTime.UtcNow;

        // Act
        var role = new Role(
            id,
            "Administrator",
            false,
            scope,
            createdAt);

        // Assert
        role.Id.Should().Be(id);
        role.Name.Should().Be("Administrator");
        role.IsSystemRole.Should().BeFalse();
        role.Scope.Should().Be(scope);
        role.CreatedAt.Should().Be(createdAt);
        role.IsActive.Should().BeTrue();
        role.PermissionIds.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that a null role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsNull()
    {
        // Arrange

        // Act
        var action = () => new Role(
            Guid.NewGuid(),
            null!,
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an empty role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        // Arrange

        // Act
        var action = () => new Role(
            Guid.NewGuid(),
            string.Empty,
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a whitespace role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsWhitespace()
    {
        // Arrange

        // Act
        var action = () => new Role(
            Guid.NewGuid(),
            "   ",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a null role scope
    /// is rejected.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrow_WhenScopeIsNull()
    {
        // Arrange

        // Act
        var action = () => new Role(
            Guid.NewGuid(),
            "Administrator",
            false,
            null!,
            DateTime.UtcNow);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    #endregion

    #region AddPermission Tests

    /// <summary>
    /// Verifies that a permission
    /// can be added successfully.
    /// </summary>
    [Fact]
    public void AddPermission_ShouldAddPermission()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        // Act
        role.AddPermission(permission);

        // Assert
        role.PermissionIds.Should()
            .Contain(permission);

        role.HasPermission(permission)
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that duplicate permissions
    /// are ignored because the aggregate
    /// uses a hash set.
    /// </summary>
    [Fact]
    public void AddPermission_ShouldIgnoreDuplicatePermission()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission1 =
            new PermissionId("USER.CREATE");
        
        var permission2 = 
            new PermissionId("USER.CREATE");

        // Act
        role.AddPermission(permission1);
        role.AddPermission(permission2);

        // Assert
        role.PermissionIds.Should()
            .HaveCount(1);
    }

    /// <summary>
    /// Verifies that a null permission
    /// is rejected.
    /// </summary>
    [Fact]
    public void AddPermission_ShouldThrow_WhenPermissionIsNull()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () =>
            role.AddPermission(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that permissions cannot
    /// be modified after the role has
    /// been deactivated.
    /// </summary>
    [Fact]
    public void AddPermission_ShouldThrow_WhenRoleIsInactive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.Deactivate();

        var permission =
            new PermissionId("USER.CREATE");

        // Act
        var action = () =>
            role.AddPermission(permission);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode
            .Should()
            .Be("ROLE.INACTIVE");
    }

    #endregion

    #region RemovePermission Tests

    /// <summary>
    /// Verifies that an existing permission
    /// can be removed successfully.
    /// </summary>
    [Fact]
    public void RemovePermission_ShouldRemoveExistingPermission()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        role.AddPermission(permission);

        // Act
        role.RemovePermission(permission);

        // Assert
        role.PermissionIds.Should()
            .BeEmpty();

        role.HasPermission(permission)
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that removing a permission
    /// that is not assigned does not throw.
    /// </summary>
    [Fact]
    public void RemovePermission_ShouldIgnoreMissingPermission()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        // Act
        var action = () =>
            role.RemovePermission(permission);

        // Assert
        action.Should()
            .NotThrow();

        role.PermissionIds.Should()
            .BeEmpty();
    }

    /// <summary>
    /// Verifies that a null permission
    /// is rejected.
    /// </summary>
    [Fact]
    public void RemovePermission_ShouldThrow_WhenPermissionIsNull()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () =>
            role.RemovePermission(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that permissions cannot
    /// be removed from an inactive role.
    /// </summary>
    [Fact]
    public void RemovePermission_ShouldThrow_WhenRoleIsInactive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        role.Deactivate();

        // Act
        var action = () =>
            role.RemovePermission(permission);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode
            .Should()
            .Be("ROLE.INACTIVE");
    }

    /// <summary>
    /// Verifies that permissions cannot
    /// be removed from a system role.
    /// </summary>
    [Fact]
    public void RemovePermission_ShouldThrow_WhenRoleIsSystemRole()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Administrator",
            true,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        // Act
        var action = () =>
            role.RemovePermission(permission);

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode
            .Should()
            .Be("ROLE.SYSTEM_PROTECTED");
    }

    #endregion

    #region HasPermission Tests

    /// <summary>
    /// Verifies that the aggregate returns
    /// true when the permission exists.
    /// </summary>
    [Fact]
    public void HasPermission_ShouldReturnTrue_WhenPermissionExists()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        role.AddPermission(permission);

        // Act
        var result =
            role.HasPermission(permission);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the aggregate returns
    /// false when the permission does not exist.
    /// </summary>
    [Fact]
    public void HasPermission_ShouldReturnFalse_WhenPermissionDoesNotExist()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        var permission =
            new PermissionId("USER.CREATE");

        // Act
        var result =
            role.HasPermission(permission);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that a null permission
    /// is rejected.
    /// </summary>
    [Fact]
    public void HasPermission_ShouldThrow_WhenPermissionIsNull()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () =>
            role.HasPermission(null!);

        // Assert
        action.Should()
            .Throw<ArgumentNullException>();
    }

    #endregion

    #region ClearPermissions Tests

    /// <summary>
    /// Verifies that all permissions
    /// are removed successfully.
    /// </summary>
    [Fact]
    public void ClearPermissions_ShouldRemoveAllPermissions()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.AddPermission(new PermissionId("USER.CREATE"));
        role.AddPermission(new PermissionId("USER.UPDATE"));

        // Act
        role.ClearPermissions();

        // Assert
        role.PermissionIds.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that inactive roles
    /// cannot clear permissions.
    /// </summary>
    [Fact]
    public void ClearPermissions_ShouldThrow_WhenRoleIsInactive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.Deactivate();

        // Act
        var action = () => role.ClearPermissions();

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be("ROLE.INACTIVE");
    }

    /// <summary>
    /// Verifies that system roles
    /// cannot clear permissions.
    /// </summary>
    [Fact]
    public void ClearPermissions_ShouldThrow_WhenRoleIsSystemRole()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Administrator",
            true,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.ClearPermissions();

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be("ROLE.SYSTEM_PROTECTED");
    }

    #endregion

    #region Deactivate Tests

    /// <summary>
    /// Verifies that an active role
    /// becomes inactive.
    /// </summary>
    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        role.Deactivate();

        // Assert
        role.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that deactivating an
    /// already inactive role is idempotent.
    /// </summary>
    [Fact]
    public void Deactivate_ShouldBeIdempotent_WhenAlreadyInactive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.Deactivate();

        // Act
        var action = () => role.Deactivate();

        // Assert
        action.Should().NotThrow();
        role.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that system roles
    /// cannot be deactivated.
    /// </summary>
    [Fact]
    public void Deactivate_ShouldThrow_WhenRoleIsSystemRole()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Administrator",
            true,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Deactivate();

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be("ROLE.SYSTEM_PROTECTED");
    }

    #endregion

    #region Activate Tests

    /// <summary>
    /// Verifies that an inactive role
    /// becomes active.
    /// </summary>
    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.Deactivate();

        // Act
        role.Activate();

        // Assert
        role.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that activating an
    /// already active role is idempotent.
    /// </summary>
    [Fact]
    public void Activate_ShouldBeIdempotent_WhenAlreadyActive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Activate();

        // Assert
        action.Should().NotThrow();
        role.IsActive.Should().BeTrue();
    }

    #endregion

    #region Rename Tests

    /// <summary>
    /// Verifies that the role name
    /// can be changed successfully.
    /// </summary>
    [Fact]
    public void Rename_ShouldUpdateRoleName()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        role.Rename("Supervisor");

        // Assert
        role.Name.Should().Be("Supervisor");
    }

    /// <summary>
    /// Verifies that a null role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Rename_ShouldThrow_WhenNameIsNull()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Rename(null!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that an empty role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Rename_ShouldThrow_WhenNameIsEmpty()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Rename(string.Empty);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that a whitespace role name
    /// is rejected.
    /// </summary>
    [Fact]
    public void Rename_ShouldThrow_WhenNameIsWhitespace()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Rename("   ");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Verifies that inactive roles
    /// cannot be renamed.
    /// </summary>
    [Fact]
    public void Rename_ShouldThrow_WhenRoleIsInactive()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Operator",
            false,
            RoleScope.Global,
            DateTime.UtcNow);

        role.Deactivate();

        // Act
        var action = () => role.Rename("Supervisor");

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be("ROLE.INACTIVE");
    }

    /// <summary>
    /// Verifies that system roles
    /// cannot be renamed.
    /// </summary>
    [Fact]
    public void Rename_ShouldThrow_WhenRoleIsSystemRole()
    {
        // Arrange
        var role = new Role(
            Guid.NewGuid(),
            "Administrator",
            true,
            RoleScope.Global,
            DateTime.UtcNow);

        // Act
        var action = () => role.Rename("New Name");

        // Assert
        var exception = action.Should()
            .Throw<DomainException>()
            .Which;

        exception.ErrorCode.Should()
            .Be("ROLE.SYSTEM_PROTECTED");
    }

    #endregion
}