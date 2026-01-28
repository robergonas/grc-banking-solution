using FluentValidation;

namespace GRC.Risk.Application.Commands.IdentifyRisk;

public class IdentifyRiskCommandValidator : AbstractValidator<IdentifyRiskCommand>
{
    public IdentifyRiskCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Risk code is required")
            .MaximumLength(50).WithMessage("Risk code must not exceed 50 characters");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Risk title is required")
            .MaximumLength(200).WithMessage("Risk title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid category is required")
            .LessThanOrEqualTo(9).WithMessage("Invalid category");

        RuleFor(x => x.TypeId)
            .GreaterThan(0).WithMessage("Valid type is required")
            .LessThanOrEqualTo(3).WithMessage("Invalid type");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Risk owner is required");

        RuleFor(x => x.IdentifiedBy)
            .NotEmpty().WithMessage("Identified by is required");
    }
}