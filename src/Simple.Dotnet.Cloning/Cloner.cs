using System;
using Simple.Dotnet.Cloning.Generators;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

using TypelessFunc = System.Func<object, object>;

namespace Simple.Dotnet.Cloning
{
    public static class Cloner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DeepClone<T>(this T instance) => Cloner<T>.DeepClone(instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ShallowClone<T>(this T instance) => Cloner<T>.ShallowClone(instance);
    }

    internal static class Cloner<T>
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

    internal static class TypelessCloner
    {
        static readonly Type ObjectType = typeof(object);
        static readonly Type OpenClonerType = typeof(Cloner<>);

        static readonly Func<Type, (TypelessFunc Shallow, TypelessFunc Deep)> Generator = t => Generate(t);

        static readonly ConcurrentDictionary<Type, (TypelessFunc Shallow, TypelessFunc Deep)> Cache = new ConcurrentDictionary<Type, (TypelessFunc Shallow, TypelessFunc Deep)>()
        {
            [ObjectType] = (obj => obj, obj => obj) // For object - do nothing both for shallow and deep clone
        }; // TODO: Find other thread safe approach?

        public static object DeepClone(object instance, Type type)
        {
            if (instance == null) return null;

            var (_, deep) = Cache.GetOrAdd(type, Generator);
            return deep(instance);
        }

        public static object ShallowClone(object instance, Type type)
        {
            if (instance == null) return null;

            var (shallow, _) = Cache.GetOrAdd(type, Generator);
            return shallow(instance);
        }

        static (TypelessFunc Shallow, TypelessFunc Deep) Generate(Type type)
        {
            var cloner = OpenClonerType.MakeGenericType(type);
            return 
            (
                CreateLambda(type, cloner.GetMethod(nameof(Cloner.ShallowClone), BindingFlags.Public | BindingFlags.Static)), 
                CreateLambda(type, cloner.GetMethod(nameof(Cloner.DeepClone), BindingFlags.Public | BindingFlags.Static))
            );
            
            static TypelessFunc CreateLambda(Type type, MethodInfo info)
            {
                var parameter = Expression.Parameter(ObjectType);
                return Expression.Lambda<TypelessFunc>(
                    Expression.Convert(
                        Expression.Call(
                            info,
                            Expression.Convert(parameter, type)),
                        ObjectType),
                    parameter).Compile();
            }
        }
    }
}
