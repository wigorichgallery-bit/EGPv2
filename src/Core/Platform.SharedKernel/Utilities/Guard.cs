// ===========================================
// File Location : src/Core/Platform.SharedKernel/Utilities/Guard.cs
// ===========================================
namespace Platform.SharedKernel.Utilities;

/// <summary>
/// Provides guard clause validation utilities.
///
/// Responsibility:
/// - Centralize defensive programming.
/// - Validate method arguments.
/// - Throw framework exceptions only.
/// - Never perform business rule validation.
///
/// Architectural Rules:
/// - Can be used by Domain and Application.
/// - Must never throw DomainException.
/// - Must remain stateless.
///
/// Usage:
/// - Validate parameters.
/// - Validate primitive values.
/// - Validate collections.
/// - Validate enum values.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures value is not null.
    /// </summary>
    public static void AgainstNull<T>(
        T? value,
        string paramName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>
    /// Ensures string is not null or whitespace.
    /// </summary>
    public static void AgainstNullOrWhiteSpace(
        string? value,
        string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Value cannot be null or whitespace.",
                paramName);
        }
    }

    /// <summary>
    /// Ensures Guid is not empty.
    /// </summary>
    public static void AgainstEmpty(
        Guid value,
        string paramName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(
                "Guid cannot be empty.",
                paramName);
        }
    }

    /// <summary>
    /// Ensures condition is true.
    /// </summary>
    public static void AgainstFalse(
        bool condition,
        string message)
    {
        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }

    /// <summary>
    /// Ensures the specified timestamp is expressed in UTC.
    /// </summary>
    public static void AgainstNonUtc(
        DateTime value,
        string paramName)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException(
                "Timestamp must be expressed in UTC.",
                paramName);
        }
    }

    /// <summary>
    /// Ensures the specified enumeration value is defined.
    /// </summary>
    public static void AgainstUndefinedEnum<TEnum>(
        TEnum value,
        string paramName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentException(
                $"Undefined enum value '{value}'.",
                paramName);
        }
    }

    /// <summary>
    /// Ensures the specified integer value is not negative.
    /// </summary>
    public static void AgainstNegative(
        int value,
        string paramName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                "Value cannot be negative.");
        }
    }

    /// <summary>
    /// Ensures the specified integer value is greater than zero.
    /// </summary>
    public static void AgainstNegativeOrZero(
        int value,
        string paramName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                "Value must be greater than zero.");
        }
    }

    /// <summary>
    /// Ensures the specified collection is not null or empty.
    /// </summary>
    public static void AgainstEmptyCollection<T>(
        IEnumerable<T>? collection,
        string paramName)
    {
        if (collection is null || !collection.Any())
        {
            throw new ArgumentException(
                "Collection cannot be null or empty.",
                paramName);
        }
    }

    /// <summary>
    /// Ensures the specified comparable value is within the given range.
    /// </summary>
    public static void AgainstOutOfRange<T>(
        T value,
        T minimum,
        T maximum,
        string paramName)
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0 ||
            value.CompareTo(maximum) > 0)
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                $"Value must be between {minimum} and {maximum}.");
        }
    }
}