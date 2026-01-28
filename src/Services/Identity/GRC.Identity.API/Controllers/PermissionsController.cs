using GRC.Identity.Application.Commands.CreatePermission;
using GRC.Identity.Application.Commands.DeletePermission;
using GRC.Identity.Application.Commands.UpdatePermission;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Application.Queries.GetPermissionById;
using GRC.Identity.Application.Queries.GetPermissions;
using GRC.Identity.Application.Queries.GetPermissionsByResource;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRC.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene todos los permisos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
    {
        try
        {
            _logger.LogInformation("Getting all permissions");

            var query = new GetPermissionsQuery();
            var permissions = await _mediator.Send(query);

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un permiso por su ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermissionDto>> GetPermissionById(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting permission by ID: {PermissionId}", id);

            var query = new GetPermissionByIdQuery { PermissionId = id };
            var permission = await _mediator.Send(query);

            if (permission == null)
            {
                return NotFound(new { message = $"Permiso con ID {id} no encontrado" });
            }

            return Ok(permission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permission {PermissionId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene permisos por recurso
    /// </summary>
    [HttpGet("by-resource/{resource}")]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissionsByResource(string resource)
    {
        try
        {
            _logger.LogInformation("Getting permissions for resource: {Resource}", resource);

            var query = new GetPermissionsByResourceQuery { Resource = resource };
            var permissions = await _mediator.Send(query);

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for resource {Resource}", resource);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo permiso
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePermissionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreatePermissionResponse>> CreatePermission([FromBody] CreatePermissionCommand command)
    {
        try
        {
            _logger.LogInformation("Creating permission: {Resource}.{Action}", command.Resource, command.Action);

            var permissionId = await _mediator.Send(command);

            var response = new CreatePermissionResponse { PermissionId = permissionId };
            return CreatedAtAction(nameof(GetPermissionById), new { id = permissionId }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Permission creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating permission: {Resource}.{Action}", command.Resource, command.Action);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un permiso existente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionCommand command)
    {
        try
        {
            if (id != command.PermissionId)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del comando" });
            }

            _logger.LogInformation("Updating permission: {PermissionId}", id);

            await _mediator.Send(command);
            return Ok(new { message = "Permiso actualizado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Permission not found: {PermissionId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating permission {PermissionId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un permiso
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePermission(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting permission: {PermissionId}", id);

            var command = new DeletePermissionCommand { PermissionId = id };
            await _mediator.Send(command);

            return Ok(new { message = "Permiso eliminado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Permission not found: {PermissionId}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting permission {PermissionId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene lista de recursos disponibles
    /// </summary>
    [HttpGet("resources")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> GetResources()
    {
        var resources = new[]
        {
            "Users",
            "Roles",
            "Permissions",
            "Policies",
            "Committees",
            "Meetings",
            "Risks",
            "Controls",
            "Incidents",
            "Regulations",
            "Reports"
        };

        return Ok(resources);
    }

    /// <summary>
    /// Obtiene lista de acciones disponibles
    /// </summary>
    [HttpGet("actions")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> GetActions()
    {
        var actions = new[]
        {
            "Create",
            "Read",
            "Update",
            "Delete",
            "Approve",
            "Reject",
            "Publish",
            "Execute"
        };

        return Ok(actions);
    }
}