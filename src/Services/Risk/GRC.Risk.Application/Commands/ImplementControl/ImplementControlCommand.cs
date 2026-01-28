using MediatR;

namespace GRC.Risk.Application.Commands.ImplementControl;

public record ImplementControlCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int TypeId { get; init; }
    public int NatureId { get; init; }
    public int FrequencyId { get; init; }
    public Guid OwnerId { get; init; }
    public Guid CreatedBy { get; init; }
}