// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/IQuery.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Represents an application query that
/// retrieves information without modifying
/// application state.
///
/// Responsibility:
/// - Identify a read request.
/// - Represent query-side intent.
/// - Participate in query pipeline execution.
///
/// Architectural Rules:
/// - Marker interface only.
/// - Read-only.
/// - No behavior.
/// - No persistence dependency.
/// - No infrastructure dependency.
/// - No domain logic.
///
/// Side Effects:
/// - None.
///
/// Thread Safety:
/// - Immutable implementations are recommended.
///
/// Complexity:
/// O(1)
/// </summary>
public interface IQuery
{
}