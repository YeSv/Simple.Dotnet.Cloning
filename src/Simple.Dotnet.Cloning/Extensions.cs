using System;

namespace Simple.Dotnet.Cloning
{

    internal static class TypeExtensions
    {
        public static bool IsSafeToCopyValueType(this Type type)
        {
            if (!type.IsValueType) return false;

            if (type.IsPrimitive) return true;
            if (type.IsEnum) return true;

            var nullableInner = Nullable.GetUnderlyingType(type);
            return nullableInner != null && nullableInner.IsSafeToCopyValueType();
        }
    }
}
