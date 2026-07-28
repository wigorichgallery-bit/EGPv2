// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Transaction/TransactionBehaviorT.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Transaction;

/// <summary>
/// Transaction pipeline behavior for requests
/// returning Result&lt;TValue&gt;.
///
/// Responsibility:
/// - Execute transactional boundary.
/// - Commit successful operations.
/// - Rollback failed operations.
/// - Rollback exceptions.
///
/// Side Effects:
/// - Commits or rolls back UnitOfWork.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
/// <typeparam name="TValue">
/// Result value type.
/// </typeparam>
public sealed class TransactionBehaviorT<TRequest, TValue>
    : IPipelineBehavior<TRequest, TValue>, IPipelineOrdered
{
    private readonly IUnitOfWork _unitOfWork;
    public int Order => 300;
    /// <summary>
    /// Initializes behavior.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unit of work instance.
    /// </param>
    public TransactionBehaviorT(
        IUnitOfWork unitOfWork)
    {
        Guard.AgainstNull(
            unitOfWork,
            nameof(unitOfWork));

        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Result<TValue>> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result<TValue>>> next)
    {
        Guard.AgainstNull(
            request,
            nameof(request));

        Guard.AgainstNull(
            next,
            nameof(next));

        try
        {
            var result =
                await next();

            if (result.IsFailure)
            {
                await _unitOfWork.RollbackAsync(
                    cancellationToken);

                return result;
            }

            await _unitOfWork.CommitAsync(
                cancellationToken);

            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(
                cancellationToken);

            throw;
        }
    }
}