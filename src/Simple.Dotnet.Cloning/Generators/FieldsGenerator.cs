using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class FieldsGenerator
    {
        public static ILGenerator CopyFields(this ILGenerator generator, Type owner, FieldInfo[] fields)
        {
            foreach (var field in fields) _ = field.FieldType switch
            {
                var ft when ft.IsSafeToCopyType() => generator.CopyFieldValue(owner, field),
                _ => generator.DeepCloneField(owner, field)
            };

            return generator;
        }
    }
}
