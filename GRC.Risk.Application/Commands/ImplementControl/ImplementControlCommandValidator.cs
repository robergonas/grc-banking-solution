using FluentValidation;

namespace GRC.Risk.Application.Commands.ImplementControl;

public class ImplementControlCommandValidator : AbstractValidator<ImplementControlCommand>
{
    public ImplementControlCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Control code is required")
            .MaximumLength(50).WithMessage("Control code must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Control name is required")
            .MaximumLength(200).WithMessage("Control name must not exceed 200 characters");

        RuleFor(x => x.TypeId)
            .GreaterThan(0).WithMessage("Valid control type is required");

        RuleFor(x => x.NatureId)
            .GreaterThan(0).WithMessage("Valid control nature is required");

        RuleFor(x => x.FrequencyId)
            .GreaterThan(0).WithMessage("Valid control frequency is required");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Control owner is required");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("Created by is required");
    }
}