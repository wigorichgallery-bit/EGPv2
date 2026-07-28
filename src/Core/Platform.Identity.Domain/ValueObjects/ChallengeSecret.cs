// ===========================================
// File Location :
// src/Core/Platform.Identity.Domain/
// ValueObjects/ChallengeSecret.cs
// ===========================================

using Platform.SharedKernel.Base;
using Platform.SharedKernel.Utilities;

namespace Platform.Identity.Domain.ValueObjects;

/// <summary>
/// Represents the protected secret associated with an
/// authentication challenge.
///
/// <para>
/// This value object encapsulates sensitive challenge data used
/// during authentication workflows, such as:
/// </para>
/// <list type="bullet">
/// <item><description>Hashed one-time password (OTP).</description></item>
/// <item><description>Encrypted TOTP secret.</description></item>
/// <item><description>Protected authentication challenge data.</description></item>
/// </list>
///
/// <para>
/// The internal representation is intentionally hidden from
/// consumers to prevent accidental disclosure.
/// </para>
///
/// <para>
/// This value object is immutable and participates in structural
/// equality.
/// </para>
/// </summary>
public sealed class ChallengeSecret : ValueObject
{
    /// <summary>
    /// Gets the protected challenge secret value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ChallengeSecret"/> class for ORM materialization.
    /// </summary>
    private ChallengeSecret()
    {
        Value = default!;
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ChallengeSecret"/> class.
    /// </summary>
    /// <param name="value">
    /// The protected challenge secret.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the value is null or whitespace.
    /// </exception>
    public ChallengeSecret(string value)
    {
        Guard.AgainstNullOrWhiteSpace(
            value,
            nameof(value));

        Value = value;
    }

    /// <summary>
    /// Returns the atomic values used for structural equality.
    /// </summary>
    /// <returns>
    /// The ordered collection of equality components.
    /// </returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    /// <summary>
    /// Returns a masked representation of the protected value.
    /// </summary>
    /// <returns>
    /// A masked string that never exposes the underlying secret.
    /// </returns>
    public override string ToString()
    {
        return "********";
    }
}