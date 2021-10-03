using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class RootGenerator
    {
        static readonly Func<Type, FieldInfo[]> FieldsLazy = type => type.GetFields(FieldFlags);
        static readonly BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        static readonly MethodInfo MemberwiseClone = typeof(object).GetMethod(nameof(MemberwiseClone), BindingFlags.Instance | BindingFlags.NonPublic);

        public static ILGenerator Deep(this ILGenerator generator, Type type)
        {
            generator.DeclareLocal(type); // create local variable

            generator.Init(type); // Initialize a clone (into a local variable)

            _ = type switch
            {
                { } when type.IsSafeToCopyType() => generator.CopyByValue(), // When type is safe to copy - just load onto stack and return (for example: string is immutable)
                { IsValueType: true } => generator.CopyValueType(type, FieldsLazy), // Copy value type
                _ => generator.CopyReferenceType(type, FieldsLazy(type)) // Copy reference type
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return clone's value

            return generator;
        }

        public static ILGenerator Shallow(this ILGenerator generator, Type type)
        {
            generator.DeclareLocal(type); // create local variable

            generator.Init(type); // Initialize a clone (into a local variable)

            _ = type switch
            {
                // If type is a value type or safe to copy reference type -> we can just load it as a shallow clone
                { } when type.IsValueType || type.IsSafeToCopyType() => generator.CopyByValue(),

                // If type us non-safe to copy reference type - use memberwise clone
                _ => generator.ShallowCopyReferenceType(type)
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return loaded value

            return generator;
        }
    }
}
