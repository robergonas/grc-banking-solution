using GRC.Identity.Application.Commands.AssignPermissionsToRole;
using GRC.Identity.Application.Commands.CreateRole;
using GRC.Identity.Application.Commands.DeleteRole;
using GRC.Identity.Application.Commands.RemovePermissionFromRole;
using GRC.Identity.Application.Commands.UpdateRole;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Application.Queries.GetRoleById;
using GRC.Identity.Application.Queries.GetRoles;
using GRC.Identity.Application.Queries.GetUsersByRole;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRC.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IMediator mediator, ILogger<RolesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    /// <summary>
    /// Obtiene todos los roles con paginación
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<RoleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<RoleDto>>> GetRoles(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        [FromQuery] bool includeSystemRoles = true)
    {
        try
        {
            _logger.LogInformation("Getting roles - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var query = new GetRolesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                IsActive = isActive,
                IncludeSystemRoles = includeSystemRoles
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles");
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Obtiene un rol por su ID con sus permisos
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoleWithPermissionsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleWithPermissionsDto>> GetRoleById(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting role by ID: {RoleId}", id);

            var query = new GetRoleByIdQuery { RoleId = id };
            var role = await _mediator.Send(query);

            if (role == null)
            {
                return NotFound(new { message = $"Rol con ID {id} no encontrado" });
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRoleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateRoleResponse>> CreateRole([FromBody] CreateRoleCommand command)
    {
        try
        {
            _logger.LogInformation("Creating role: {RoleName}", command.Name);

            var roleId = await _mediator.Send(command);

            var response = new CreateRoleResponse { RoleId = roleId };
            return CreatedAtAction(nameof(GetRoleById), new { id = roleId }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role: {RoleName}", command.Name);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleCommand command)
    {
        try
        {
            if (id != command.RoleId)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del comando" });
            }

            _logger.LogInformation("Updating role: {RoleId}", id);

            await _mediator.Send(command);
            return Ok(new { message = "Rol actualizado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role not found: {RoleId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Elimina un rol (solo roles no-system)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRole(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting role: {RoleId}", id);

            var command = new DeleteRoleCommand { RoleId = id };
            await _mediator.Send(command);

            return Ok(new { message = "Rol eliminado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot delete role: {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Asigna permisos a un rol
    /// </summary>
    [HttpPost("{id:guid}/permissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AssignPermissions(Guid id, [FromBody] AssignPermissionsRequest request)
    {
        try
        {
            _logger.LogInformation("Assigning {Count} permissions to role: {RoleId}",
                request.PermissionIds.Count, id);

            var command = new AssignPermissionsToRoleCommand
            {
                RoleId = id,
                PermissionIds = request.PermissionIds
            };

            await _mediator.Send(command);
            return Ok(new { message = "Permisos asignados exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role not found: {RoleId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permissions to role {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Remueve un permiso de un rol
    /// </summary>
    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemovePermission(Guid id, Guid permissionId)
    {
        try
        {
            _logger.LogInformation("Removing permission {PermissionId} from role {RoleId}",
                permissionId, id);

            var command = new RemovePermissionFromRoleCommand
            {
                RoleId = id,
                PermissionId = permissionId
            };

            await _mediator.Send(command);
            return Ok(new { message = "Permiso removido exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot remove permission: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission {PermissionId} from role {RoleId}",
                permissionId, id);
            return BadRequest(new { message = ex.Message });
        }
    }
    /// <summary>
    /// Obtiene usuarios que tienen este rol
    /// </summary>
    [HttpGet("{id:guid}/users")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting users with role: {RoleId}", id);

            var query = new GetUsersByRoleQuery(id);// { RoleId = id };
            var users = await _mediator.Send(query);

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users for role {RoleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}