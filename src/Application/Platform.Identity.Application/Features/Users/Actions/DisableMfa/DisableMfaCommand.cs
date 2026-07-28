// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/DisableMfa/DisableMfaCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to disable multi-factor authentication.
///
/// Responsibility:
/// - Carry MFA disablement information.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
public sealed record DisableMfaCommand(
    Guid UserId) :ICommand, IGovernanceRequest
{
    public string GovernancePolicy =>
        "IDENTITY.MFA.DISABLE";

    public string Resource =>
        "User";

    public string Action =>
        "DisableMfa";
};