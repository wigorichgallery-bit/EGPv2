using System.Text.RegularExpressions;

namespace Platform.Communication.Validation;

/// <summary>
/// Provides structural validation for E.164 phone numbers.
///
/// Responsibility:
/// - Validate international phone number syntax.
/// - Stateless.
/// - Reusable across Communication value objects.
///
/// This validator does not perform:
/// - Number existence validation
/// - SIM verification
/// - WhatsApp account lookup
/// - SMS provider validation
/// - Carrier lookup
/// </summary>
internal static partial class E164PhoneNumberValidator
{
    /// <summary>
    /// Determines whether the specified phone number is a valid
    /// E.164 formatted number.
    /// </summary>
    /// <param name="value">
    /// Phone number.
    /// </param>
    /// <returns>
    /// <c>true</c> when the phone number is structurally valid;
    /// otherwise <c>false</c>.
    /// </returns>
    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return E164Regex().IsMatch(value);
    }

    [GeneratedRegex(
        @"^\+[1-9]\d{1,14}$",
        RegexOptions.CultureInvariant)]
    private static partial Regex E164Regex();
}