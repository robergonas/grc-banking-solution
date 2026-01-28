using MediatR;

namespace GRC.BuildingBlocks.Domain.SeedWork;

/// 
/// Interface para eventos de dominio.
/// Los Domain Events representan algo que sucedió en el dominio que es de interés para otros bounded contexts.
/// Heredan de INotification de MediatR para poder ser publicados y manejados.
/// 
public interface IDomainEvent : INotification
{
    /// 
    /// Fecha y hora en que ocurrió el evento
    /// 
    DateTime OccurredOn { get; }    
}