using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.AddMitigationAction;

public class AddMitigationActionCommandHandler : IRequestHandler<AddMitigationActionCommand, Guid>
{
    private readonly IMitigationPlanRepository _mitigationPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddMitigationActionCommandHandler(
        IMitigationPlanRepository mitigationPlanRepository,
        IUnitOfWork unitOfWork)
    {
        _mitigationPlanRepository = mitigationPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddMitigationActionCommand request, CancellationToken cancellationToken)
    {
        var mitigationPlan = await _mitigationPlanRepository.GetByIdWithActionsAsync(
            request.MitigationPlanId,
            cancellationToken);

        if (mitigationPlan == null)
        {
            throw new RiskDomainException($"Mitigation plan with ID '{request.MitigationPlanId}' not found.");
        }

        mitigationPlan.AddAction(
            request.Description,
            request.ResponsibleId,
            request.DueDate
        );

        _mitigationPlanRepository.Update(mitigationPlan);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        // Return the ID of the last added action
        return mitigationPlan.Actions.OrderByDescending(a => a.ActionNumber).First().Id;
    }
}