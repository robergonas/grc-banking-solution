using FluentValidation;

namespace GRC.Risk.Application.Commands.CreateMitigationPlan;

public class CreateMitigationPlanCommandValidator : AbstractValidator<CreateMitigationPlanCommand>
{
    public CreateMitigationPlanCommandValidator()
    {
        RuleFor(x => x.RiskId)
            .NotEmpty().WithMessage("Risk ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.StrategyId)
            .GreaterThan(0).WithMessage("Valid strategy is required");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner is required");

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Estimated cost cannot be negative")
            .When(x => x.EstimatedCost.HasValue);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.TargetCompletionDate)
            .WithMessage("Start date must be before target completion date")
            .When(x => x.StartDate.HasValue && x.TargetCompletionDate.HasValue);

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("Created by is required");
    }
}