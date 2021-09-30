using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class DeepCloneFieldGenerator
    {
        static readonly Type ClonerOpenType = typeof(Cloner<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator DeepCloneField(this ILGenerator generator, Type owner, FieldInfo field)
        {
            var closedType = ClonerOpenType.MakeGenericType(field.FieldType);

            generator.LoadLocalOntoStack(owner.IsValueType); // Load clone to stack
            generator.Emit(OpCodes.Ldarg_0); // Load instance to stack
            generator.Emit(OpCodes.Ldfld, field); // Load field to stack
            generator.Emit(OpCodes.Call, closedType.GetMethod(nameof(Cloner.DeepClone))); // Call method with 
            generator.Emit(OpCodes.Stfld, field); // Store value in field of a clone

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator LoadLocalOntoStack(this ILGenerator generator, bool isStruct)
        {
            if (isStruct) generator.Emit(OpCodes.Ldloca_S, (byte)0);
            else generator.Emit(OpCodes.Ldloc_0);

            return generator;
        }
    }
}
