// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Enums/AuthenticationChallengePurpose.cs
// ===========================================

namespace Platform.Identity.Domain.Enums;

/// <summary>
/// Represents the business purpose of an authentication challenge.
///
/// <para>
/// The purpose determines why an authentication challenge is
/// created and allows the authentication engine to support
/// multiple authentication workflows using a single aggregate.
/// </para>
///
/// <para>
/// The numeric values defined by this enumeration are part of
/// the domain contract and must remain stable to preserve
/// persistence compatibility.
/// </para>
/// </summary>
public enum AuthenticationChallengePurpose
{
    /// <summary>
    /// Indicates that the challenge is created for
    /// user authentication during login.
    /// </summary>
    Login = 0,

    /// <summary>
    /// Indicates that the challenge is created to
    /// verify a password reset request.
    /// </summary>
    PasswordReset = 1,

    /// <summary>
    /// Indicates that the challenge is created to
    /// verify ownership of an email address.
    /// </summary>
    EmailVerification = 2,

    /// <summary>
    /// Indicates that the challenge is created to
    /// verify ownership of a phone number.
    /// </summary>
    PhoneVerification = 3,

    /// <summary>
    /// Indicates that the challenge protects a
    /// security-sensitive operation.
    /// </summary>
    SensitiveOperation = 4,

    /// <summary>
    /// Indicates that the challenge is created for
    /// account recovery.
    /// </summary>
    AccountRecovery = 5,

    /// <summary>
    /// Indicates that the challenge is created for
    /// a custom business workflow.
    /// </summary>
    Custom = 6
}