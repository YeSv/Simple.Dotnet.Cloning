using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class CopyNullableFieldGenerator
    {
        static readonly Type ClonerOpenType = typeof(Cloner<>);
        static readonly Type NullableOpenType = typeof(Nullable<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator CopyNullableField(this ILGenerator generator, FieldInfo field, Type owner, Type innerType)
        {
            generator.LoadLocalOntoStack(owner.IsValueType); // Load instance onto stack
            generator.Emit(OpCodes.Ldflda, field); // Load nullable field
            generator.Emit(OpCodes.Call, field.FieldType.GetProperty(nameof(Nullable<int>.HasValue)).GetGetMethod()); // Check if it has value

            var ifLabel = generator.DefineLabel(); // Label for if instruction
            var returnLabel = generator.DefineLabel(); // Label for return instruction

            // If HasValue
            generator.Emit(OpCodes.Brfalse_S, ifLabel); // If false -> jump
            generator.LoadLocalOntoStack(owner.IsValueType); // Load clone onto stack
            generator.Emit(OpCodes.Ldarg_0); // Load instance onto stack
            generator.Emit(OpCodes.Ldflda, field); // Load nullable field onto stack
            generator.Emit(OpCodes.Call, field.FieldType.GetProperty(nameof(Nullable<int>.Value)).GetGetMethod()); // Use .Value and put onto stack
            generator.Emit(OpCodes.Call, ClonerOpenType.MakeGenericType(innerType).GetMethod(nameof(Cloner.DeepClone))); // Use pushed value and call DeepClone on it
            generator.Emit(OpCodes.Newobj, NullableOpenType.MakeGenericType(innerType).GetConstructor(new[] { innerType })); // Call new Nullable<T>(value) with a value pushed to stack
            generator.Emit(OpCodes.Stfld, field); // Set field to new nullable
            generator.Emit(OpCodes.Br_S, returnLabel);

            // If !HasValue
            generator.MarkLabel(ifLabel);
            generator.CopyFieldValue(owner, field); // Just copy by value, as nullable is empty

            // Skip !HasValue from if
            generator.MarkLabel(returnLabel);

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
