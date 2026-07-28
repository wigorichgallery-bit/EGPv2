// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/IQueryT.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Represents an application query that
/// retrieves information and returns
/// a response.
///
/// Responsibility:
/// - Identify a read request.
/// - Define response type.
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
/// <typeparam name="TResult">
/// Query response type.
/// </typeparam>
public interface IQuery<TResult>
{
}