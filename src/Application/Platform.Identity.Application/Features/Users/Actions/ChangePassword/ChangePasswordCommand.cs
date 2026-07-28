// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/ChangePassword/ChangePasswordCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to change a user's password.
///
/// Responsibility:
/// - Carry password change information.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="CurrentPassword">Current password.</param>
/// <param name="NewPassword">New password.</param>
public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword) :ICommand, IGovernanceRequest
{
    public string GovernancePolicy =>
    "IDENTITY.PASSWORD.CHANGE";

    public string Resource =>
        "User";

    public string Action =>
        "ChangePassword";
};