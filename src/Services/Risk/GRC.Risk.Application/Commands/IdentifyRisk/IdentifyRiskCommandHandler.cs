using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.IdentifyRisk;

public class IdentifyRiskCommandHandler : IRequestHandler<IdentifyRiskCommand, Guid>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IdentifyRiskCommandHandler(
        IRiskRepository riskRepository,
        IUnitOfWork unitOfWork)
    {
        _riskRepository = riskRepository ?? throw new ArgumentNullException(nameof(riskRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Guid> Handle(IdentifyRiskCommand request, CancellationToken cancellationToken)
    {
        // Check if risk code already exists
        var existingRisk = await _riskRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingRisk != null)
        {
            throw new RiskDomainException($"Risk with code '{request.Code}' already exists.");
        }

        // Create new risk
        var risk = Domain.Aggregates.RiskAggregate.Risk.Create(
            request.Code,
            request.Title,
            request.Description ?? string.Empty,
            request.CategoryId,
            request.TypeId,
            request.Source ?? string.Empty,
            request.IdentifiedBy,
            request.OwnerId,
            request.IdentifiedBy
        );

        // Add to repository
        _riskRepository.Add(risk);

        // Save changes
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return risk.Id;
    }
}