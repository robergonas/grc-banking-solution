using AutoMapper;
using GRC.Risk.Application.DTOs;
using GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;
using MediatR;

namespace GRC.Risk.Application.Queries.GetMitigationPlans;

public class GetMitigationPlansQueryHandler : IRequestHandler<GetMitigationPlansQuery, PagedResult<MitigationPlanDto>>
{
    private readonly IMitigationPlanRepository _mitigationPlanRepository;
    private readonly IMapper _mapper;

    public GetMitigationPlansQueryHandler(
        IMitigationPlanRepository mitigationPlanRepository,
        IMapper mapper)
    {
        _mitigationPlanRepository = mitigationPlanRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<MitigationPlanDto>> Handle(
        GetMitigationPlansQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<MitigationPlan> plans;

        if (request.RiskId.HasValue)
        {
            plans = await _mitigationPlanRepository.GetByRiskIdAsync(request.RiskId.Value, cancellationToken);
        }
        else if (request.StatusId.HasValue)
        {
            plans = await _mitigationPlanRepository.GetByStatusAsync(request.StatusId.Value, cancellationToken);
        }
        else if (request.OwnerId.HasValue)
        {
            plans = await _mitigationPlanRepository.GetByOwnerAsync(request.OwnerId.Value, cancellationToken);
        }
        else if (request.OnlyOverdue == true)
        {
            plans = await _mitigationPlanRepository.GetOverduePlansAsync(cancellationToken);
        }
        else
        {
            plans = new List<MitigationPlan>();
        }

        var totalCount = plans.Count();
        var pagedPlans = plans
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var planDtos = _mapper.Map<List<MitigationPlanDto>>(pagedPlans);

        return new PagedResult<MitigationPlanDto>(planDtos, totalCount, request.PageNumber, request.PageSize);
    }
}