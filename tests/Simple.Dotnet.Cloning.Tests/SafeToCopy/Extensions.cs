using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.SafeToCopy
{
    public class Extensions
    {
        [Fact]
        public void Types_Should_Be_Safe_To_Copy() => CloningTypes.SafeToCopy.All(t => t.IsSafeToCopy()).Should().BeTrue();

        [Fact]
        public void Delegates_Should_Be_Safe_To_Copy()
        {
            SafeToCopy<Action>();
            SafeToCopy<Action<int>>();
            SafeToCopy<Action<int, int, object, object>>();
            SafeToCopy<Func<int>>();
            SafeToCopy<Func<int, int, int, object>>();
        }

        [Fact]
        public void Exceptions_Should_Be_Safe_To_Copy()
        {
            SafeToCopy<Exception>();
            SafeToCopy<ArgumentException>();
            SafeToCopy<NullReferenceException>();
            SafeToCopy<UnauthorizedAccessException>();
            SafeToCopy<OutOfMemoryException>();
        }

        [Fact]
        public void Expressions_Should_Be_Safe_To_Copy()
        {
            SafeToCopy<Expression>();
            SafeToCopy<BinaryExpression>();
            SafeToCopy<TypeBinaryExpression>();
            SafeToCopy<BlockExpression>();
        }

        [Fact]
        public void Generics_Closed_Should_Be_Safe_To_Copy()
        {
            SafeToCopy<AsyncLocal<int>>();
            SafeToCopy<Lazy<int>>();
            SafeToCopy<Task<object>>();
        }

        [Fact]
        public void Nullables_Should_Be_Safe_To_Copy_If_Underlying_Is_Safe_To_Copy()
        {
            SafeToCopy<int?>();
            SafeToCopy<char?>();
            SafeToCopy<byte?>();
            SafeToCopy<UIntPtr?>();
        }

        [Fact]
        public void Object_Should_Not_Be_Safe_To_Copy() => typeof(object).IsSafeToCopy().Should().BeFalse();

        static void SafeToCopy<T>() => typeof(T).IsSafeToCopy().Should().BeTrue();

    }
}
