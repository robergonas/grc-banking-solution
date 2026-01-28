using FluentValidation;

namespace GRC.Identity.Application.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("La contraseña actual es requerida");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("La nueva contraseña es requerida")
            .MinimumLength(8)
            .WithMessage("La contraseña debe tener al menos 8 caracteres")
            .Matches(@"[A-Z]")
            .WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches(@"[a-z]")
            .WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches(@"[0-9]")
            .WithMessage("La contraseña debe contener al menos un número")
            .Matches(@"[\W_]")
            .WithMessage("La contraseña debe contener al menos un carácter especial");

        RuleFor(x => x)
            .Must(x => x.NewPassword != x.CurrentPassword)
            .WithMessage("La nueva contraseña debe ser diferente a la actual");
    }
}