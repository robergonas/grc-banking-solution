using GRC.Identity.Application.Commands.ChangePassword;
using GRC.Identity.Application.Commands.LoginUser;
using GRC.Identity.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GRC.Identity.API.Controllers;

/// <summary>
/// Controlador de autenticación y registro de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            _logger.LogInformation("Registering new user with email: {Email}", command.Email);
            var userId = await _mediator.Send(command);

            var response = new RegisterUserResponse { UserId = userId };
            return CreatedAtAction(nameof(Register), response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", command.Email);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Autentica un usuario y devuelve un token JWT
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", command.Email);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Failed login attempt for email: {Email}", command.Email);
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", command.Email);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cambia la contraseña de un usuario autenticado
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        try
        {
            _logger.LogInformation("Password change request for user: {UserId}", command.UserId);
            await _mediator.Send(command);
            return Ok(new { message = "Contraseña cambiada exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", command.UserId);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Verifica si el token JWT es válido
    /// </summary>
    [HttpGet("verify-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult VerifyToken()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName = User.FindFirst("fullName")?.Value;

            _logger.LogInformation("Token verified for user: {Email}", email);

            return Ok(new
            {
                isValid = true,
                userId = userId,
                email = email,
                fullName = fullName,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying token");
            return BadRequest(new { message = ex.Message });
        }
    }
}