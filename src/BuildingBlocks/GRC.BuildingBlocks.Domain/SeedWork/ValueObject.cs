using System.Collections.Generic;
using System.Linq;

namespace GRC.BuildingBlocks.Domain.SeedWork;
/// 
/// Clase base para Value Objects.
/// Los Value Objects son inmutables y se comparan por valor, no por identidad.
/// Ejemplos: Email, Money, Address, DateRange
///
public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null)
            return false;

        return left?.Equals(right!) != false;
    }
    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }
    /// 
    /// Obtiene los componentes atómicos del Value Object para comparación
    /// 
    protected abstract IEnumerable<object> GetEqualityComponents();
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
    public ValueObject? GetCopy()
    {
        return MemberwiseClone() as ValueObject;
    }
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return EqualOperator(left, right);
    }
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return NotEqualOperator(left, right);
    }
}