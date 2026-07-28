// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Roles/Actions/AssignRole/AssignRoleCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Roles.Actions;
/// <summary>
/// Represents a request to assign a role to a user.
///
/// Responsibility:
/// - Carry role assignment information.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="RoleId">Role identifier.</param>
public sealed record AssignRoleCommand(
    Guid UserId,
    Guid RoleId) : ICommand, IGovernanceRequest
{
    public string GovernancePolicy
    => "IDENTITY.ROLE.ASSIGN";

    public string Resource
        => "Role";

    public string Action
        => "Assign";
};