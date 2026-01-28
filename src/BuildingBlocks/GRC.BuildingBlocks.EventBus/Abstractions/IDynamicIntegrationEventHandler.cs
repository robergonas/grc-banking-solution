using System.Threading.Tasks;

namespace GRC.BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Interface para handlers dinámicos que procesan eventos sin conocer su tipo en tiempo de compilación.
/// Útil para escenarios donde los eventos se manejan de forma genérica.
/// </summary>
public interface IDynamicIntegrationEventHandler
{
    /// <summary>
    /// Maneja un evento dinámico
    /// </summary>
    /// <param name="eventData">Datos del evento como objeto dinámico</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    Task Handle(dynamic eventData);
}