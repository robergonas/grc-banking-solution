using FluentValidation;

namespace GRC.Identity.Application.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("El nombre completo es requerido")
            .MaximumLength(200)
            .WithMessage("El nombre completo no puede exceder 200 caracteres");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("El email no es válido")
                .MaximumLength(256)
                .WithMessage("El email no puede exceder 256 caracteres");
        });
    }
}