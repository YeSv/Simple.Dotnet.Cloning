using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class ObjectCloner
    {
        internal delegate object CloneDelegate(object instance);

        static readonly Type ObjectType = typeof(object);
        static readonly Type OpenClonerType = typeof(RootCloner<>);

        static readonly Func<Type, (CloneDelegate Shallow, CloneDelegate Deep)> Generator = t => Generate(t);

        static readonly ConcurrentDictionary<Type, (CloneDelegate Shallow, CloneDelegate Deep)> Cache = new ConcurrentDictionary<Type, (CloneDelegate Shallow, CloneDelegate Deep)>()
        {
            [ObjectType] = (obj => new(), obj => new()) // For object - allocate new one
        }; // TODO: Find other thread safe approach?

        public static object DeepClone(object instance, Type type)
        {
            var (_, deep) = Cache.GetOrAdd(type, Generator);
            return deep(instance);
        }

        public static object ShallowClone(object instance, Type type)
        {
            var (shallow, _) = Cache.GetOrAdd(type, Generator);
            return shallow(instance);
        }

        static (CloneDelegate Shallow, CloneDelegate Deep) Generate(Type type)
        {
            var cloner = OpenClonerType.MakeGenericType(type);
            return
            (
                CreateLambda(type, cloner.GetMethod(nameof(Cloner.ShallowClone), BindingFlags.Public | BindingFlags.Static)),
                CreateLambda(type, cloner.GetMethod(nameof(Cloner.DeepClone), BindingFlags.Public | BindingFlags.Static))
            );

            static CloneDelegate CreateLambda(Type type, MethodInfo info)
            {
                var parameter = Expression.Parameter(ObjectType);
                return Expression.Lambda<CloneDelegate>(
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
