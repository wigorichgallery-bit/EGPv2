// ===========================================
// File Location : src/Core/Platform.Identity.Domain/ValueObjects/EmailAddress.cs
// ===========================================
using Platform.SharedKernel.Base;
using Platform.SharedKernel.Exceptions;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a validated email address value object.
/// 
/// Responsibility:
/// - Encapsulates email format validation.
/// - Guarantees immutability.
/// - Provides structural equality.
/// 
/// Invariants:
/// - Must not be null or whitespace.
/// - Must match standard email format pattern.
/// - Stored in normalized lowercase form.
/// 
/// Side Effects:
/// - Throws DomainException on invariant violation.
/// </summary>
public sealed class EmailAddress : ValueObject
{
    /// <summary>
    /// RFC 5322 simplified email validation pattern.
    /// </summary>
    private static readonly Regex _emailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Normalized email value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EmailAddress"/>.
    /// 
    /// Validation:
    /// - Reject null or whitespace.
    /// - Reject invalid email format.
    /// - Normalize to lowercase.
    /// 
    /// Failure Condition:
    /// - Throws DomainException if format invalid.
    /// </summary>
    /// <param name="value">Raw email input.</param>
    /// <exception cref="DomainException">Thrown when email format invalid.</exception>
    public EmailAddress(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!_emailRegex.IsMatch(normalized))
        {
            throw new DomainException("IDENTITY.INVALID_EMAIL", "Email format is invalid.");
        }

        Value = normalized;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    /// Returns the string representation.
    /// </summary>
    /// <returns>Email value.</returns>
    public override string ToString()
    {
        return Value;
    }
}