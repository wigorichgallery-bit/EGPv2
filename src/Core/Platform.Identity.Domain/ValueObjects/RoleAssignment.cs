// ===========================================
// File Location : src/Core/Platform.Identity.Domain/ValueObjects/RoleAssignment.cs
// ===========================================
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a role binding to a user.
/// 
/// Responsibility:
/// - Encapsulates RoleId.
/// - Guarantees immutability.
/// - Ensures structural equality.
/// 
/// Invariants:
/// - RoleId must not be empty.
/// 
/// Side Effects:
/// - None.
/// </summary>
public sealed class RoleAssignment : ValueObject
{
    /// <summary>
    /// Assigned role identifier.
    /// </summary>
    public Guid RoleId { get; }

    /// <summary>
    /// Initializes role assignment.
    /// </summary>
    /// <param name="roleId">Role identifier.</param>
    public RoleAssignment(Guid roleId)
    {
        Guard.AgainstEmpty(roleId, nameof(roleId));
        RoleId = roleId;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RoleId;
    }
}