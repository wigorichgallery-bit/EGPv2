// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentication/Requests/LoginRequest.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Requests;

/// <summary>
/// Represents the credentials required to authenticate a user.
///
/// <para>
/// This request is the input contract for the login use case.
/// It contains only the information required to verify the
/// user's identity.
/// </para>
///
/// <para>
/// Request context information such as IP address, user agent,
/// device identifier, client application, and correlation
/// identifier are intentionally excluded from this contract.
/// Such information is obtained through application
/// abstractions and infrastructure services.
/// </para>
///
/// <para>
/// This request is an immutable application contract and
/// contains no business logic or validation logic.
/// </para>
/// </summary>
/// <param name="Identity">
/// The user identity supplied for authentication.
///
/// <para>
/// The identity may represent a username, email address,
/// employee number, or another supported login identifier,
/// depending on the configured authentication policy.
/// </para>
/// </param>
/// <param name="Password">
/// The plaintext password supplied by the user.
///
/// The password is validated by the authentication workflow
/// and must never be logged or persisted.
/// </param>
public sealed record LoginRequest(
    string Identity,
    string Password);