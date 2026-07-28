// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Actions/CreateUser/CreateUserCommand.cs
// ===========================================
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Actions;

/// <summary>
/// Represents a request to create a new user account.
///
/// Responsibility:
/// - Carry user registration data from caller to use case.
/// - Remain immutable throughout execution.
///
/// Invariants:
/// - Command itself does not perform validation.
/// - Validation is delegated to validator and use case.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="Username">Unique username.</param>
/// <param name="Email">User email address.</param>
/// <param name="PhoneNumber">User phone number.</param>
/// <param name="Password">Plain text password.</param>
public sealed record CreateUserCommand(
    string Username,
    string Email,
    string PhoneNumber,
    string Password) :ICommand<Guid>, IGovernanceRequest
{
    public string GovernancePolicy
       => "IDENTITY.USER.CREATE";

    public string Resource
        => "User";

    public string Action
        => "Create";
};