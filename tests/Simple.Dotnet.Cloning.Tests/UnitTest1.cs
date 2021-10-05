using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed class Test
    {
        public int Value { get; set; }
        public int? NullableInt { get; set; }
        public Guid? NullableGuid { get; set; }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var fields = typeof(Guid).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToArray();

            var guid = Guid.NewGuid();
            var clone = guid.DeepClone();

            var test = new Test { Value = 11, NullableInt = 12, NullableGuid = Guid.Empty };
            Expression<Func<int, int>> e = i => i;
            var cloned = test.DeepClone();
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
