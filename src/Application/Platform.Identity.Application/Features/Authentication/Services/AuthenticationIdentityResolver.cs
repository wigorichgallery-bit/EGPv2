// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Features/Authentication/Services/
// AuthenticationIdentityResolver.cs
// ===========================================

using Platform.Identity.Application.Abstractions.Authentication;
using Platform.Identity.Application.Abstractions.Persistence.Commands;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;
using Platform.SharedKernel.Exceptions;

namespace Platform.Identity.Application.Features.Authentication.Services;

/// <summary>
/// Resolves a user account from a supplied authentication identity.
///
/// Supported identities:
/// - Username.
/// - Email address.
///
/// Responsibility:
/// - Determine the identity type.
/// - Resolve the corresponding UserAccount aggregate.
///
/// Architectural Rules:
/// - Application-layer service.
/// - Uses application persistence abstractions only.
/// - Does not verify passwords.
/// - Does not generate tokens.
/// - Does not perform authorization.
/// - Does not validate MFA.
///
/// Security Rules:
/// - Invalid email identities are treated as unresolved.
/// - No domain validation exception is exposed to callers.
/// - Returns null when no matching user account exists.
/// </summary>
public sealed class AuthenticationIdentityResolver
    : IAuthenticationIdentityResolver
{
    private readonly IUserAccountRepository
        _userAccountRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AuthenticationIdentityResolver"/> class.
    /// </summary>
    /// <param name="userAccountRepository">
    /// User account repository used to resolve identities.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="userAccountRepository"/>
    /// is null.
    /// </exception>
    public AuthenticationIdentityResolver(
        IUserAccountRepository userAccountRepository)
    {
        ArgumentNullException.ThrowIfNull(
            userAccountRepository);

        _userAccountRepository =
            userAccountRepository;
    }

    /// <inheritdoc />
    public async Task<UserAccount?> ResolveAsync(
        string identity,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            identity);

        cancellationToken.ThrowIfCancellationRequested();

        var normalizedIdentity =
            identity.Trim();

        if (normalizedIdentity.Contains(
                '@',
                StringComparison.Ordinal))
        {
            return await ResolveByEmailAsync(
                normalizedIdentity,
                cancellationToken);
        }

        return await _userAccountRepository
            .GetByUsernameAsync(
                normalizedIdentity,
                cancellationToken);
    }

    /// <summary>
    /// Resolves a user account using an email address.
    /// </summary>
    /// <param name="identity">
    /// Supplied email identity.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Matching user account when found;
    /// otherwise null.
    /// </returns>
    private async Task<UserAccount?> ResolveByEmailAsync(
        string identity,
        CancellationToken cancellationToken)
    {
        try
        {
            var email =
                new EmailAddress(identity);

            return await _userAccountRepository
                .GetByEmailAsync(
                    email,
                    cancellationToken);
        }
        catch (DomainException)
        {
            return null;
        }
    }
}