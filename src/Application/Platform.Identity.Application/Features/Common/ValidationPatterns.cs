// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/Features/Common/ValidationPatterns.cs
//
// STEP-7B
// LOCKED
// ===========================================
namespace Platform.Identity.Application.Features.Common;

/// <summary>
/// Centralized validation patterns.
///
/// RESPONSIBILITY:
/// - Eliminate regex duplication.
/// - Ensure validation consistency.
/// - Provide reusable validation patterns.
///
/// SIDE EFFECTS:
/// - None.
///
/// COMPLEXITY:
/// - O(1)
/// </summary>
internal static class ValidationPatterns
{
    /// <summary>
    /// E.164 international phone number format.
    ///
    /// Examples:
    /// +6281234567890
    /// +14155552671
    /// </summary>
    public const string E164Phone =
        @"^\+[1-9]\d{1,14}$";
}