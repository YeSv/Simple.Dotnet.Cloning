using Simple.Dotnet.Cloning.Cloners;
using System.Runtime.CompilerServices;

namespace Simple.Dotnet.Cloning
{
    public static class Cloner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DeepClone<T>(this T instance) => RootCloner<T>.DeepClone(instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ShallowClone<T>(this T instance) => RootCloner<T>.ShallowClone(instance);
    }
}
