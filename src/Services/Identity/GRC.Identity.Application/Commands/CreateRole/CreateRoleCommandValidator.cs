using FluentValidation;

namespace GRC.Identity.Application.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del rol es requerido")
            .MaximumLength(100).WithMessage("El nombre del rol no puede exceder 100 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9 ]+$")
            .WithMessage("El nombre del rol solo puede contener letras, números y espacios");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción del rol es requerida")
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
    }
}