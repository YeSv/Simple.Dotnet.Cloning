using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class RootGenerator
    {
        static readonly Type StringType = typeof(string);
        static readonly Func<Type, FieldInfo[]> FieldsLazy = type => type.GetFields(FieldFlags);
        static readonly BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        static readonly MethodInfo MemberwiseClone = typeof(object).GetMethod(nameof(MemberwiseClone), BindingFlags.Instance | BindingFlags.NonPublic);

        public static ILGenerator Deep(this ILGenerator generator, Type type)
        {
            generator.DeclareLocal(type); // create local variable

            generator.Init(type); // Initialize a clone (into a local variable)

            _ =  type switch
            {
                { } when type.IsSafeToCopyType() => generator.CopyByValue(),
                { IsValueType: true } => generator.CopyValueType(type, FieldsLazy(type)),
                _ => generator.CopyFields(type, FieldsLazy(type))
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return clone's value

            return generator;
        }

        public static ILGenerator Shallow(this ILGenerator generator, Type type)
        {
            if (type.IsSafeToCopyType()) generator.Emit(OpCodes.Ldarg_0); // Just load string (they are immutable)
            else
            {
                generator.Emit(OpCodes.Ldarg_0); // Load local onto stack
                generator.Emit(OpCodes.Callvirt, MemberwiseClone); // Call memberwiseClone on object
                generator.Emit(OpCodes.Castclass, type); // Cast returned object to our type
            }

            generator.Emit(OpCodes.Ret); // Return loaded value
            return generator;
        }
    }
}
