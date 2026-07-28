// ===========================================
// File Location :
// src/Web/Platform.WebApi/Controllers/
// RolesController.cs
// ===========================================
using Platform.Identity.Application.Features.Roles.Actions;
using Platform.Pipeline.Abstractions;

namespace Platform.WebApi.Controllers;

/// <summary>
/// Role management endpoints.
///
/// Responsibility:
/// - Receive HTTP requests.
/// - Execute pipeline.
/// - Execute use cases.
/// - Return HTTP responses.
///
/// Architectural Rules:
/// - No business logic.
/// - No persistence logic.
/// - No domain logic.
/// - No infrastructure logic.
///
/// Side Effects:
/// - None.
/// </summary>
[Route("api/roles")]
public sealed class RolesController
    : BaseApiController
{
    private readonly IPipelineExecutor _pipeline;

    private readonly AssignRoleUseCase _assignRoleUseCase;

    private readonly RemoveRoleUseCase _removeRoleUseCase;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="RolesController"/> class.
    /// </summary>
    /// <param name="pipeline">
    /// Pipeline executor.
    /// </param>
    /// <param name="assignRoleUseCase">
    /// Assign role use case.
    /// </param>
    /// <param name="removeRoleUseCase">
    /// Remove role use case.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when dependency is null.
    /// </exception>
    public RolesController(
        IPipelineExecutor pipeline,
        AssignRoleUseCase assignRoleUseCase,
        RemoveRoleUseCase removeRoleUseCase)
    {
        _pipeline =
            pipeline
            ?? throw new ArgumentNullException(
                nameof(pipeline));

        _assignRoleUseCase =
            assignRoleUseCase
            ?? throw new ArgumentNullException(
                nameof(assignRoleUseCase));

        _removeRoleUseCase =
            removeRoleUseCase
            ?? throw new ArgumentNullException(
                nameof(removeRoleUseCase));
    }

    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    /// <param name="command">
    /// Assign role command.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// HTTP action result.
    /// </returns>
    [HttpPost("assign")]
    public async Task<ActionResult> AssignRole(
        AssignRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _assignRoleUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(
            result);
    }

    /// <summary>
    /// Removes a role from a user.
    /// </summary>
    /// <param name="command">
    /// Remove role command.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// HTTP action result.
    /// </returns>
    [HttpPost("remove")]
    public async Task<ActionResult> RemoveRole(
        RemoveRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _removeRoleUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(
            result);
    }
}