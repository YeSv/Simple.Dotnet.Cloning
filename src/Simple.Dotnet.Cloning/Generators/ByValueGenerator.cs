using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ByValueGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator CopyByValue(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load argument
            generator.Emit(OpCodes.Stloc_0); // Store as local

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator CopyFieldValue(this ILGenerator generator, Type owner, FieldInfo field)
        {
            // Load local onto stack
            if (owner.IsValueType) generator.Emit(OpCodes.Ldloca_S, (byte)0);
            else generator.Emit(OpCodes.Ldloc_0);

            generator.Emit(OpCodes.Ldarg_0); // Load argument onto stack
            generator.Emit(OpCodes.Ldfld, field); // Load argument's field
            generator.Emit(OpCodes.Stfld, field); // Store local's field
            return generator;
        }
    }
}
