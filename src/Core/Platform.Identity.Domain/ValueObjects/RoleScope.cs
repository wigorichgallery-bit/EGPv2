// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// ValueObjects/RoleScope.cs
// ===========================================
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents the scope in which a role
/// is valid within the Enterprise
/// Governance Platform.
///
/// Responsibility:
/// - Encapsulate role scope.
/// - Eliminate magic strings.
/// - Guarantee structural equality.
/// - Restrict role scope to supported
///   platform values.
/// - Support authorization and
///   governance boundaries.
///
/// Architectural Rules:
/// - Immutable.
/// - Strongly Typed Value Object.
/// - No business behavior.
/// - No infrastructure dependency.
/// - No persistence dependency.
///
/// Supported Scopes:
/// - GLOBAL
/// - TENANT
/// - ORGANIZATION
/// - BUSINESS_UNIT
/// - DEPARTMENT
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable.
/// </summary>
public sealed class RoleScope
    : ValueObject
{
    /// <summary>
    /// Gets the global scope.
    /// </summary>
    public static RoleScope Global { get; } =
        new("GLOBAL");

    /// <summary>
    /// Gets the tenant scope.
    /// </summary>
    public static RoleScope Tenant { get; } =
        new("TENANT");

    /// <summary>
    /// Gets the organization scope.
    /// </summary>
    public static RoleScope Organization { get; } =
        new("ORGANIZATION");

    /// <summary>
    /// Gets the business unit scope.
    /// </summary>
    public static RoleScope BusinessUnit { get; } =
        new("BUSINESS_UNIT");

    /// <summary>
    /// Gets the department scope.
    /// </summary>
    public static RoleScope Department { get; } =
        new("DEPARTMENT");

    /// <summary>
    /// Supported scope values.
    /// </summary>
    private static readonly HashSet<string>
        SupportedScopes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            Global.Value,
            Tenant.Value,
            Organization.Value,
            BusinessUnit.Value,
            Department.Value
        };

    /// <summary>
    /// Gets scope value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="RoleScope"/> class.
    /// </summary>
    /// <param name="value">
    /// Scope value.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied scope
    /// is invalid.
    /// </exception>
    public RoleScope(
        string value)
    {
        Guard.AgainstNullOrWhiteSpace(
            value,
            nameof(value));

        value =
            value
                .Trim()
                .ToUpperInvariant();

        if (!SupportedScopes.Contains(value))
        {
            throw new ArgumentException(
                $"Unsupported role scope '{value}'.",
                nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Creates a role scope from its
    /// string representation.
    /// </summary>
    /// <param name="value">
    /// Scope value.
    /// </param>
    /// <returns>
    /// Matching <see cref="RoleScope"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when scope is invalid.
    /// </exception>
    public static RoleScope From(
        string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            value);

        return value
            .Trim()
            .ToUpperInvariant() switch
        {
            "GLOBAL" => Global,
            "TENANT" => Tenant,
            "ORGANIZATION" => Organization,
            "BUSINESS_UNIT" => BusinessUnit,
            "DEPARTMENT" => Department,

            _ => throw new ArgumentException(
                $"Unsupported role scope '{value}'.",
                nameof(value))
        };
    }

    /// <summary>
    /// Returns the scope value.
    /// </summary>
    /// <returns>
    /// Scope string.
    /// </returns>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Converts string into
    /// <see cref="RoleScope"/>.
    /// </summary>
    /// <param name="value">
    /// Scope value.
    /// </param>
    public static implicit operator RoleScope(
        string value)
    {
        return From(value);
    }

    /// <summary>
    /// Converts
    /// <see cref="RoleScope"/>
    /// into string.
    /// </summary>
    /// <param name="scope">
    /// Role scope.
    /// </param>
    public static implicit operator string(
        RoleScope scope)
    {
        ArgumentNullException.ThrowIfNull(
            scope);

        return scope.Value;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?>
        GetAtomicValues()
    {
        yield return Value;
    }
}