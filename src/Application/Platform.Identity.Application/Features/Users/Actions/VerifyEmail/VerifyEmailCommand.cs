// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/VerifyEmail/VerifyEmailCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to verify a user's email address.
///
/// Responsibility:
/// - Carry email verification data.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="UserId">Target user identifier.</param>
/// <param name="VerificationCode">Verification code supplied by user.</param>
public sealed record VerifyEmailCommand(
    Guid UserId,
    string VerificationCode) : ICommand, IGovernanceRequest
{
    public string GovernancePolicy
    => "IDENTITY.EMAIL.VERIFY";

    public string Resource
        => "User";

    public string Action
        => "VerifyEmail";
};