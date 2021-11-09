using System;
using System.Reflection;
using System.Reflection.Emit;
using Simple.Dotnet.Cloning.Cloners;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class CustomTypesGenerator
    {
        static readonly Type Cloner = typeof(RecurringTypesCloner);
        internal static readonly MethodInfo LinkedListOpenedMethod = Cloner.GetMethod(
            nameof(RecurringTypesCloner.CloneLinkedList), 
            BindingFlags.Static | BindingFlags.Public); 
        internal static readonly MethodInfo DictionaryOpenedMethod = Cloner.GetMethod(
            nameof(RecurringTypesCloner.CloneDictionary), 
            BindingFlags.Static | BindingFlags.Public);
        internal static readonly MethodInfo SortedDictionaryOpenedMethod = Cloner.GetMethod(
            nameof(RecurringTypesCloner.CloneSortedDictionary), 
            BindingFlags.Static | BindingFlags.Public);

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
