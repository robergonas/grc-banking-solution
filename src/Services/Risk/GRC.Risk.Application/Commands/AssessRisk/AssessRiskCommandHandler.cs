using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.AssessRisk;

public class AssessRiskCommandHandler : IRequestHandler<AssessRiskCommand, Guid>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssessRiskCommandHandler(
        IRiskRepository riskRepository,
        IUnitOfWork unitOfWork)
    {
        _riskRepository = riskRepository ?? throw new ArgumentNullException(nameof(riskRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Guid> Handle(AssessRiskCommand request, CancellationToken cancellationToken)
    {
        // Get risk
        var risk = await _riskRepository.GetByIdWithAssessmentsAsync(request.RiskId, cancellationToken);
        if (risk == null)
        {
            throw new RiskDomainException($"Risk with ID '{request.RiskId}' not found.");
        }

        // Assess risk
        risk.Assess(
            request.Probability,
            request.Impact,
            request.ProbabilityJustification ?? string.Empty,
            request.ImpactJustification ?? string.Empty,
            request.AssessedBy
        );

        // Get the latest assessment
        var latestAssessment = risk.LatestAssessment;
        if (latestAssessment == null)
        {
            throw new RiskDomainException("Failed to create risk assessment.");
        }

        // Set additional assessment properties
        if (request.ControlEffectiveness.HasValue && !string.IsNullOrEmpty(request.ExistingControls))
        {
            latestAssessment.SetControlEffectiveness(
                request.ControlEffectiveness.Value,
                request.ExistingControls
            );
        }

        if (request.ResidualRiskLevel.HasValue)
        {
            latestAssessment.SetResidualRisk(
                request.ResidualRiskLevel.Value,
                request.ResidualRiskJustification ?? string.Empty
            );
        }

        if (request.RiskVelocity.HasValue)
        {
            latestAssessment.SetRiskVelocity(request.RiskVelocity.Value);
        }

        if (request.RiskAppetite.HasValue)
        {
            latestAssessment.SetRiskAppetite(request.RiskAppetite.Value);
        }

        if (request.NextReviewDate.HasValue)
        {
            latestAssessment.ScheduleNextReview(request.NextReviewDate.Value);
        }

        // Update repository
        _riskRepository.Update(risk);

        // Save changes
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return latestAssessment.Id;
    }
}