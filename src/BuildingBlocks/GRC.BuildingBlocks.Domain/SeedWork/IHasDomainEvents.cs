using MediatR;
using System.Collections.Generic;

namespace GRC.BuildingBlocks.Domain.SeedWork;

/// <summary>
/// Interfaz que marca las entidades que pueden generar eventos de dominio
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Colección de eventos de dominio pendientes
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Agrega un evento de dominio a la entidad
    /// </summary>
    void AddDomainEvent(IDomainEvent eventItem);

    /// <summary>
    /// Limpia todos los eventos de dominio de la entidad
    /// </summary>
    void ClearDomainEvents();
}