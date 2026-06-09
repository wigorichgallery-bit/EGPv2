// ===========================================
// File Location : src/Core/Platform.SharedKernel/Base/BaseEntity.cs
// =========================================== 

namespace Platform.SharedKernel.Base;

/// <summary>
/// Represents the base entity in the domain model.
/// 
/// Responsibility:
/// - Provides identity equality.
/// - Serves as base class for all entities.
/// 
/// Invariants:
/// - Id must not be empty.
/// - Identity is immutable after construction.
/// 
/// Side Effects:
/// - None.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// Constraint:
    /// - Must not be Guid.Empty.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class.
    /// 
    /// Validation Logic:
    /// - Throws if id is Guid.Empty.
    /// </summary>
    /// <param name="id">Entity unique identifier.</param>
    /// <exception cref="ArgumentException">Thrown if id is empty.</exception>
    protected BaseEntity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Entity Id cannot be empty.", nameof(id));

        Id = id;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// 
    /// Business Rule:
    /// - Two entities are equal if they share the same type and Id.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
            return false;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    /// <summary>
    /// Returns the hash code for this entity.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !Equals(left, right);
    }
}