using System;
using Simple.Dotnet.Cloning.Generators;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class RootCloner<T>
    {
        internal delegate T CloneDelegate(T instance);

        static readonly Type Type = typeof(T);
        static readonly Module ClonerModule = typeof(Cloner).Module;

        static readonly CloneDelegate DeepCloner = CreateDeepCloner();
        static readonly CloneDelegate ShallowCloner = CreateShallowCloner();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DeepClone(T instance) => DeepCloner(instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ShallowClone(T instance) => ShallowCloner(instance);

        static CloneDelegate CreateShallowCloner()
        {
            var method = new DynamicMethod($"{Type.FullName}_{nameof(ShallowClone)}", Type, new[] { Type }, ClonerModule, true);

            method.GetILGenerator().Shallow(Type);
            return method.CreateDelegate(typeof(CloneDelegate)) as CloneDelegate;
        }

        static CloneDelegate CreateDeepCloner()
        {
            var method = new DynamicMethod($"{Type.FullName}_{nameof(DeepClone)}", Type, new[] { Type }, ClonerModule, true);

            method.GetILGenerator().Deep(Type);
            return method.CreateDelegate(typeof(CloneDelegate)) as CloneDelegate;
        }
    }
}
