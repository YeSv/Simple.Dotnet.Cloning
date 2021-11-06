using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Simple.Dotnet.Cloning.Cloners;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class RecurringTypesGenerator
    {
        static readonly Type Cloner = typeof(RecurringTypesCloner);
        static readonly MethodInfo LinkedListOpenedMethod = Cloner.GetMethod(nameof(RecurringTypesCloner.CloneLinkedList), BindingFlags.Static | BindingFlags.Public); 
        static readonly MethodInfo DictionaryOpenedMethod = Cloner.GetMethod(nameof(RecurringTypesCloner.CloneDictionary), BindingFlags.Static | BindingFlags.Public);
        static readonly MethodInfo SortedDictionaryOpenedMethod = Cloner.GetMethod(nameof(RecurringTypesCloner.CloneSortedDictionary), BindingFlags.Static | BindingFlags.Public);

        static readonly Dictionary<Type, Func<Type[], MethodInfo>> Cloners = new Dictionary<Type, Func<Type[], MethodInfo>>
        {
            [typeof(LinkedList<>)] = t => LinkedListOpenedMethod.MakeGenericMethod(t),
            [typeof(Dictionary<,>)] = t => DictionaryOpenedMethod.MakeGenericMethod(t),
            [typeof(SortedDictionary<,>)] = t => SortedDictionaryOpenedMethod.MakeGenericMethod(t)
        };

        public static ILGenerator CopyRecurringType(this ILGenerator generator, Type type)
        {
            // Pick method to clone
            var clonerMethod = type.IsGenericType
                ? Cloners[type.GetGenericTypeDefinition()](type.GetGenericArguments())
                : Cloners[type](Array.Empty<Type>());

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Call, clonerMethod); // Call specified method
            generator.Emit(OpCodes.Stloc_0); // Store clone as a local field

            return generator;
        }
    }
}
