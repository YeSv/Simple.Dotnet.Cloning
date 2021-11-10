using System;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class CustomTypesGenerator
    {
        public static ILGenerator CopyCustomCloningType(this ILGenerator generator, Type type)
        {
            // Pick method to clone
            var clonerMethod = type.IsGenericType
                ? CloningTypes.Custom[type.GetGenericTypeDefinition()](type.GetGenericArguments())
                : CloningTypes.Custom[type](Array.Empty<Type>());

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Call, clonerMethod); // Call specified method
            generator.Emit(OpCodes.Stloc_0); // Store clone as a local field

            return generator;
        }
    }
}
