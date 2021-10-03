using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class NullableGenerator
    {
        static readonly Type ClonerOpenType = typeof(Cloner<>);
        static readonly Type NullableOpenType = typeof(Nullable<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator CopyNullable(this ILGenerator generator, Type type, Type innerType)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load argument onto stack
            generator.Emit(OpCodes.Call, type.GetProperty(nameof(Nullable<int>.HasValue)).GetGetMethod()); // Check if it has value

            var returnLabel = generator.DefineLabel(); // Label for return instruction

            // If HasValue
            generator.Emit(OpCodes.Brfalse_S, returnLabel); // If false -> jump
            generator.Emit(OpCodes.Ldarg_0); // Load argument onto stack
            generator.Emit(OpCodes.Call, type.GetProperty(nameof(Nullable<int>.Value)).GetGetMethod()); // Use .Value and put onto stack
            generator.Emit(OpCodes.Call, ClonerOpenType.MakeGenericType(type).GetMethod(nameof(Cloner.DeepClone))); // Use pushed value and call DeepClone on it
            generator.Emit(OpCodes.Newobj, type.GetConstructor(new[] { innerType })); // Call new Nullable<T>(value) with a value pushed to stack
            generator.Emit(OpCodes.Stloc_0); // Save 
            generator.Emit(OpCodes.Br_S, returnLabel);

            // Else just skip
            generator.MarkLabel(returnLabel);

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator LoadLocalOntoStack(this ILGenerator generator, bool isStruct)
        {
            if (isStruct) generator.Emit(OpCodes.Ldloca, (byte)0);
            else generator.Emit(OpCodes.Ldloc_0);

            return generator;
        }
    }
}
