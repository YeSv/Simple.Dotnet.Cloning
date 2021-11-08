using FluentAssertions;
using System.Numerics;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Nullable
{
    public class NullableTests
    {
        [Fact]
        public void Nullable_Deep_Clone_Should_Clone_References_Fields()
        {
            var unknown = new Unknown { Value1 = 1, Obj = new() };
            var clone = unknown.DeepClone();

            ReferenceEquals(clone.Obj, unknown.Obj).Should().BeFalse();
        }

        [Fact]
        public void Nullable_Shallow_Clone_Should_Not_Clone_References_Fields()
        {
            var unknown = new Unknown { Value1 = 1, Obj = new() };
            var clone = unknown.ShallowClone();

            ReferenceEquals(clone.Obj, unknown.Obj).Should().BeTrue();
        }

        [Fact]
        public void Nullable_Null_Should_Be_Cloned_As_Is()
        {
            new Unknown?().ShallowClone().Should().Be(new Unknown?());
            new Unknown?().DeepClone().Should().Be(new Unknown?());
        }

        struct Unknown
        {
            public int Value1;
            public object Obj;
        }
    }
}
