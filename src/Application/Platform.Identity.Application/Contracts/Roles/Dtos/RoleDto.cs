// ===========================================
// File Location : src/Application/Platform.Identity.Application/Contracts/Roles/Dtos/RoleDto.cs
// ===========================================
namespace Platform.Identity.Application.Contracts.Roles.Dtos;

/// <summary>
/// Represents a role read model.
///
/// Responsibility:
/// - Transfer role information outside the domain layer.
/// - Support RBAC administration screens.
/// - Support permission inspection and governance review.
/// - Prevent aggregate leakage.
///
/// Invariants:
/// - Immutable.
/// - Permission collection is read-only.
/// - Does not expose domain behavior.
///
/// Side Effects:
/// - None.
///
/// Complexity:
/// O(1)
/// </summary>
/// <param name="RoleId">Unique role identifier.</param>
/// <param name="Name">Role name.</param>
/// <param name="IsSystemRole">Indicates whether role is system protected.</param>
/// <param name="ScopeType">Role scope classification.</param>
/// <param name="IsActive">Role active status.</param>
/// <param name="PermissionIds">Assigned permission identifiers.</param>
public sealed record RoleDto(
    Guid RoleId,
    string Name,
    bool IsSystemRole,
    string ScopeType,
    bool IsActive,
    IReadOnlyCollection<string> PermissionIds);