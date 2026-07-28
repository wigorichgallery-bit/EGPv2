using System.Collections.Generic;

using Platform.Communication.Validation;
using Platform.SharedKernel.Base;

namespace Platform.Communication.ValueObjects;

/// <summary>
/// Represents a valid WhatsApp recipient number in E.164 format.
/// </summary>
public sealed class WhatsAppNumber : ValueObject
{
    /// <summary>
    /// Gets the WhatsApp number.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WhatsAppNumber"/> class.
    /// </summary>
    /// <param name="value">
    /// WhatsApp number.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the WhatsApp number format is invalid.
    /// </exception>
    public WhatsAppNumber(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Trim();

        if (!E164PhoneNumberValidator.IsValid(value))
        {
            throw new ArgumentException(
                "WhatsApp number must follow E.164 format.",
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