// ===========================================
// File Location : src/Core/Platform.SharedKernel/Results/ResultT.cs
// ===========================================

namespace Platform.SharedKernel.Results;

/// <summary>
/// Represents operation result with return value.
/// 
/// Responsibility:
/// - Encapsulate success/failure.
/// - Carry value when successful.
/// </summary>
public sealed class Result<T> : Result
{
    /// <summary>
    /// Gets result value.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Initializes generic result.
    /// </summary>
    private Result(T value)
        : base(true, Error.None)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes failure generic result.
    /// </summary>
    private Result(Error error)
        : base(false, error)
    {
        Value = default!;
    }

    /// <summary>
    /// Creates success result with value.
    /// </summary>
    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new Result<T>(value);
    }

    /// <summary>
    /// Creates failure result.
    /// </summary>
    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }
}