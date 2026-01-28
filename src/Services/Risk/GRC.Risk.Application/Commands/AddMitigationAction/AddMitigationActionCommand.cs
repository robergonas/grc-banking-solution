using MediatR;

namespace GRC.Risk.Application.Commands.AddMitigationAction;

public record AddMitigationActionCommand : IRequest<Guid>
{
    public Guid MitigationPlanId { get; init; }
    public string Description { get; init; } = string.Empty;
    public Guid ResponsibleId { get; init; }
    public DateTime DueDate { get; init; }
}