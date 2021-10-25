using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Arrays
{
    public class SingleDimDeep
    {
        [Fact]
        public void SingleDimArrays_Should_Have_Different_References_And_Different_Values()
        {
            ShouldBeSame<string>().Should().BeTrue();
            ShouldBeSame<object>().Should().BeTrue();
            ShouldBeSame<Guid>().Should().BeTrue();

            ShouldBeSame<string>(0).Should().BeTrue();
            ShouldBeSame<object>(0).Should().BeTrue();
            ShouldBeSame<Guid>(0).Should().BeTrue();

            ShouldBeSame(10, i => (object)null).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f,s) => f == s).Should().BeTrue(); // strings
            ShouldBeSame(10, i => new object(), (f,s) => f == s).Should().BeTrue(); // objects
            ShouldBeSame(10, i => new SingleDimDeep(), (f, s) => f != s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid()).Should().BeTrue();
        }

        [Fact]
        public void SingleDimArrays_Should_Have_Different_References_And_Different_Values_Wrappers()
        {
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate<Guid>()), (f, s) => f != s && f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate<object>()), (f, s) => f != s && f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate<Guid>(0)), (f, s) => f != s && f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate<object>(0)), (f, s) => f != s && f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new Wrapper<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new Wrapper<object[]>(Generate(10, i => new object())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new Wrapper<SingleDimDeep[]>(Generate(10, i => new SingleDimDeep())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate<Guid>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate<object>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate<Guid>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate<object>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperRecord<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperRecord<object[]>(Generate(10, i => new object())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperRecord<SingleDimDeep[]>(Generate(10, i => new SingleDimDeep())), (f, s) => f != s && f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate<Guid>()), (f, s) => f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate<object>()), (f, s) => f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate<object>(0)), (f, s) => f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperReadonly<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperReadonly<object[]>(Generate(10, i => new object())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperReadonly<SingleDimDeep[]>(Generate(10, i => new SingleDimDeep())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate<Guid>()), (f, s) => f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate<object>()), (f, s) => f.Value == s.Value).Should().BeTrue(); // nulls
            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate<object>(0)), (f, s) => f.Value == s.Value).Should().BeTrue(); // array.Empty
            ShouldBeSame(10, i => new WrapperStruct<Guid[]>(Generate(10, i => Guid.NewGuid())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperStruct<object[]>(Generate(10, i => new object())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, i => new WrapperStruct<SingleDimDeep[]>(Generate(10, i => new SingleDimDeep())), (f, s) => f.Value != s.Value && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();
        }

        static T[] Generate<T>(int? len = null, Func<int, T> @new = null) => len switch
        {
            null => null,
            0 => Array.Empty<T>(),
            _ => Enumerable.Range(0, len.Value).Select(@new).ToArray()
        };

        static bool CompareAll<T>(T[] array, T[] clone, Func<T, T, bool> comparer = null)
        {
            if (array == null) return clone == null;
            if (array.Length == 0) return clone.Length == 0;
            if (array == clone) return false;
            if (array.Length != clone.Length) return false;

            if (comparer == null) return true;

            for (var i = 0; i < array.Length; i++)
            {
                if (!comparer(array[i], clone[i])) return false;
            }

            return true;
        }

        static bool ShouldBeSame<T>(
            int? len = null,
            Func<int, T> @new = null,
            Func<T, T, bool> comparer = null)
        {
            var array = Generate(len, @new);
            return CompareAll(array, array.DeepClone(), comparer);
        }
    }


}
