// ===========================================
// File Location :
// src/Application/Platform.Pipeline/
// Abstractions/ICommand.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Represents an application command that
/// performs a state-changing operation.
///
/// Responsibility:
/// - Identify a command request.
/// - Represent write-side intent.
/// - Participate in pipeline execution.
///
/// Architectural Rules:
/// - Marker interface only.
/// - No behavior.
/// - No infrastructure dependency.
/// - No persistence dependency.
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
public interface ICommand
{
}