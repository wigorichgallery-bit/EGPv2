// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Enums/AuthenticationChallengeStatus.cs
// ===========================================

namespace Platform.Identity.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of an authentication challenge.
///
/// <para>
/// An authentication challenge is created after the user's
/// primary credentials have been successfully verified and
/// remains active until it is completed, cancelled,
/// revoked, expired, or locked.
/// </para>
///
/// <para>
/// This enumeration is used exclusively within the domain model
/// to enforce authentication challenge lifecycle rules.
/// </para>
///
/// <para>
/// The numeric values defined by this enumeration are part of
/// the domain contract and must remain stable to preserve
/// persistence compatibility.
/// </para>
/// </summary>
public enum AuthenticationChallengeStatus
{
    /// <summary>
    /// Indicates that the authentication challenge has been
    /// created successfully and is awaiting user verification.
    ///
    /// <para>
    /// This is the initial state of every authentication
    /// challenge.
    /// </para>
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Indicates that the authentication challenge has been
    /// successfully verified and completed.
    ///
    /// <para>
    /// A completed challenge cannot be reused.
    /// </para>
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Indicates that the authentication challenge has expired
    /// before successful verification.
    ///
    /// <para>
    /// Expired challenges are permanently invalid and require
    /// the authentication process to be restarted.
    /// </para>
    /// </summary>
    Expired = 2,

    /// <summary>
    /// Indicates that the authentication challenge has been
    /// cancelled by the authentication workflow.
    ///
    /// <para>
    /// Cancellation typically occurs due to normal application
    /// flow, such as the user abandoning the authentication
    /// process or initiating a new authentication attempt.
    /// </para>
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// Indicates that the authentication challenge has been
    /// locked because the maximum number of verification
    /// attempts has been exceeded.
    ///
    /// <para>
    /// Locked challenges are permanently invalid and are used
    /// to mitigate brute-force attacks.
    /// </para>
    /// </summary>
    Locked = 4,

    /// <summary>
    /// Indicates that the authentication challenge has been
    /// revoked by a security policy or administrative action.
    ///
    /// <para>
    /// Examples include password changes, account suspension,
    /// security stamp invalidation, session revocation,
    /// administrative intervention, or other security events
    /// that invalidate the authentication process.
    /// </para>
    /// </summary>
    Revoked = 5
}