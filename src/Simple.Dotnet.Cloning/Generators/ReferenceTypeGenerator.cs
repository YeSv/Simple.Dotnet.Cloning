using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ReferenceTypeGenerator
    {
        static readonly MethodInfo MemberwiseClone = typeof(object).GetMethod(nameof(MemberwiseClone), BindingFlags.Instance | BindingFlags.NonPublic);

        public static ILGenerator CopyReferenceType(this ILGenerator generator, Type type, FieldInfo[] fields)
        {
            var notNullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check equality
            generator.Emit(OpCodes.Brfalse_S, notNullLabel); // If not null go to label
            generator.Emit(OpCodes.Ldnull); // Load null if instance is null
            generator.Emit(OpCodes.Stloc_0); // Store latest value on a stack as a clone
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit :)

            generator.MarkLabel(notNullLabel);
            
            // If all fields are safe to copy - we can just use shallow cloning in other case - copy fields value by value
            _ = fields.All(f => f.FieldType.IsSafeToCopyType()) ? generator.CallMemberwiseClone(type) : generator.Init(type).CopyFields(type, fields);
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit :)

            generator.MarkLabel(exitLabel);

            return generator;
        }

        public static ILGenerator ShallowCopyReferenceType(this ILGenerator generator, Type type)
        {
            var notNullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check equality
            generator.Emit(OpCodes.Brfalse_S, notNullLabel); // If not null go to label
            generator.Emit(OpCodes.Ldnull); // Load null if instance is null
            generator.Emit(OpCodes.Stloc_0); // Store latest value on a stack as a clone
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit :)

            generator.MarkLabel(notNullLabel); // If not null
            generator.CallMemberwiseClone(type); // Just call memberwise clone
            generator.Emit(OpCodes.Br_S, exitLabel); // Go to exit

            generator.MarkLabel(exitLabel);
            
            return generator;
        }

        static ILGenerator CallMemberwiseClone(this ILGenerator generator, Type type)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load local onto stack
            generator.Emit(OpCodes.Callvirt, MemberwiseClone); // Call memberwiseClone on object
            generator.Emit(OpCodes.Castclass, type); // Cast returned object to our type
            generator.Emit(OpCodes.Stloc_0); // Store latest value on a stack as a clone

            return generator;
        }
    }
}
