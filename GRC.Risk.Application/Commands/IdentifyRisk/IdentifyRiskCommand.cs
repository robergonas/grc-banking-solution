using MediatR;

namespace GRC.Risk.Application.Commands.IdentifyRisk;

public record IdentifyRiskCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int CategoryId { get; init; }
    public int TypeId { get; init; }
    public string? Source { get; init; }
    public Guid OwnerId { get; init; }
    public Guid IdentifiedBy { get; init; }
}