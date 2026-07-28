// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Abstractions/Security/ITokenService.cs
// ===========================================

using Platform.Identity.Application.Contracts.Authentication.Dtos;
using Platform.Identity.Application.Contracts.Authentication.Requests;

namespace Platform.Identity.Application.Abstractions.Security;

/// <summary>
/// Defines the contract for generating authentication tokens.
///
/// <para>
/// Implementations of this interface are responsible only for
/// generating authentication tokens from validated authentication
/// data. Business validation, credential verification, account
/// validation, and authorization checks are performed by the
/// corresponding application use case.
/// </para>
///
/// <para>
/// This abstraction is implemented by the infrastructure layer.
/// </para>
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates an authentication token for a successfully
    /// authenticated user.
    /// </summary>
    /// <param name="request">
    /// Contains all information required to generate the
    /// authentication token.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// An <see cref="AuthenticationTokenDto"/> containing the
    /// generated access token and related authentication data.
    /// </returns>
    Task<AuthenticationTokenDto> GenerateTokenAsync(
        TokenGenerationRequest request,
        CancellationToken cancellationToken = default);
}