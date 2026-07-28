// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Abstractions/IPipelineOrdered.cs
// ===========================================
namespace Platform.Pipeline.Abstractions;

/// <summary>
/// Defines execution order for pipeline behaviors.
///
/// Responsibility:
/// - Provide deterministic pipeline execution order.
/// - Ensure behaviors execute in a predictable sequence.
/// - Support pipeline composition.
///
/// Architectural Rules:
/// - Lower values execute first.
/// - Order values must be unique.
/// - Used by pipeline orchestration only.
///
/// Examples:
/// - Validation = 100
/// - Governance = 200
/// - Transaction = 300
/// - Logging = 400
///
/// Side Effects:
/// - None.
/// </summary>
public interface IPipelineOrdered
{
    /// <summary>
    /// Gets pipeline execution order.
    ///
    /// Business Rules:
    /// - Lower values execute first.
    /// - Must remain stable across releases.
    /// </summary>
    int Order { get; }
}