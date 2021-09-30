using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class CopyInstanceFieldsGenerator
    {
        public static ILGenerator CopyFields(this ILGenerator generator, Type owner, FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                var nullableInnerType = Nullable.GetUnderlyingType(field.FieldType);
                generator = field.FieldType switch
                {
                    // Primitive:
                    { IsPrimitive: true } => generator.CopyFieldValue(owner, field),

                    // Enum
                    { IsEnum: true } => generator.CopyFieldValue(owner, field),

                    // Nullable: 
                    { IsValueType: true } when nullableInnerType != null && nullableInnerType.IsSafeToCopyValueType() => generator.CopyFieldValue(owner, field),
                    { IsValueType: true } when nullableInnerType != null => generator.CopyNullableField(field, owner, nullableInnerType),
                    
                    // Ref types and structs should be deep cloned
                    _ => generator.DeepCloneField(owner, field) // TODO: strings, arrays, HashSets, etc..
                };
            }

            return generator;
        }
    }
}
