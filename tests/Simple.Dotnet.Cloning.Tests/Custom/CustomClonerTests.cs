using FluentAssertions;
using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Custom
{
    public static class NullCloner<T> where T : class
    {
        public static T Clone(T instance) => null;
    }

    public sealed class NullClass { }

    public static class DoNothingCloner<T>
    {
        public static T Clone(T instance) => instance;
    }

    public sealed class DoNothingClass { }

    public sealed class GenericClass<T1, T2, T3>
    {
        public T1 V1 { get; set; }
        public T2 V2 { get; set; }
        public T3 V3 { get; set; }
    }

    public class CustomClonerTests
    {
        static CustomClonerTests()
        {
            CloningTypes.Custom.Add(typeof(DoNothingClass), _ => typeof(DoNothingCloner<>).MakeGenericType(typeof(DoNothingClass)).GetMethod("Clone"));
            CloningTypes.Custom.Add(typeof(NullClass), _ => typeof(NullCloner<>).MakeGenericType(typeof(NullClass)).GetMethod("Clone"));
            CloningTypes.Custom.Add(typeof(GenericClass<,,>), t => typeof(NullCloner<>).MakeGenericType(typeof(GenericClass<,,>).MakeGenericType(t)).GetMethod("Clone"));
        }

        [Fact]
        public void DoNothingCloner_Should_Return_The_Same_Instance()
        {
            var instance = new DoNothingClass();

            instance.ShallowClone().Should().NotBe(instance);
            instance.DeepClone().Should().Be(instance);

            ((DoNothingClass)null).ShallowClone().Should().BeNull();
            ((DoNothingClass)null).DeepClone().Should().BeNull();
        }

        [Fact]
        public void NullCloner_Should_Return_Null()
        {
            var instance = new NullClass();

            instance.ShallowClone().Should().NotBeNull();
            instance.DeepClone().Should().BeNull();

            ((NullClass)null).ShallowClone().Should().BeNull();
            ((NullClass)null).DeepClone().Should().BeNull();
        }

        [Fact]
        public void GenericClasses_UsingCustomCloner_Should_Work()
        {
            var instance = new GenericClass<Guid, int, string>();

            instance.ShallowClone().Should().NotBeNull();
            instance.DeepClone().Should().BeNull();

            ((GenericClass<Guid, int, string>)null).ShallowClone().Should().BeNull();
            ((GenericClass<Guid, int, string>)null).DeepClone().Should().BeNull();
        }
    }
}
