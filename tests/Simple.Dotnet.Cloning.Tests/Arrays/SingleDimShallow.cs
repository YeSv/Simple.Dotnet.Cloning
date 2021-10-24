using System;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Arrays
{
    public class SingleDimShallow
    {
        [Fact]
        public void SingleDimArrays_Should_Have_Different_References_But_Same_Values()
        {
            ShouldBeSame<string>();
            ShouldBeSame<object>();
            ShouldBeSame<Guid>();

            ShouldBeSame<string>(0);
            ShouldBeSame<object>(0);
            ShouldBeSame<Guid>(0);

            ShouldBeSame(10, i => (object)null);
            ShouldBeSame(10, i => i.ToString(), (f,s) => f == s);
            ShouldBeSame(10, i => new object(), (f,s) => f == s);
            ShouldBeSame(10, i => Guid.NewGuid());
        }

        [Fact]
        public void SingleDimArrays_Should_Have_Different_References_But_Same_Values_Wrappers()
        {
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate<Guid>()), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate<object>()), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate<Guid>(0)), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate<object>(0)), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate(10, i => new object())), (f, s) => f != s && f.Value == s.Value);


            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate<Guid>()), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate<object>()), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate<object>(0)), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate(10, i => new object())), (f, s) => f.Value == s.Value);


            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate<Guid>()), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate<object>()), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate<Guid>(0)), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate<object>(0)), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f != s && f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate(10, i => new object())), (f, s) => f != s && f.Value == s.Value);


            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate<Guid>()), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate<object>()), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate<object>(0)), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f.Value == s.Value);
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate(10, i => new object())), (f, s) => f.Value == s.Value);
        }

        static T[] Generate<T>(int? len = null, Func<int, T> @new = null) => len switch
        {
            null => null,
            0 => Array.Empty<T>(),
            _ => Enumerable.Range(0, len.Value).Select(@new).ToArray()
        };

        static bool ShouldBeSame<T>(
            int? len = null,
            Func<int, T> @new = null,
            Func<T, T, bool> comparer = null)
        {
            var array = Generate(len, @new);
            var clone = array.ShallowClone();

            if (array == null) return clone == null;
            if (array == clone) return false;
            if (array.Length == 0) return clone.Length == 0;
            if (array.Length != clone.Length) return false;

            if (comparer == null) return true;

            for (var i = 0; i < array.Length; i++)
            {
                if (!comparer(array[i], clone[i])) return false;
            }

            return true;
        }
    }


}
