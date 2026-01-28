using System;
using System.Text.Json.Serialization;

namespace GRC.BuildingBlocks.EventBus.Events;

/// <summary>
/// Clase base para todos los eventos de integración.
/// Los Integration Events se usan para comunicación asíncrona entre microservicios.
/// </summary>
public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }

    /// <summary>
    /// Identificador único del evento
    /// </summary>
    [JsonInclude]
    public Guid Id { get; private set; }

    /// <summary>
    /// Fecha y hora de creación del evento (UTC)
    /// </summary>
    [JsonInclude]
    public DateTime CreationDate { get; private set; }
}