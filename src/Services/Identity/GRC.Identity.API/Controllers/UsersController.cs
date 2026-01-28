using GRC.Identity.Application.Commands.ChangePassword;
using GRC.Identity.Application.Commands.ChangeUserRoles;
using GRC.Identity.Application.Commands.DeactivateUser;
using GRC.Identity.Application.Commands.UpdateUser;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Application.Queries.GetUserById;
using GRC.Identity.Application.Queries.GetUsers;
using GRC.Identity.Application.Queries.GetUsersByRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Identity.API.Controllers;

/// <summary>
/// Controlador de gestión de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("GetUsers")]
    [AllowAnonymous] // TEMPORAL para debug - QUITAR después
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            _logger.LogInformation("Getting users - Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}",
                pageNumber, pageSize, searchTerm ?? "none");

            // Log de autenticación
            _logger.LogInformation("User authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
            if (User.Identity?.IsAuthenticated == true)
            {
                _logger.LogInformation("User claims: {Claims}",
                    string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}")));
            }

            var query = new GetUsersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            _logger.LogInformation("Retrieved {Count} users", result.Items.Count());

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting user by ID: {UserId}", id);
            var query = new GetUserByIdQuery(id);// { UserId = id };
            var user = await _mediator.Send(query);

            if (user == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los usuarios
    /// </summary>
    //[HttpGet]
    //[Authorize(Roles = "Admin,Manager")]
    //[ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetAll()
    //{
    //    try
    //    {
    //        var query = new GetUsersQuery();
    //        var users = await _mediator.Send(query);

    //        return Ok(users);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error getting all users");
    //        return StatusCode(500, new { message = "Internal server error" });
    //    }
    //}

    /// <summary>
    /// Cambia los roles de un usuario
    /// </summary>
    [HttpPut("{id:guid}/roles")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeUserRoles(Guid id, [FromBody] ChangeUserRolesCommand command)
    {
        try
        {
            if (id != command.UserId)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del comando" });
            }

            _logger.LogInformation("Changing roles for user: {UserId}", id);
            await _mediator.Send(command);
            return Ok(new { message = "Roles actualizados exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "User not found: {UserId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing roles for user {UserId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene usuarios por rol
    /// </summary>
    [HttpGet("by-role/{roleId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(Guid roleId)
    {
        try
        {
            _logger.LogInformation("Getting users by role: {RoleId}", roleId);
            var query = new GetUsersByRoleQuery(roleId);// { RoleId = roleId };
            var users = await _mediator.Send(query);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users by role {RoleId}", roleId);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Desactiva un usuario
    /// </summary>
    [HttpPost("{id:guid}/deactivate")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeactivateUser(Guid id)
    {
        try
        {
            _logger.LogInformation("Deactivating user: {UserId}", id);
            var command = new DeactivateUserCommand { UserId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Usuario desactivado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "User not found: {UserId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user {UserId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene el perfil del usuario actual
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var query = new GetUserByIdQuery(userId);
            var user = await _mediator.Send(query);

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user profile");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Actualiza la información de un usuario
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
    {
        try
        {
            if (id != command.UserId)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del comando" });
            }

            _logger.LogInformation("Updating user: {UserId}", id);
            await _mediator.Send(command);
            return Ok(new { message = "Usuario actualizado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "User not found: {UserId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        _logger.LogInformation("Changing password for user: {UserId}", id);

        var command = new ChangePasswordCommand(
            id,
            request.CurrentPassword,
            request.NewPassword);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }    
}