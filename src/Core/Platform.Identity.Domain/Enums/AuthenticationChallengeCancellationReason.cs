namespace Platform.Identity.Domain.Enums;

/// <summary>
/// Specifies the reason an authentication challenge
/// was cancelled.
/// </summary>
public enum AuthenticationChallengeCancellationReason
{
    /// <summary>
    /// Cancelled by the authenticated user.
    /// </summary>
    UserCancelled = 0,

    /// <summary>
    /// Cancelled by the authentication system.
    /// </summary>
    SystemCancelled = 1,

    /// <summary>
    /// Replaced by a newer authentication challenge.
    /// </summary>
    Superseded = 2,

    /// <summary>
    /// Cancelled because the associated session ended.
    /// </summary>
    SessionEnded = 3,

    /// <summary>
    /// Cancelled by an administrator.
    /// </summary>
    AdministratorCancelled = 4
}