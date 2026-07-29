namespace Platform.SharedKernel.UnitTests.TestHelpers;

/// <summary>
/// Represents a reusable <see cref="ValueObject"/> implementation for unit tests.
///
/// <para>
/// This helper class is intended exclusively for validating the behavior of the
/// <see cref="ValueObject"/> base class. It provides two atomic values to verify
/// equality comparisons, hash code generation, and value-based semantics.
/// </para>
///
/// <para>
/// This class is part of the test infrastructure and must never be referenced
/// by production code.
/// </para>
/// </summary>
internal sealed class TestValueObject : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestValueObject"/> class.
    /// </summary>
    /// <param name="first">
    /// The first atomic value that participates in value equality.
    /// </param>
    /// <param name="second">
    /// The second atomic value that participates in value equality.
    /// </param>
    public TestValueObject(string first, int second)
    {
        First = first;
        Second = second;
    }

    /// <summary>
    /// Gets the first atomic value.
    /// </summary>
    public string First { get; }

    /// <summary>
    /// Gets the second atomic value.
    /// </summary>
    public int Second { get; }

    /// <summary>
    /// Returns the sequence of atomic values that define the equality of this
    /// value object.
    /// </summary>
    /// <returns>
    /// An ordered sequence of atomic values used by the
    /// <see cref="ValueObject"/> base class when performing equality
    /// comparisons and hash code generation.
    /// </returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return First;
        yield return Second;
    }
}

/// <summary>
/// Represents an alternative <see cref="ValueObject"/> implementation used to
/// verify comparisons between different value object types.
///
/// <para>
/// This helper enables unit tests to confirm that two value objects with
/// different runtime types are never considered equal, even when their
/// underlying values are similar.
/// </para>
///
/// <para>
/// This class is intended exclusively for unit testing and is not part of the
/// production domain model.
/// </para>
/// </summary>
internal sealed class OtherValueObject : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OtherValueObject"/> class.
    /// </summary>
    /// <param name="value">
    /// The atomic value used to determine value equality.
    /// </param>
    public OtherValueObject(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the atomic value of this value object.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the sequence of atomic values that define the equality of this
    /// value object.
    /// </summary>
    /// <returns>
    /// An ordered sequence of atomic values used by the
    /// <see cref="ValueObject"/> base class when performing equality
    /// comparisons and hash code generation.
    /// </returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}