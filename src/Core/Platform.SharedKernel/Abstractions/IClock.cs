// ===========================================
// File Location : src/Core/Platform.SharedKernel/Abstractions/IClock.cs
// ===========================================

namespace Platform.SharedKernel.Abstractions;

   /// <summary>
   /// Provides abstraction for system time.
   /// 
   /// Responsibility:
   /// - Centralize time retrieval.
   /// - Ensure deterministic testing.
   /// - Prevent direct DateTime.UtcNow usage in domain.
   /// 
   /// Architectural Rule:
   /// - Domain may depend on IClock.
   /// - Infrastructure provides concrete implementation.
   /// 
   /// Invariants:
   /// - All returned time must be UTC.
   /// </summary>
    public interface IClock
{
    /// <summary>
    /// Gets current UTC time.
    /// 
    /// Business Rule:
    /// - Must always return DateTimeKind.Utc.
    /// - Must not return local time.
    /// </summary>
    DateTime UtcNow { get; }
}