// ===========================================
// File Location :
// src/Application/Platform.Identity.Application/
// Contracts/Authentication/Enums/
// AuthenticationChallengePurpose.cs
// ===========================================

namespace Platform.Identity.Application.Contracts.Authentication.Enums;

/// <summary>
/// Represents the business purpose of an authentication
/// challenge within the application authentication workflow.
///
/// <para>
/// This enumeration is part of the public application contract
/// and identifies why an authentication challenge has been
/// created. Clients can use this information to determine the
/// appropriate user experience required to complete the
/// authentication workflow.
/// </para>
///
/// <para>
/// Although this enumeration currently mirrors the identity
/// domain model, it is intentionally maintained as a separate
/// application contract to preserve Clean Architecture
/// boundaries and prevent domain implementation details from
/// leaking into presentation or external consumers.
/// </para>
///
/// <para>
/// The numeric values defined by this enumeration are part of
/// the public application contract and must remain stable to
/// preserve backward compatibility.
/// </para>
///
/// <para>
/// This enumeration describes the business purpose of the
/// challenge, whereas
/// <see cref="AuthenticationChallengeType"/>
/// describes the authentication mechanism used to satisfy the
/// challenge.
/// </para>
/// </summary>
public enum AuthenticationChallengePurpose
{
    /// <summary>
    /// Indicates that the authentication challenge is created
    /// for user authentication during the login workflow.
    /// </summary>
    Login = 0,

    /// <summary>
    /// Indicates that the authentication challenge is created
    /// to verify a password reset request.
    /// </summary>
    PasswordReset = 1,

    /// <summary>
    /// Indicates that the authentication challenge is created
    /// to verify ownership of an email address.
    /// </summary>
    EmailVerification = 2,

    /// <summary>
    /// Indicates that the authentication challenge is created
    /// to verify ownership of a phone number.
    /// </summary>
    PhoneVerification = 3,

    /// <summary>
    /// Indicates that the authentication challenge protects a
    /// security-sensitive operation.
    /// </summary>
    SensitiveOperation = 4,

    /// <summary>
    /// Indicates that the authentication challenge is created
    /// for account recovery.
    /// </summary>
    AccountRecovery = 5,

    /// <summary>
    /// Indicates that the authentication challenge is created
    /// for a custom business workflow.
    /// </summary>
    Custom = 6
}