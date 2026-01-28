using MediatR;

namespace GRC.Risk.Application.Commands.UpdateRisk;

public record UpdateRiskCommand : IRequest<bool>
{
    public Guid RiskId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int CategoryId { get; init; }
    public int TypeId { get; init; }
    public string? Source { get; init; }
    public Guid OwnerId { get; init; }
    public Guid UpdatedBy { get; init; }
}