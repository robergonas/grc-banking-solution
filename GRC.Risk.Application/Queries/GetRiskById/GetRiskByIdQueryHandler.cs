using AutoMapper;
using GRC.Risk.Application.DTOs;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
using MediatR;

namespace GRC.Risk.Application.Queries.GetRiskById;

public class GetRiskByIdQueryHandler : IRequestHandler<GetRiskByIdQuery, RiskDetailDto?>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IMapper _mapper;

    public GetRiskByIdQueryHandler(
        IRiskRepository riskRepository,
        IMapper mapper)
    {
        _riskRepository = riskRepository;
        _mapper = mapper;
    }

    public async Task<RiskDetailDto?> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var risk = await _riskRepository.GetByIdWithAssessmentsAsync(request.RiskId, cancellationToken);

        if (risk == null)
            return null;

        return _mapper.Map<RiskDetailDto>(risk);
    }
}