// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/EnableMfa/EnableMfaCommand.cs
// ===========================================
using Platform.Identity.Domain.Enums;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to enable multi-factor authentication.
///
/// Responsibility:
/// - Carry MFA enablement information.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="Method">Requested MFA method.</param>
public sealed record EnableMfaCommand(
    Guid UserId,
    MFAMethod Method) :ICommand, IGovernanceRequest
{
    public string GovernancePolicy =>
    "IDENTITY.MFA.ENABLE";

    public string Resource =>
        "User";

    public string Action =>
        "EnableMfa";
};