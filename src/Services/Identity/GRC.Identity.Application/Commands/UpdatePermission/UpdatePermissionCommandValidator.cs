using FluentValidation;

namespace GRC.Identity.Application.Commands.UpdatePermission;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.PermissionId).NotEmpty();
        RuleFor(x => x.Resource).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Action).NotEmpty().MaximumLength(50);
    }
}