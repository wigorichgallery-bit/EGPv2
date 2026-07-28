// ===========================================
// File Location :
// src/Application/Platform.Pipeline/Validation/ValidationBehavior.cs
// ===========================================
using Platform.Pipeline.Abstractions;
using Platform.SharedKernel.Results;
using Platform.SharedKernel.Utilities;

namespace Platform.Pipeline.Validation;

/// <summary>
/// Validation pipeline behavior for requests
/// returning Result.
///
/// Responsibility:
/// - Resolve validators.
/// - Execute validators.
/// - Aggregate validation failures.
/// - Stop pipeline on failure.
/// - Continue pipeline on success.
///
/// Side Effects:
/// - None.
/// </summary>
/// <typeparam name="TRequest">
/// Request type.
/// </typeparam>
public sealed class ValidationBehavior<TRequest>
    : IPipelineBehavior<TRequest>, IPipelineOrdered
{
    private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;
    public int Order => 100;
    
    /// <summary>
    /// Initializes behavior.
    /// </summary>
    /// <param name="validators">
    /// Registered validators.
    /// </param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        Guard.AgainstNull(
            validators,
            nameof(validators));

        _validators = [.. validators];
    }

    /// <inheritdoc />
    public Task<Result> HandleAsync(
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

        if (_validators.Count == 0)
        {
            return next();
        }

        var errors =
            new List<ValidationError>();

        foreach (var validator in _validators)
        {
            var validationResult =
                validator.Validate(request);

            if (!validationResult.IsValid)
            {
                errors.AddRange(
                    validationResult.Errors);
            }
        }

        if (errors.Count > 0)
        {
            return Task.FromResult(
                ValidationFailureFactory
                    .CreateFailure(errors));
        }

        return next();
    }
}