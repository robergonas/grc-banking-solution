using FluentValidation;

namespace GRC.Risk.Application.Commands.AddMitigationAction;

public class AddMitigationActionCommandValidator : AbstractValidator<AddMitigationActionCommand>
{
    public AddMitigationActionCommandValidator()
    {
        RuleFor(x => x.MitigationPlanId)
            .NotEmpty().WithMessage("Mitigation plan ID is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Action description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.ResponsibleId)
            .NotEmpty().WithMessage("Responsible person is required");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future");
    }
}