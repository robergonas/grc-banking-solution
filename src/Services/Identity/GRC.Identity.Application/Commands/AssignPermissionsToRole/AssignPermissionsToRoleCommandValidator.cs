using FluentValidation;

namespace GRC.Identity.Application.Commands.AssignPermissionsToRole;

public class AssignPermissionsToRoleCommandValidator : AbstractValidator<AssignPermissionsToRoleCommand>
{
    public AssignPermissionsToRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("El ID del rol es requerido");
        RuleFor(x => x.PermissionIds).NotEmpty().WithMessage("Debe especificar al menos un permiso");
    }
}