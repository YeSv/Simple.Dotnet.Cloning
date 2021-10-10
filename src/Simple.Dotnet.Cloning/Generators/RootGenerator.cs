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
                { } when type.IsSafeToCopyType() => generator.CopyByValue(), // When type is safe to copy - just load onto stack and return (for example: string is immutable)
                { IsValueType: true } => generator.CopyValueType(type, FieldsLazy), // Copy value type
                { IsInterface: true } => generator.CopyInterface(type), // Copy interface at runtime
                { IsAbstract: true } => generator.CopyAbstractClass(type), // Copy abstract class at runtime
                { } when type == ObjectType => generator.CopyObject(),  // Copy object at runtime
                { IsArray: true } when type.GetArrayRank() == 1 => generator.CopyOneDimArray(type), // Copy one dimention array
                _ => generator.CopyReferenceType(type, FieldsLazy(type)) // Copy reference type
                //  TODO: { IsArray: true } => generator.CopyReferenceType(type, Array.Empty<FieldInfo>()),
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
                { } when type.IsValueType || type.IsSafeToCopyType() => generator.CopyByValue(),
                { IsInterface: true } => generator.CopyInterface(type, false), // Copy interface at runtime
                { IsAbstract: true } => generator.CopyAbstractClass(type, false), // Copy abstract class at runtime
                { } when type == ObjectType => generator.CopyObject(false), // Copy object at runtime
                { IsArray: true } when type.GetArrayRank() == 1 => generator.CopyOneDimArray(type, false), // Copy one dimention array
                // If type us non-safe to copy reference type - use memberwise clone
                _ => generator.ShallowCopyReferenceType(type)
            };

            generator.Emit(OpCodes.Ldloc_0); // Load clone
            generator.Emit(OpCodes.Ret); // Return loaded value

            return generator;
        }
    }
}
