using System.Collections.Generic;

using Platform.SharedKernel.Base;
using Platform.SharedKernel.Validation;

namespace Platform.Communication.ValueObjects;

/// <summary>
/// Represents a valid email address.
///
/// Responsibility:
/// - Encapsulates an email address as a Value Object.
/// - Guarantees structural validity.
///
/// Invariants:
/// - Email address cannot be null.
/// - Email address cannot be empty.
/// - Email address must be structurally valid.
/// </summary>
public sealed class EmailAddress : ValueObject
{
    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EmailAddress"/>.
    /// </summary>
    /// <param name="value">
    /// Email address.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the email address is invalid.
    /// </exception>
    public EmailAddress(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Trim();

        if (!EmailAddressValidator.IsValid(value))
        {
            throw new ArgumentException(
                "Email address format is invalid.",
                nameof(value));
        }

        Value = value;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value;
    }
}