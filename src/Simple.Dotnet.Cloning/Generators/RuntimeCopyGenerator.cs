using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class RuntimeCopyGenerator
    {
        static readonly Type ObjectType = typeof(object);
        static readonly MethodInfo GetTypeMethod = ObjectType.GetMethod(nameof(object.GetType), BindingFlags.Public | BindingFlags.Instance);
        static readonly MethodInfo DeepCloneMethod = typeof(ObjectCloner).GetMethod(nameof(ObjectCloner.DeepClone), BindingFlags.Public | BindingFlags.Static);
        static readonly MethodInfo ShallowCloneMethod = typeof(ObjectCloner).GetMethod(nameof(ObjectCloner.ShallowClone), BindingFlags.Public | BindingFlags.Static);

        public static ILGenerator CopyObject(this ILGenerator generator, bool deep = true)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load instance for deep clone
            generator.Emit(OpCodes.Ldarg_0); // Load instance to GetType
            generator.Emit(OpCodes.Callvirt, GetTypeMethod); // Call .GetType()
            generator.Emit(OpCodes.Call, deep ? DeepCloneMethod : ShallowCloneMethod); // Call DeepClone
            generator.Emit(OpCodes.Stloc_0); // store in local

            return generator;
        }

        public static ILGenerator CopyAbstractClass(this ILGenerator generator, Type type, bool deep = true)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load instance for deep clone
            generator.Emit(OpCodes.Castclass, ObjectType); // Cast to (object)
            generator.Emit(OpCodes.Ldarg_0); // Load instance to GetType
            generator.Emit(OpCodes.Callvirt, GetTypeMethod); // Call .GetType()
            generator.Emit(OpCodes.Call, deep ? DeepCloneMethod : ShallowCloneMethod); // Call DeepClone/ShallowClone
            generator.Emit(OpCodes.Castclass, type); // Cast back to required abstract class
            generator.Emit(OpCodes.Stloc_0); // store in local

            return generator;
        }

        public static ILGenerator CopyInterface(this ILGenerator generator, Type type, bool deep = true)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load instance for deep clone
            generator.Emit(OpCodes.Castclass, ObjectType); // Cast to (object)
            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Callvirt, GetTypeMethod); // Call .GetType()
            generator.Emit(OpCodes.Call, deep ? DeepCloneMethod : ShallowCloneMethod); // Call DeepClone
            generator.Emit(OpCodes.Castclass, type); // Cast back to required abstract class
            generator.Emit(OpCodes.Stloc_0); // store in local

            return generator;
        }
    }
}
