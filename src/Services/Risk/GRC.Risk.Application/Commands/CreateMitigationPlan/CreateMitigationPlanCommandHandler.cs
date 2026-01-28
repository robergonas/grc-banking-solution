using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.CreateMitigationPlan;

public class CreateMitigationPlanCommandHandler : IRequestHandler<CreateMitigationPlanCommand, Guid>
{
    private readonly IMitigationPlanRepository _mitigationPlanRepository;
    private readonly IRiskRepository _riskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMitigationPlanCommandHandler(
        IMitigationPlanRepository mitigationPlanRepository,
        IRiskRepository riskRepository,
        IUnitOfWork unitOfWork)
    {
        _mitigationPlanRepository = mitigationPlanRepository;
        _riskRepository = riskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateMitigationPlanCommand request, CancellationToken cancellationToken)
    {
        // Verify risk exists
        var risk = await _riskRepository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk == null)
        {
            throw new RiskDomainException($"Risk with ID '{request.RiskId}' not found.");
        }

        // Create mitigation plan
        var mitigationPlan = MitigationPlan.Create(
            request.RiskId,
            request.Title,
            request.Description,
            request.StrategyId,
            request.Priority,
            request.OwnerId,
            request.CreatedBy
        );

        // Set optional properties
        if (request.EstimatedCost.HasValue || !string.IsNullOrEmpty(request.EstimatedBenefit))
        {
            mitigationPlan.SetCostAndBenefit(request.EstimatedCost, request.EstimatedBenefit ?? string.Empty);
        }

        if (request.StartDate.HasValue && request.TargetCompletionDate.HasValue)
        {
            mitigationPlan.ScheduleDates(request.StartDate.Value, request.TargetCompletionDate.Value);
        }

        // Update risk status
        risk.StartTreatment();

        _mitigationPlanRepository.Add(mitigationPlan);
        _riskRepository.Update(risk);

        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return mitigationPlan.Id;
    }
}