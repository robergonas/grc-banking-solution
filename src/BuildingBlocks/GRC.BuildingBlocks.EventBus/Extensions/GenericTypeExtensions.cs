using System;
using System.Linq;

namespace GRC.BuildingBlocks.EventBus.Extensions;

public static class GenericTypeExtensions
{
    /// <summary>
    /// Obtiene el nombre de un tipo genérico de forma legible
    /// </summary>
    /// <param name="type">Tipo a procesar</param>
    /// <returns>Nombre del tipo</returns>
    public static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    /// <summary>
    /// Obtiene el nombre de un objeto genérico
    /// </summary>
    /// <param name="object">Objeto a procesar</param>
    /// <returns>Nombre del tipo del objeto</returns>
    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}