// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Features/Authentication/Actions/Login/
// LoginCommand.cs
// ===========================================

using Platform.Identity.Application.Contracts.Authentication.Responses;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Authentication.Actions;

/// <summary>
/// Represents a request to authenticate a user.
///
/// Responsibility:
/// - Carry authentication credentials from caller to the login use case.
/// - Remain immutable throughout execution.
///
/// Invariants:
/// - Command itself does not perform validation.
/// - Validation is delegated to the corresponding validator and use case.
///
/// Side Effects:
/// - None.
/// </summary>
/// <param name="Identity">
/// Username, email address, or other supported identity supplied by the user.
/// </param>
/// <param name="Password">
/// Plain text password supplied by the user.
/// </param>
public sealed record LoginCommand(
    string Identity,
    string Password)
    : ICommand<LoginResponse>, IGovernanceRequest
{
    /// <summary>
    /// Gets the governance policy required to execute
    /// the login operation.
    /// </summary>
    public string GovernancePolicy =>
        "IDENTITY.AUTH.LOGIN";

    /// <summary>
    /// Gets the protected resource.
    /// </summary>
    public string Resource =>
        "Authentication";

    /// <summary>
    /// Gets the requested action.
    /// </summary>
    public string Action =>
        "Login";
}