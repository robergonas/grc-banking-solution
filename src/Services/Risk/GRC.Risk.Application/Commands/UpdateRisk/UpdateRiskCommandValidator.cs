using FluentValidation;

namespace GRC.Risk.Application.Commands.UpdateRisk;

public class UpdateRiskCommandValidator : AbstractValidator<UpdateRiskCommand>
{
    public UpdateRiskCommandValidator()
    {
        RuleFor(x => x.RiskId)
            .NotEmpty().WithMessage("Risk ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Risk title is required")
            .MaximumLength(200).WithMessage("Risk title must not exceed 200 characters");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid category is required");

        RuleFor(x => x.TypeId)
            .GreaterThan(0).WithMessage("Valid type is required");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Risk owner is required");

        RuleFor(x => x.UpdatedBy)
            .NotEmpty().WithMessage("Updated by is required");
    }
}