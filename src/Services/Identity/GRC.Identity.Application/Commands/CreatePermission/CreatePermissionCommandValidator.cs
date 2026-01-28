using FluentValidation;

namespace GRC.Identity.Application.Commands.CreatePermission;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Resource).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Action).NotEmpty().MaximumLength(50)
            .Must(action => new[] { "Create", "Read", "Update", "Delete", "Approve", "Reject", "Publish", "Execute" }
                .Contains(action, StringComparer.OrdinalIgnoreCase))
            .WithMessage("La acción debe ser una de: Create, Read, Update, Delete, Approve, Reject, Publish, Execute");
    }
}