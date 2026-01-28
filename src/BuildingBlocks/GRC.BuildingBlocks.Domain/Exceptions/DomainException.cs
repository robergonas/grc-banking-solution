namespace GRC.BuildingBlocks.Domain.Exceptions;
/// 
/// Excepción base para todas las excepciones del dominio.
/// Se usa para capturar errores de reglas de negocio violadas.
/// 
public class DomainException : Exception
{
    public DomainException()
    { }

    public DomainException(string message)
        : base(message)
    { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
