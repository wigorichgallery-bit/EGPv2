// ===========================================
// File Location : src/Core/Platform.SharedKernel/Utilities/Guard.cs
// ===========================================

namespace Platform.SharedKernel.Utilities;

/// <summary>
/// Provides guard clause validation utilities.
/// 
/// Responsibility:
/// - Centralize defensive programming.
/// - Throw ArgumentException / ArgumentNullException.
/// 
/// Usage:
/// - Used inside domain and application layer.
/// - Not for business rule validation (DomainException).
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures value is not null.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="value">Value to check.</param>
    /// <param name="paramName">Parameter name.</param>
    /// <exception cref="ArgumentNullException">Thrown if null.</exception>
    public static void AgainstNull<T>(T? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
    }

    /// <summary>
    /// Ensures string is not null or whitespace.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <param name="paramName">Parameter name.</param>
    public static void AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
    }

    /// <summary>
    /// Ensures Guid is not empty.
    /// </summary>
    /// <param name="value">Guid value.</param>
    /// <param name="paramName">Parameter name.</param>
    public static void AgainstEmpty(Guid value, string paramName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Guid cannot be empty.", paramName);
    }

    /// <summary>
    /// Ensures condition is true.
    /// </summary>
    /// <param name="condition">Condition.</param>
    /// <param name="message">Message.</param>
    public static void AgainstFalse(bool condition, string message)
    {
        if (!condition)
            throw new ArgumentException(message);
    }
}