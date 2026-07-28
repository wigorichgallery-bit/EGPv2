// ===========================================
// File Location : src/Core/Platform.Identity.Domain/Enums/UserStatus.cs
// ===========================================
namespace Platform.Identity.Domain.Enums;

/// <summary>
/// Represents user lifecycle state.
/// 
/// States:
/// - Active: Can login.
/// - Locked: Temporarily locked due to failed attempts.
/// - Disabled: Disabled by administrator.
/// 
/// Invariants:
/// - Must follow state transition rules enforced in aggregate.
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// User allowed to authenticate.
    /// </summary>
    Active = 1,

    /// <summary>
    /// User temporarily locked.
    /// </summary>
    Locked = 2,

    /// <summary>
    /// User administratively disabled.
    /// </summary>
    Disabled = 3
}