using GRC.BuildingBlocks.EventBus.Events;

namespace GRC.BuildingBlocks.EventBus.Abstractions;

public interface IEventBus
{
    /// <summary>
    /// Publica un evento de integración en el bus
    /// </summary>
    /// <param name="event">Evento a publicar</param>
    void Publish(IntegrationEvent @event);

    /// <summary>
    /// Suscribe un handler a un evento de integración
    /// </summary>
    /// <typeparam name="T">Tipo del evento</typeparam>
    /// <typeparam name="TH">Tipo del handler</typeparam>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Suscribe un handler dinámico a un evento por nombre
    /// </summary>
    /// <typeparam name="TH">Tipo del handler dinámico</typeparam>
    /// <param name="eventName">Nombre del evento</param>
    void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    /// <summary>
    /// Desuscribe un handler de un evento
    /// </summary>
    /// <typeparam name="T">Tipo del evento</typeparam>
    /// <typeparam name="TH">Tipo del handler</typeparam>
    void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Desuscribe un handler dinámico
    /// </summary>
    /// <typeparam name="TH">Tipo del handler dinámico</typeparam>
    /// <param name="eventName">Nombre del evento</param>
    void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;
}