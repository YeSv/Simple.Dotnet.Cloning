using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Simple.Dotnet.Cloning.Generators
{
    public static class InitializationGenerator
    {
        private static readonly MethodInfo GetUninitializedObject = typeof(FormatterServices).GetMethod(
            nameof(FormatterServices.GetSafeUninitializedObject),
            BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo GetTypeFromHandle = typeof(Type).GetMethod(
            nameof(Type.GetTypeFromHandle),
            BindingFlags.Public | BindingFlags.Static);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILGenerator Init(this ILGenerator generator, Type type)
        {
            if (type.IsValueType) return Struct(generator, type);

            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor != null) return RefTypeCtor(generator, ctor);

            return RefTypeUninitialized(generator, type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator Struct(ILGenerator generator, Type type)
        {
            generator.Emit(OpCodes.Ldloca_S, (byte)0); // local_0
            generator.Emit(OpCodes.Initobj, type); // default(T)

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator RefTypeCtor(ILGenerator generator, ConstructorInfo ctor)
        {
            generator.Emit(OpCodes.Newobj, ctor); // call new
            generator.Emit(OpCodes.Stloc_0); // Store as local variable 0

            return generator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ILGenerator RefTypeUninitialized(ILGenerator generator, Type type)
        {
            generator.Emit(OpCodes.Ldtoken, type); // typeof()
            generator.Emit(OpCodes.Call, GetTypeFromHandle); // GetTypeFromHandle(handle)
            generator.Emit(OpCodes.Call, GetUninitializedObject); // GetUninitObject(type)
            generator.Emit(OpCodes.Castclass, type); // (cast)
            generator.Emit(OpCodes.Stloc_0); // set local to instance

            return generator;
        }
    }
}
