using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class RootGenerator
    {
        static readonly Type ObjectType = typeof(object);
        static readonly Func<Type, FieldInfo[]> FieldsLazy = type => type.GetFields(FieldFlags);
        static readonly BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static ILGenerator Deep(this ILGenerator generator, Type type)
        {
            generator.DeclareLocal(type); // create local variable
            generator.Default(type); // Init value type as 0 or null for ref type
  
            _ = type switch
            {
                { } when type.IsCustomCloning() => generator.CopyCustomCloningType(type), // When type has custom cloner - use it instead of all other
                { } when type.IsSafeToCopy() => generator.CopyByValue(), // When type is safe to copy - just load onto stack and return (for example: string is immutable)
                { IsValueType: true } => generator.CopyValueType(type, FieldsLazy), // Copy value type
                { IsInterface: true } => generator.CopyInterface(type), // Copy interface at runtime
                { IsAbstract: true } => generator.CopyAbstractClass(type), // Copy abstract class at runtime
                { } when type == ObjectType => generator.CopyObject(),  // Copy object at runtime
                { IsArray: true } => generator.CopyArray(type), // Copy array
                _ => generator.CopyReferenceType(type, FieldsLazy(type)) // Copy reference type
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return clone's value

            return generator;
        }

        public static ILGenerator Shallow(this ILGenerator generator, Type type)
        {
            generator.DeclareLocal(type); // create local variable
            generator.Default(type); // Init value type as 0 or null for ref type

            _ = type switch
            {
                // If type is a value type or safe to copy reference type -> we can just load it as a shallow clone
                { } when type.IsValueType || type.IsSafeToCopy() => generator.CopyByValue(),
                { IsInterface: true } => generator.CopyInterface(type, false), // Copy interface at runtime
                { IsAbstract: true } => generator.CopyAbstractClass(type, false), // Copy abstract class at runtime
                { } when type == ObjectType => generator.CopyObject(false), // Copy object at runtime
                { IsArray: true } => generator.CopyArray(type, false), // Copy array
                _ => generator.ShallowCopyReferenceType(type, FieldsLazy(type))
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return loaded value

            return generator;
        }
    }
}
