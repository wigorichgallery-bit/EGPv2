// ===========================================
// File Location :
// src/Web/Platform.WebApi/Controllers/
// BaseApiController.cs
// ===========================================
using Platform.SharedKernel.Results;
using Platform.WebApi.Contracts;

namespace Platform.WebApi.Controllers;

/// <summary>
/// Base controller for API endpoints.
///
/// Responsibility:
/// - Convert Result to ActionResult.
/// - Convert Result&lt;T&gt; to ActionResult.
/// - Provide consistent API responses.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No orchestration logic.
///
/// Side Effects:
/// - None.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class BaseApiController
    : ControllerBase
{
    /// <summary>
    /// Converts a Result into an HTTP response.
    /// </summary>
    /// <param name="result">
    /// Operation result.
    /// </param>
    /// <returns>
    /// HTTP action result.
    /// </returns>
    protected ActionResult FromResult(
        Result result)
    {
        ArgumentNullException.ThrowIfNull(
            result);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return CreateFailureResult(result.Error);
    }

    /// <summary>
    /// Converts a Result&lt;TValue&gt; into an
    /// HTTP response.
    /// </summary>
    /// <typeparam name="TValue">
    /// Result value type.
    /// </typeparam>
    /// <param name="result">
    /// Operation result.
    /// </param>
    /// <returns>
    /// HTTP action result.
    /// </returns>
    protected ActionResult FromResult<TValue>(
        Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(
            result);

        if (result.IsSuccess)
        {
            return Ok(
                result.Value);
        }

        return CreateFailureResult(result.Error);
    }

    /// <summary>
    /// Creates an ActionResult based on the provided Error object.
    /// </summary>
    /// <param name="error">The error object.</param>
    /// <returns>The action result.</returns>
    private ActionResult CreateFailureResult(
    Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        var response =
            new ApiErrorResponse(
                error.Code,
                error.Message,
                HttpContext.TraceIdentifier);

        return error.Type switch
        {
            ErrorType.Validation
                => BadRequest(response),

            ErrorType.Unauthorized
                => Unauthorized(response),

            ErrorType.Forbidden
                => StatusCode(
                    StatusCodes.Status403Forbidden,
                    response),

            ErrorType.NotFound
                => NotFound(response),

            ErrorType.Conflict
                => Conflict(response),

            _ => StatusCode(
                StatusCodes.Status500InternalServerError,
                response)
        };
    }
}