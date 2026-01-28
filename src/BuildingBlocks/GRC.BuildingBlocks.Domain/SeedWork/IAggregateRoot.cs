namespace GRC.BuildingBlocks.Domain.SeedWork;
/// 
/// Marca una entidad como raíz de un agregado.
/// Un Aggregate Root es el punto de entrada al agregado y garantiza la consistencia.
/// Solo los Aggregate Roots pueden ser accedidos directamente desde fuera del agregado.
/// 
public interface IAggregateRoot
{
    // Marker interface - no requiere miembros
    // Se usa para identificar las entidades raíz de agregados
}