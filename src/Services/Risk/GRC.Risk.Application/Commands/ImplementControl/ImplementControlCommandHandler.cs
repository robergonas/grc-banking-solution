using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Aggregates.ControlAggregate;
using GRC.Risk.Domain.Exceptions;
using MediatR;

namespace GRC.Risk.Application.Commands.ImplementControl;

public class ImplementControlCommandHandler : IRequestHandler<ImplementControlCommand, Guid>
{
    private readonly IControlRepository _controlRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ImplementControlCommandHandler(
        IControlRepository controlRepository,
        IUnitOfWork unitOfWork)
    {
        _controlRepository = controlRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ImplementControlCommand request, CancellationToken cancellationToken)
    {
        // Check if control code already exists
        var existingControl = await _controlRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingControl != null)
        {
            throw new RiskDomainException($"Control with code '{request.Code}' already exists.");
        }

        // Create new control
        var control = Control.Create(
            request.Code,
            request.Name,
            request.Description ?? string.Empty,
            request.TypeId,
            request.NatureId,
            request.FrequencyId,
            request.OwnerId,
            request.CreatedBy
        );

        _controlRepository.Add(control);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return control.Id;
    }
}