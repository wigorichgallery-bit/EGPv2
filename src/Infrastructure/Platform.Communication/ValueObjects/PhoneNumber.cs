using System.Collections.Generic;

using Platform.Communication.Validation;
using Platform.SharedKernel.Base;

namespace Platform.Communication.ValueObjects;

/// <summary>
/// Represents a valid phone number in E.164 format.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    /// <summary>
    /// Gets the phone number.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="value">Phone number.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the phone number format is invalid.
    /// </exception>
    public PhoneNumber(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Trim();

        if (!E164PhoneNumberValidator.IsValid(value))
        {
            throw new ArgumentException(
                "Phone number must follow E.164 format.",
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