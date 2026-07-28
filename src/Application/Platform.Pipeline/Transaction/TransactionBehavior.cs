// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Transaction/TransactionBehavior.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Abstractions;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Transaction;

/// <summary>
/// Transaction pipeline behavior for requests
/// returning Result.
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
public sealed class TransactionBehavior<TRequest>
    : IPipelineBehavior<TRequest>, IPipelineOrdered
{
    private readonly IUnitOfWork _unitOfWork;
    public int Order => 300;
    /// <summary>
    /// Initializes behavior.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unit of work instance.
    /// </param>
    public TransactionBehavior(
        IUnitOfWork unitOfWork)
    {
        Guard.AgainstNull(
            unitOfWork,
            nameof(unitOfWork));

        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Result> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        Func<Task<Result>> next)
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