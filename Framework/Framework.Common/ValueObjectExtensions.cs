using System.Reflection;

namespace Framework.Common;

public static class ValueObjectExtensions
{
    public static List<TValueObject> GetAllOptionsAsList<TValueObject>()
    {
        return typeof(TValueObject)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TValueObject))
            .Select(f => (TValueObject)f.GetValue(null))
            .ToList();
    }
}