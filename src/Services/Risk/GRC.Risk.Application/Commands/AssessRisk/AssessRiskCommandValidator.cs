using FluentValidation;

namespace GRC.Risk.Application.Commands.AssessRisk;

public class AssessRiskCommandValidator : AbstractValidator<AssessRiskCommand>
{
    public AssessRiskCommandValidator()
    {
        RuleFor(x => x.RiskId)
            .NotEmpty().WithMessage("Risk ID is required");

        RuleFor(x => x.Probability)
            .InclusiveBetween(1, 5).WithMessage("Probability must be between 1 and 5");

        RuleFor(x => x.Impact)
            .InclusiveBetween(1, 5).WithMessage("Impact must be between 1 and 5");

        RuleFor(x => x.ControlEffectiveness)
            .InclusiveBetween(1, 5).WithMessage("Control effectiveness must be between 1 and 5")
            .When(x => x.ControlEffectiveness.HasValue);

        RuleFor(x => x.ResidualRiskLevel)
            .InclusiveBetween(1, 25).WithMessage("Residual risk level must be between 1 and 25")
            .When(x => x.ResidualRiskLevel.HasValue);

        RuleFor(x => x.RiskVelocity)
            .InclusiveBetween(1, 5).WithMessage("Risk velocity must be between 1 and 5")
            .When(x => x.RiskVelocity.HasValue);

        RuleFor(x => x.RiskAppetite)
            .InclusiveBetween(1, 25).WithMessage("Risk appetite must be between 1 and 25")
            .When(x => x.RiskAppetite.HasValue);

        RuleFor(x => x.NextReviewDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next review date must be in the future")
            .When(x => x.NextReviewDate.HasValue);

        RuleFor(x => x.AssessedBy)
            .NotEmpty().WithMessage("Assessed by is required");
    }
}