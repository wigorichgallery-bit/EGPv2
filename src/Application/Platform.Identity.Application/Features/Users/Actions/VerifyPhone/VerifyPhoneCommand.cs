// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/VerifyPhone/VerifyPhoneCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to verify a user's phone number.
///
/// Responsibility:
/// - Carry phone verification data.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="VerificationCode">Verification code supplied by user.</param>
public sealed record VerifyPhoneCommand(
    Guid UserId,
    string VerificationCode) : ICommand, IGovernanceRequest
{
    public string GovernancePolicy
    => "IDENTITY.PHONE.VERIFY";

    public string Resource
        => "User";

    public string Action
        => "VerifyPhone";
};