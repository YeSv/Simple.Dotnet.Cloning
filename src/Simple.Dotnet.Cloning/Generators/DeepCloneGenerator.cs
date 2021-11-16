using Simple.Dotnet.Cloning.Cloners;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class DeepCloneGenerator
    {
        static readonly Type ClonerOpenType = typeof(RootCloner<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator DeepCloneField(this ILGenerator generator, Type owner, FieldInfo field)
        {
            var closedType = ClonerOpenType.MakeGenericType(field.FieldType);

            // Load clone to stack
            if (owner.IsValueType) generator.Emit(OpCodes.Ldloca_S, (byte)0);
            else generator.Emit(OpCodes.Ldloc_0);

            // Load instance to stack
            if (owner.IsValueType) generator.Emit(OpCodes.Ldarga_S, (byte)0);
            else generator.Emit(OpCodes.Ldarg_0);

            generator.Emit(OpCodes.Ldfld, field); // Load field to stack
            generator.Emit(OpCodes.Call, closedType.GetMethod(nameof(Cloner.DeepClone))!); // Call method with 
            generator.Emit(OpCodes.Stfld, field); // Store value in field of a clone

            return generator;
        }
    }
}
