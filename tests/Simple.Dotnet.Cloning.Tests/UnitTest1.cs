using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class Abstract
    {
    }

    public class Implementation : Abstract
    {
        public int Integer { get; set; }
    }

    public sealed class Empty
    {
    }

    public sealed class EmptyOfEmpty
    {
        public Empty Empty { get; set; }
    }


    public sealed class Test
    {
        public int Value { get; set; }
        public int? NullableInt { get; set; }
        public Guid? NullableGuid { get; set; }
        public object Object { get; set; }
        public IEquatable<int> Interface { get; set; }
        public Abstract Abstract { get; set; }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var fields = typeof(Guid).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToArray();

            var guid = Guid.NewGuid();
            var clone = guid.DeepClone();

            var empty = new Empty();
            var emptyC = empty.DeepClone();

            var emptyOfEmpty = new EmptyOfEmpty { Empty = new Empty() };
            var eoeC = emptyOfEmpty.DeepClone();

            var test = new Test { Value = 11, NullableInt = 12, NullableGuid = Guid.Empty, Object = 5, Interface = 25, Abstract = new Implementation { Integer = 12 }};
            Expression<Func<int, int>> e = i => i;
            var cloned = test.DeepClone().ShallowClone();

            for (var i = 0; i < 10_000; i++) cloned = test.DeepClone().ShallowClone();

        }

        /*[Fact]
        public void Test2()
        {

            var shallow = TypelessCloner.ShallowClone(new Test { }, typeof(Test));
            var deep = TypelessCloner.DeepClone(new Test { }, typeof(Test));


            var guidShallow = TypelessCloner.ShallowClone(Guid.NewGuid(), typeof(Guid));
            var guidDeep = TypelessCloner.DeepClone(Guid.NewGuid(), typeof(Guid));
        }*/
    }
}
