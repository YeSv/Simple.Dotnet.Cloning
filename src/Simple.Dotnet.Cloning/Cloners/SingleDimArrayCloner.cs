using System;

namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class SingleDimArrayCloner
    {
        public static T[] ShallowClone<T>(T[] array)
        {
            if (array == null) return null;
            if (array.Length == 0) return Array.Empty<T>();

            var clone = new T[array.Length];
            array.AsSpan().CopyTo(clone);

            return clone;
        }

        public static T[] DeepClone<T>(T[] array)
        {
            if (array == null) return null;
            if (array.Length == 0) return Array.Empty<T>();

            var clone = new T[array.Length];
            for (var i = 0; i < array.Length; i++) clone[i] = RootCloner<T>.DeepClone(array[i]);

            return clone;
        }
    } 
}
