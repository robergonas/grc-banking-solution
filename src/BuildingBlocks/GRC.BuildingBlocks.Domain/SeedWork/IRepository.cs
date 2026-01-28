namespace GRC.BuildingBlocks.Domain.SeedWork;
/// 
/// Interface genérica para repositorios.
/// Los repositorios son responsables de la persistencia de los agregados.
/// Solo se crean repositorios para Aggregate Roots.
/// 
/// Tipo de Aggregate Root
public interface IRepository<T> where T : IAggregateRoot
{
    /// 
    /// Unit of Work para manejar transacciones
    /// 
    IUnitOfWork UnitOfWork { get; }
}