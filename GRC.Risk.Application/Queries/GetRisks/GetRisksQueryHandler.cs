using AutoMapper;
using GRC.Risk.Application.DTOs;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using MediatR;

namespace GRC.Risk.Application.Queries.GetRisks;

public class GetRisksQueryHandler : IRequestHandler<GetRisksQuery, PagedResult<RiskDto>>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IMapper _mapper;

    public GetRisksQueryHandler(
        IRiskRepository riskRepository,
        IMapper mapper)
    {
        _riskRepository = riskRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<RiskDto>> Handle(GetRisksQuery request, CancellationToken cancellationToken)
    {
        // This is a simplified implementation
        // In a real scenario, you would implement filtering and pagination in the repository

        IEnumerable<Domain.Aggregates.RiskAggregate.Risk> risks;

        if (request.CategoryId.HasValue)
        {
            risks = await _riskRepository.GetByCategoryAsync(request.CategoryId.Value, cancellationToken);
        }
        else if (request.StatusId.HasValue)
        {
            risks = await _riskRepository.GetByStatusAsync(request.StatusId.Value, cancellationToken);
        }
        else if (request.OwnerId.HasValue)
        {
            risks = await _riskRepository.GetByOwnerAsync(request.OwnerId.Value, cancellationToken);
        }
        else
        {
            // Get all risks - this should be paginated in repository
            risks = new List<Domain.Aggregates.RiskAggregate.Risk>();
        }

        var filteredRisks = risks;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filteredRisks = risks.Where(r =>
                r.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.Title.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }

        var totalCount = filteredRisks.Count();
        var pagedRisks = filteredRisks
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var riskDtos = _mapper.Map<List<RiskDto>>(pagedRisks);

        return new PagedResult<RiskDto>(riskDtos, totalCount, request.PageNumber, request.PageSize);
    }
}