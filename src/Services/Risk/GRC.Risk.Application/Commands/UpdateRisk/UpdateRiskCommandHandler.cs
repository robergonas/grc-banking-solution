using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.UpdateRisk;

public class UpdateRiskCommandHandler : IRequestHandler<UpdateRiskCommand, bool>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRiskCommandHandler(
        IRiskRepository riskRepository,
        IUnitOfWork unitOfWork)
    {
        _riskRepository = riskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await _riskRepository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk == null)
        {
            throw new RiskDomainException($"Risk with ID '{request.RiskId}' not found.");
        }

        risk.UpdateDetails(
            request.Title,
            request.Description ?? string.Empty,
            request.CategoryId,
            request.TypeId,
            request.Source ?? string.Empty,
            request.OwnerId,
            request.UpdatedBy
        );

        _riskRepository.Update(risk);
        return await _unitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}