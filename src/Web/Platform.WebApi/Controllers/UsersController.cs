// ===========================================
// File Location :
// src/Web/Platform.WebApi/Controllers/
// UsersController.cs
// ===========================================
using Platform.Identity.Application.Features.Users.Actions;
using Platform.Pipeline.Abstractions;

namespace Platform.WebApi.Controllers;

/// <summary>
/// User management endpoints.
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
/// </summary>
[Route("api/users")]
public sealed class UsersController
    : BaseApiController
{
    private readonly IPipelineExecutor _pipeline;

    private readonly CreateUserUseCase _createUserUseCase;
    private readonly ChangePasswordUseCase _changePasswordUseCase;
    private readonly EnableMfaUseCase _enableMfaUseCase;
    private readonly DisableMfaUseCase _disableMfaUseCase;
    private readonly VerifyEmailUseCase _verifyEmailUseCase;
    private readonly VerifyPhoneUseCase _verifyPhoneUseCase;

    /// <summary>
    /// Initializes controller.
    /// </summary>
    public UsersController(
        IPipelineExecutor pipeline,
        CreateUserUseCase createUserUseCase,
        ChangePasswordUseCase changePasswordUseCase,
        EnableMfaUseCase enableMfaUseCase,
        DisableMfaUseCase disableMfaUseCase,
        VerifyEmailUseCase verifyEmailUseCase,
        VerifyPhoneUseCase verifyPhoneUseCase)
    {
        _pipeline = pipeline
            ?? throw new ArgumentNullException(
                nameof(pipeline));

        _createUserUseCase = createUserUseCase
            ?? throw new ArgumentNullException(
                nameof(createUserUseCase));

        _changePasswordUseCase = changePasswordUseCase
            ?? throw new ArgumentNullException(
                nameof(changePasswordUseCase));

        _enableMfaUseCase = enableMfaUseCase
            ?? throw new ArgumentNullException(
                nameof(enableMfaUseCase));

        _disableMfaUseCase = disableMfaUseCase
            ?? throw new ArgumentNullException(
                nameof(disableMfaUseCase));

        _verifyEmailUseCase = verifyEmailUseCase
            ?? throw new ArgumentNullException(
                nameof(verifyEmailUseCase));

        _verifyPhoneUseCase = verifyPhoneUseCase
            ?? throw new ArgumentNullException(
                nameof(verifyPhoneUseCase));
    }

    /// <summary>
    /// Creates a user account.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateUser(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync<
                CreateUserCommand,
                Guid>(
                command,
                () => _createUserUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }

    /// <summary>
    /// Changes password.
    /// </summary>
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _changePasswordUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }

    /// <summary>
    /// Enables MFA.
    /// </summary>
    [HttpPost("enable-mfa")]
    public async Task<ActionResult> EnableMfa(
        EnableMfaCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _enableMfaUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }

    /// <summary>
    /// Disables MFA.
    /// </summary>
    [HttpPost("disable-mfa")]
    public async Task<ActionResult> DisableMfa(
        DisableMfaCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _disableMfaUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }

    /// <summary>
    /// Verifies email.
    /// </summary>
    [HttpPost("verify-email")]
    public async Task<ActionResult> VerifyEmail(
        VerifyEmailCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _verifyEmailUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }

    /// <summary>
    /// Verifies phone.
    /// </summary>
    [HttpPost("verify-phone")]
    public async Task<ActionResult> VerifyPhone(
        VerifyPhoneCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _pipeline.ExecuteAsync(
                command,
                () => _verifyPhoneUseCase.ExecuteAsync(
                    command,
                    cancellationToken),
                cancellationToken);

        return FromResult(result);
    }
}