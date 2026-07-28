// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Roles/Actions/RemoveRole/RemoveRoleCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Roles.Actions;
/// <summary>
/// Represents a request to remove a role from a user.
///
/// Responsibility:
/// - Carry role removal information.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="RoleId">Role identifier.</param>
public sealed record RemoveRoleCommand(
    Guid UserId,
    Guid RoleId) :ICommand, IGovernanceRequest
{
    public string GovernancePolicy =>
        "IDENTITY.ROLE.REMOVE";

    public string Resource =>
        "Role";

    public string Action =>
        "Remove";
};