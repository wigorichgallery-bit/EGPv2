// ===========================================
// File Location : src/Core/Platform.Identity.Domain/ValueObjects/PhoneNumber.cs
// ===========================================

using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;
using System.Text.RegularExpressions;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents an E.164 compliant phone number.
/// 
/// Responsibility:
/// - Enforces WhatsApp/SMS compatible E.164 format.
/// - Ensures immutability.
/// 
/// Invariants:
/// - Must start with '+'.
/// - Must contain country code.
/// - Length between 8 and 15 digits.
/// 
/// Side Effects:
/// - Throws DomainException on invalid format.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    /// <summary>
    /// E.164 validation pattern.
    /// </summary>
    private static readonly Regex _phoneRegex =
        new(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Normalized phone number.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes phone number.
    /// </summary>
    /// <param name="value">Raw phone input.</param>
    /// <exception cref="DomainException">Thrown when format invalid.</exception>
    public PhoneNumber(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(value));

        var normalized = value.Trim();

        if (!_phoneRegex.IsMatch(normalized))
        {
            throw new DomainException("IDENTITY.INVALID_PHONE", "Phone number must follow E.164 format.");
        }

        Value = normalized;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    /// Returns string representation.
    /// </summary>
    public override string ToString()
    {
        return Value;
    }
}