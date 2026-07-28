// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// ValueObjects/PermissionId.cs
// ===========================================
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a strongly typed permission identifier.
///
/// Responsibility:
/// - Encapsulate permission identifier.
/// - Eliminate magic strings.
/// - Guarantee structural equality.
/// - Validate permission format.
///
/// Architectural Rules:
/// - Immutable.
/// - Value Object.
/// - No business behavior.
/// - No infrastructure dependency.
///
/// Supported Formats:
/// - MODULE.ACTION
/// - MODULE.SUBMODULE.ACTION
/// - MODULE.SUBMODULE.SUBMODULE.ACTION
///
/// Examples:
/// - USER.CREATE
/// - USER.UPDATE
/// - ROLE.DELETE
/// - IDENTITY.USER.CREATE
///
/// Invariants:
/// - Value cannot be null.
/// - Value cannot be empty.
/// - Value must follow permission naming convention.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
public sealed class PermissionId
    : ValueObject
{
    /// <summary>
    /// Permission format validator.
    /// </summary>
    private static readonly Regex PermissionPattern =
        new(
            @"^[A-Z]+(\.[A-Z]+)+$",
            RegexOptions.Compiled);

    /// <summary>
    /// Gets permission identifier.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of
    /// <see cref="PermissionId"/>.
    /// </summary>
    /// <param name="value">
    /// Permission identifier.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when permission identifier
    /// is invalid.
    /// </exception>
    public PermissionId(
        string value)
    {
        Guard.AgainstNullOrWhiteSpace(
            value,
            nameof(value));

        value = value.Trim().ToUpperInvariant();

        if (!PermissionPattern.IsMatch(value))
        {
            throw new ArgumentException(
                "Invalid permission identifier.",
                nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Returns permission identifier.
    /// </summary>
    /// <returns>
    /// Permission identifier.
    /// </returns>
    public override string ToString()
    {
        return Value;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?>
        GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    /// Creates a permission identifier.
    /// </summary>
    /// <param name="value">
    /// Permission identifier.
    /// </param>
    public static implicit operator PermissionId(
        string value)
    {
        return new PermissionId(
            value);
    }

    /// <summary>
    /// Returns permission identifier value.
    /// </summary>
    /// <param name="permissionId">
    /// Permission identifier.
    /// </param>
    public static implicit operator string(
        PermissionId permissionId)
    {
        return permissionId.Value;
    }
}