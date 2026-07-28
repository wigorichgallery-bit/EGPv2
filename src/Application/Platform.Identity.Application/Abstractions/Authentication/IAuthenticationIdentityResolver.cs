// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Abstractions/Authentication/
// IAuthenticationIdentityResolver.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Abstractions.Authentication;

/// <summary>
/// Defines the contract for resolving a user account from a login identity.
///
/// <para>
/// This abstraction encapsulates the application's identity resolution
/// strategy used during authentication.
/// </para>
///
/// <para>
/// Implementations are responsible for determining the identity type
/// (for example username, email address, employee number, or future
/// identity formats) and retrieving the corresponding
/// <see cref="UserAccount"/> aggregate.
/// </para>
///
/// <para>
/// This abstraction intentionally isolates authentication policies from
/// application use cases to support long-term extensibility without
/// modifying authentication workflows.
/// </para>
///
/// <para>
/// Implementations must not perform:
/// <list type="bullet">
/// <item><description>Password verification.</description></item>
/// <item><description>Token generation.</description></item>
/// <item><description>Authorization.</description></item>
/// <item><description>Multi-factor authentication validation.</description></item>
/// </list>
/// </para>
/// </summary>
public interface IAuthenticationIdentityResolver
{
    /// <summary>
    /// Resolves a user account from the specified login identity.
    /// </summary>
    /// <param name="identity">
    /// The user supplied login identity.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// The matching <see cref="UserAccount"/> when found;
    /// otherwise <see langword="null"/>.
    /// </returns>
    Task<UserAccount?> ResolveAsync(
        string identity,
        CancellationToken cancellationToken = default);
}