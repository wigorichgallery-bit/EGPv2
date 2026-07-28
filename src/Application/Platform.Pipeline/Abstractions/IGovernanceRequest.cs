// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IGovernanceRequest.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Represents a request that participates in
/// enterprise governance evaluation.
///
/// Responsibility:
/// - Provide governance metadata.
/// - Enable policy evaluation.
/// - Enable risk evaluation.
/// - Enable approval routing.
/// - Enable audit classification.
///
/// Side Effects:
/// - None.
/// </summary>
public interface IGovernanceRequest
{
    /// <summary>
    /// Gets governance policy identifier.
    ///
    /// Examples:
    /// - IDENTITY.USER.CREATE
    /// - IDENTITY.ROLE.ASSIGN
    /// - SECURITY.POLICY.UPDATE
    /// </summary>
    string GovernancePolicy { get; }

    /// <summary>
    /// Gets target resource name.
    ///
    /// Examples:
    /// - User
    /// - Role
    /// - Permission
    /// - Policy
    /// </summary>
    string Resource { get; }

    /// <summary>
    /// Gets requested action.
    ///
    /// Examples:
    /// - Create
    /// - Update
    /// - Delete
    /// - Assign
    /// - Approve
    /// </summary>
    string Action { get; }
}