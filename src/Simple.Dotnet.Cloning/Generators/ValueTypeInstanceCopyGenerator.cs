using System;
using System.Reflection.Emit;
using System.Linq;
using System.Reflection;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class ValueTypeInstanceCopyGenerator
    {
        // Check whether all fields are safe to copy, if so -> just copy whole instance, copy field by field otherwise
        public static ILGenerator CopyValueType(this ILGenerator generator, Type type, FieldInfo[] fields) =>
            fields.All(f => f.FieldType.IsSafeToCopyValueType())
                ? generator.CopyByValue()
                : generator.CopyFields(type, fields);
    }
}
