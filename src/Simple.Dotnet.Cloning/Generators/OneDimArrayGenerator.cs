namespace Simple.Dotnet.Cloning.Generators
{
    using Simple.Dotnet.Cloning.Cloners;
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class OneDimArrayGenerator
    {
        static readonly Type ObjectType = typeof(object);
        static readonly Type ClonerType = typeof(OneDimArrayCloner);
        static readonly MethodInfo DeepClone = ClonerType.GetMethod(nameof(OneDimArrayCloner.DeepClone), BindingFlags.Static | BindingFlags.Public);
        static readonly MethodInfo ShallowClone = ClonerType.GetMethod(nameof(OneDimArrayCloner.ShallowClone), BindingFlags.Static | BindingFlags.Public);

        public static ILGenerator CopyOneDimArray(this ILGenerator generator, Type type, bool deep = true)
        {
            var nullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check for equality
            generator.Emit(OpCodes.Brtrue_S, nullLabel); // If equal -> jump to null label

            // Not null:
            var underlying = type.GetElementType(); // Get underlying type
            var method = (deep, underlying) switch // Decide which method to use
            {
                (false, _) => ShallowClone.MakeGenericMethod(underlying),
                (true, var u) when u.IsSafeToCopyType() => ShallowClone.MakeGenericMethod(underlying),
                _ => DeepClone.MakeGenericMethod(underlying)
            };

            generator.Emit(OpCodes.Ldarg_0); // Load array
            generator.Emit(OpCodes.Call, method); // Call Cloner
            generator.Emit(OpCodes.Stloc_0); // Store returned array as a clone
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
    }
}
