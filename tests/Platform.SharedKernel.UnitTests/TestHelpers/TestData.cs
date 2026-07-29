namespace Platform.SharedKernel.UnitTests.TestHelpers;

/// <summary>
/// Provides reusable deterministic data for unit tests.
///
/// <remarks>
/// <para>
/// Purpose:
/// Supplies predefined values that can be shared across multiple unit tests,
/// ensuring consistency, readability, and deterministic test execution.
/// </para>
///
/// <para>
/// Test data defined in this class should remain immutable and independent of
/// external state so that every test produces repeatable results.
/// </para>
///
/// <para>
/// Scope:
/// Test infrastructure only. This class must never be referenced by production
/// code.
/// </para>
/// </remarks>
internal static class TestData
{
    #region Common

    /// <summary>
    /// Represents a valid identifier that can be reused by unit tests.
    /// </summary>
    public static readonly Guid ValidId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    /// <summary>
    /// Represents an alternative valid identifier used when two distinct
    /// identifiers are required within the same test scenario.
    /// </summary>
    public static readonly Guid OtherId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    /// <summary>
    /// Represents a deterministic UTC timestamp that can be reused by unit
    /// tests requiring a valid UTC <see cref="DateTime"/>.
    /// </summary>
    public static readonly DateTime UtcNow =
        new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    #endregion

    #region Email

    /// <summary>
    /// Represents a syntactically valid email address for unit tests.
    /// </summary>
    public const string ValidEmail = "user@example.com";

    /// <summary>
    /// Represents an invalid email address for validation test scenarios.
    /// </summary>
    public const string InvalidEmail = "invalid-email";

    #endregion
}