using Simple.Dotnet.Cloning.Cloners;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ArrayGenerator
    {
        static readonly Type ArrayType = typeof(Array);
        static readonly Type SingleDimClonerType = typeof(ArrayCloner.SingleDim);
        static readonly Type TwoDimClonerType = typeof(ArrayCloner.TwoDim);
        static readonly Type ThreeDimClonerType = typeof(ArrayCloner.ThreeDim);
        static readonly Type FourDimClonerType = typeof(ArrayCloner.FourDim);
        static readonly MethodInfo MemberwiseCloneMethod = ArrayType.GetMethod(nameof(MemberwiseClone), BindingFlags.Instance | BindingFlags.NonPublic);

        public static ILGenerator CopyArray(this ILGenerator generator, Type type, bool deep = true)
        {
            var nullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check for equality
            generator.Emit(OpCodes.Brtrue_S, nullLabel); // If equal -> jump to null label

            // Not null:
            var elementType = type.GetElementType(); // Get underlying type
            var useShallow = !deep || elementType.IsSafeToCopyType(); // Check whether it's possible to use shallow clone 
            _ = type.GetArrayRank() switch // Decide which method to use
            {
                1 => useShallow ? ShallowClone(generator, elementType, SingleDimClonerType) : DeepClone(generator, elementType, SingleDimClonerType),
                2 => useShallow ? ShallowClone(generator, elementType, TwoDimClonerType) : DeepClone(generator, elementType, TwoDimClonerType),
                3 => useShallow ? ShallowClone(generator, elementType, ThreeDimClonerType) : DeepClone(generator, elementType, ThreeDimClonerType),
                4 => useShallow ? ShallowClone(generator, elementType, FourDimClonerType) : DeepClone(generator, elementType, FourDimClonerType),
                _ => MemberwiseClone(generator, type) // For all others - shallow clone even if deep is specified. (by design)
            };
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit

            // Null:
            generator.MarkLabel(nullLabel);
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Stloc_0); // Store as a clone
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit

            // Exit
            generator.MarkLabel(exitLabel);
            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator MemberwiseClone(this ILGenerator generator, Type arrayType)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load array
            generator.Emit(OpCodes.Castclass, ArrayType); // (Array)
            generator.Emit(OpCodes.Callvirt, MemberwiseCloneMethod); // Call memberwise clone
            generator.Emit(OpCodes.Castclass, arrayType); // (Type)
            generator.Emit(OpCodes.Stloc_0); // Store as a clone

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator ShallowClone(this ILGenerator generator, Type elementType, Type clonerType)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load array
            generator.Emit(OpCodes.Call, clonerType.GetMethod(nameof(ShallowClone), BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(elementType)); // Call shallow clone
            generator.Emit(OpCodes.Stloc_0); // Store as a clone

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator DeepClone(this ILGenerator generator, Type elementType, Type clonerType)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load array
            generator.Emit(OpCodes.Call, clonerType.GetMethod(nameof(DeepClone), BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(elementType)); // Call deep clone
            generator.Emit(OpCodes.Stloc_0); // Store as a clone

            return generator;
        }
    }
}
