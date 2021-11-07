using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Simple.Dotnet.Cloning.Generators
{
    internal static class ReferenceTypeGenerator
    {
        public static ILGenerator CopyReferenceType(this ILGenerator generator, Type type, FieldInfo[] fields)
        {
            var notNullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check equality
            generator.Emit(OpCodes.Brfalse, notNullLabel); // If not null go to label
            generator.Emit(OpCodes.Ldnull); // Load null if instance is null
            generator.Emit(OpCodes.Stloc_0); // Store latest value on a stack as a clone
            generator.Emit(OpCodes.Br, exitLabel); // Go to exit :)

            generator.MarkLabel(notNullLabel);
            generator.Init(type); // Init clone
            
            // If all fields are safe to copy - we can just use copy fields as is in other case - copy fields value by value
            _ = fields.All(f => f.FieldType.IsSafeToCopy()) ? generator.ShallowCopyFields(type, fields) : generator.DeepCopyFields(type, fields);
            generator.Emit(OpCodes.Br, exitLabel); // Go to exit :)

            generator.MarkLabel(exitLabel);

            return generator;
        }

        public static ILGenerator ShallowCopyReferenceType(this ILGenerator generator, Type type, FieldInfo[] fields)
        {
            var notNullLabel = generator.DefineLabel();
            var exitLabel = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_0); // Load instance
            generator.Emit(OpCodes.Ldnull); // Load null
            generator.Emit(OpCodes.Ceq); // Check equality
            generator.Emit(OpCodes.Brfalse, notNullLabel); // If not null go to label
            generator.Emit(OpCodes.Ldnull); // Load null if instance is null
            generator.Emit(OpCodes.Stloc_0); // Store latest value on a stack as a clone
            generator.Emit(OpCodes.Br, exitLabel); // Go to exit :)

            generator.MarkLabel(notNullLabel); // If not null
            generator.Init(type); // Init clone
            generator.ShallowCopyFields(type, fields); // Copy field by field
            generator.Emit(OpCodes.Br, exitLabel); // Go to exit

            generator.MarkLabel(exitLabel);
            
            return generator;
        }
    }
}
