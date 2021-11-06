using System;
using System.Reflection.Emit;
using System.Linq;
using System.Reflection;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ValueTypeGenerator
    {
        // Check whether all fields are safe to copy, if so -> just copy whole instance, copy field by field otherwise
        public static ILGenerator CopyValueType(this ILGenerator generator, Type type, Func<Type, FieldInfo[]> fieldsLazy)
        {
            // For nullable - use special handling
            var nullableInnerType = Nullable.GetUnderlyingType(type);
            if (nullableInnerType != null) return generator.CopyNullable(type, nullableInnerType);

            var fields = fieldsLazy(type);

            // If all fields are safe to copy - we can just copy struct by value or use field by field copy otherwise
            return fields.All(f => f.FieldType.IsSafeToCopyType()) ? generator.CopyByValue() : generator.DeepCopyFields(type, fields);
        }
    }
}
