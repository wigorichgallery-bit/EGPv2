// ===========================================
// File Location : src/Core/Platform.SharedKernel/Results/Result.cs
// ===========================================
namespace Platform.SharedKernel.Results;

/// <summary>
/// Represents operation result without return value.
/// 
/// Responsibility:
/// - Encapsulates success/failure.
/// - Prevents exception-driven flow.
/// 
/// Invariants:
/// - Success must contain Error.None.
/// - Failure must not contain Error.None.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets success indicator.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets failure indicator.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets associated error.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Initializes result instance.
    /// </summary>
    /// <param name="isSuccess">Success flag.</param>
    /// <param name="error">Error object.</param>
    /// <exception cref="InvalidOperationException">Thrown if invariant violated.</exception>
    protected Result(bool isSuccess, Error error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));

        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Success result must contain Error.None.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failure result must contain actual error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates successful result.
    /// </summary>
    /// <returns>Success result.</returns>
    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    /// <summary>
    /// Creates failure result.
    /// </summary>
    /// <param name="error">Error object.</param>
    /// <returns>Failure result.</returns>
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}