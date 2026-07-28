// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// Constants/SystemRoleIds.cs
// ===========================================
namespace Platform.Identity.Domain.Constants;

/// <summary>
/// Provides immutable identifiers for
/// built-in system roles.
///
/// Responsibility:
/// - Define stable role identifiers.
/// - Prevent environment-specific role IDs.
/// - Support cross-module references.
///
/// Architectural Rules:
/// - Domain constant.
/// - Immutable.
/// - No business logic.
/// - No infrastructure dependency.
///
/// Thread Safety:
/// - Thread-safe.
/// </summary>
public static class SystemRoleIds
{
    /// <summary>
    /// System Administrator role identifier.
    /// </summary>
    public static readonly Guid
        SystemAdministrator =
            Guid.Parse(
                "A8A5C41E-9F42-4E55-BEE8-000000000001");

    /// <summary>
    /// Governance Administrator role identifier.
    /// </summary>
    public static readonly Guid
        GovernanceAdministrator =
            Guid.Parse(
                "A8A5C41E-9F42-4E55-BEE8-000000000002");

    /// <summary>
    /// Security Administrator role identifier.
    /// </summary>
    public static readonly Guid
        SecurityAdministrator =
            Guid.Parse(
                "A8A5C41E-9F42-4E55-BEE8-000000000003");

    /// <summary>
    /// Auditor role identifier.
    /// </summary>
    public static readonly Guid
        Auditor =
            Guid.Parse(
                "A8A5C41E-9F42-4E55-BEE8-000000000004");

    /// <summary>
    /// Operator role identifier.
    /// </summary>
    public static readonly Guid
        Operator =
            Guid.Parse(
                "A8A5C41E-9F42-4E55-BEE8-000000000005");
}