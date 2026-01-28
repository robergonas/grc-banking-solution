using GRC.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace GRC.BuildingBlocks.EventBus.Abstractions;

/// 
/// Interface base (marker) para todos los handlers
/// 
public interface IIntegrationEventHandler
{
}

/// 
/// Interface para handlers de eventos de integración tipados
/// 
/// Tipo del evento a manejar
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Maneja el evento de integración
    /// </summary>
    /// <param name="event">Evento a procesar</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    Task Handle(TIntegrationEvent @event);
}