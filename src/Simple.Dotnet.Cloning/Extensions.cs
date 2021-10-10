using System;
using System.Linq.Expressions;

namespace Simple.Dotnet.Cloning
{
    internal static class TypeExtensions
    {
        static readonly Type DelegateType = typeof(Delegate);
        static readonly Type ExceptionType = typeof(Exception);
        static readonly Type ExpressionType = typeof(Expression);
        static readonly Type MulticastDelegateType = typeof(MulticastDelegate);

        public static bool IsSafeToCopyValueType(this Type type)
        {
            if (!type.IsValueType) return false;

            if (type.IsEnum) return true;
            if (type.IsPointer) return true;
            if (type.IsPrimitive) return true;
            if (type.IsMarshalByRef) return true;
            if (Types.SafeToCopy.Contains(type)) return true;

            // Handle nullables and their inner type
            var nullableInner = Nullable.GetUnderlyingType(type);
            if (nullableInner != null) return nullableInner.IsSafeToCopyValueType();

            // Some of types might be generic too, so check here 
            if (type.IsGenericType)
            {
                var openGeneric = type.GetGenericTypeDefinition();
                if (openGeneric != null && Types.SafeToCopy.Contains(openGeneric)) return true;
            }

            return false;
        }

        public static bool IsSafeToCopyType(this Type type)
        {
            if (Types.SafeToCopy.Contains(type)) return true;
            if (type.IsValueType) return type.IsSafeToCopyValueType();
            
            // Do nothing with marshal by ref
            if (type.IsMarshalByRef) return true;

            // Exceptions are ok
            if (ExceptionType.IsAssignableFrom(type)) return true;

            // Expressions are ok
            if (ExpressionType.IsAssignableFrom(type)) return true;

            // Delegates are ok
            if (DelegateType.IsAssignableFrom(type) || MulticastDelegateType.IsAssignableFrom(type)) return true;

            // Check if generic is safe to copy
            if (type.IsGenericType)
            {
                var openGeneric = type.GetGenericTypeDefinition();
                if (openGeneric != null && Types.SafeToCopy.Contains(openGeneric)) return true;
            }

            return false;
        }
    }
}
