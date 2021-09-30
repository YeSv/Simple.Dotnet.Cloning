using System;
using System.Reflection.Emit;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class ValueTypeInstanceCopyGenerator
    {
        public static ILGenerator CopyValueType(this ILGenerator generator, Type type, FieldInfo[] fields)
        {
            // Do nothing, RootGenerator will load instance onto stack before return
            if (type.IsPrimitive) return generator.CopyByValue();
            if (type.IsEnum) return generator.CopyByValue();
            
            // Copy fields only if type has non-safe types to copy
            return fields.All(f => f.FieldType.IsSafeToCopyValueType())
                ? generator.CopyByValue()
                : generator.CopyFields(type, fields);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator CopyByValue(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Stloc_0);

            return generator;
        }
    }
}
