// ===========================================
// File Location:
// src/Application/Platform.Identity.Application/
// Abstractions/
// Common/
// IGuidGenerator.cs
// ===========================================

namespace Platform.Identity.Application.Abstractions.Common;

/// <summary>
/// Generates globally unique identifiers for application
/// workflows.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generate globally unique identifiers.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provide an abstraction over identifier generation to
/// improve testability and infrastructure independence.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not depend on infrastructure.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not contain business rules.
/// </description>
/// </item>
/// <item>
/// <description>
/// Must not persist data.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Design Notes:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Implementations may generate identifiers using GUID,
/// sequential GUID, ULID, Snowflake, or any future identifier
/// strategy without affecting application workflows.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Thread Safety:
/// Implementations should be thread-safe.
/// </para>
/// </summary>
public interface IGuidGenerator
{
    /// <summary>
    /// Generates a new globally unique identifier.
    /// </summary>
    /// <returns>
    /// A newly generated unique identifier.
    /// </returns>
    Guid Create();
}