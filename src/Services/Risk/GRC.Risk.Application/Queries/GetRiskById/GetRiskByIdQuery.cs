using GRC.Risk.Application.DTOs;
using MediatR;

namespace GRC.Risk.Application.Queries.GetRiskById;

public record GetRiskByIdQuery(Guid RiskId) : IRequest<RiskDetailDto?>;