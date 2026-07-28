// ===========================================
// File Location : src/Core/Platform.SharedKernel/Base/ValueObject.cs
// ===========================================
namespace Platform.SharedKernel.Base;

/// <summary>
/// Represents a Value Object in the domain model.
/// 
/// Responsibility:
/// - Implements structural equality.
/// - Enforces immutability by convention.
/// 
/// Invariants:
/// - Equality determined by atomic values.
/// - Must override GetAtomicValues().
/// 
/// Side Effects:
/// - None.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Returns atomic values used for equality comparison.
    /// 
    /// Business Rule:
    /// - Must return ordered components.
    /// - Must not return null.
    /// </summary>
    protected abstract IEnumerable<object?> GetAtomicValues();

    /// <summary>
    /// Determines equality using structural comparison.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other)
            return false;

        if (GetType() != other.GetType())
            return false;

        return GetAtomicValues()
            .SequenceEqual(other.GetAtomicValues());
    }

    /// <summary>
    /// Returns hash code based on atomic values.
    /// </summary>
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(0, (hash, value) =>
            {
                unchecked
                {
                    return (hash * 23) + (value?.GetHashCode() ?? 0);
                }
            });
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}