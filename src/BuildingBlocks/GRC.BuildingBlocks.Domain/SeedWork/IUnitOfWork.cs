using System;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.BuildingBlocks.Domain.SeedWork;

/// <summary>
/// Interface para el patrón Unit of Work.
/// Coordina la escritura de cambios en la base de datos.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Guarda todos los cambios y publica los eventos de dominio
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}

