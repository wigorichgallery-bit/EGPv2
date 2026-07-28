// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentication/Enums/AuthenticationStatus.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Enums;

/// <summary>
/// Represents the outcome of an authentication operation.
///
/// <para>
/// This enumeration is used by authentication responses to indicate
/// the current authentication workflow state.
/// </para>
///
/// <para>
/// The values defined in this enumeration are part of the public
/// application contract and must remain stable to preserve backward
/// compatibility.
/// </para>
/// </summary>
public enum AuthenticationStatus
{
    /// <summary>
    /// Authentication completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    // An authentication challenge must be completed
    // before authentication can continue.
    /// </summary>
    ChallengeRequired = 1,

    /// <summary>
    /// Email verification is required before
    /// authentication can continue.
    /// </summary>
    EmailVerificationRequired = 2,

    /// <summary>
    /// Phone number verification is required before
    /// authentication can continue.
    /// </summary>
    PhoneVerificationRequired = 3,

    /// <summary>
    /// The user's password has expired and must be changed
    /// before authentication can continue.
    /// </summary>
    PasswordExpired = 4,

    /// <summary>
    /// The user account is locked and cannot be authenticated.
    /// </summary>
    Locked = 5
}