using System;
using System.Reflection.Emit;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ValueTypeGenerator
    {
        public static ILGenerator CopyValueType(this ILGenerator generator, Type type, Func<Type, IEnumerable<FieldInfo>> fieldsLazy)
        {
            // For nullable - use special handling
            var nullableInnerType = Nullable.GetUnderlyingType(type);
            if (nullableInnerType != null) return generator.CopyNullable(type, nullableInnerType);

            var fields = fieldsLazy(type);

            generator.CopyByValue(); // Copy struct by value -> clone = instance

            if (!fields.All(f => f.FieldType.IsSafeToCopy())) generator.DeepCopyValueTypeFields(type, fields); // Deep clone fields that are not safe to copy

            return generator;
        }
    }
}
