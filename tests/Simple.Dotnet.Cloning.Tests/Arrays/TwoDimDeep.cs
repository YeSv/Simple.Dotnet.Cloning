using FluentAssertions;
using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Arrays
{
    public class TwoDimDeep
    {
        [Fact]
        public void TwoDimArrays_Should_Have_Different_References_And_Different_Values()
        {
            ShouldBeSame<string>().Should().BeTrue();
            ShouldBeSame<object>().Should().BeTrue();
            ShouldBeSame<Guid>().Should().BeTrue();

            ShouldBeSame<string>(0).Should().BeTrue();
            ShouldBeSame<object>(0).Should().BeTrue();
            ShouldBeSame<Guid>(0).Should().BeTrue();

            ShouldBeSame(10, 10, (i, j) => (object)null).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new object(), (f, s) => f != s).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new TwoDimDeep(), (f, s) => f != s).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => Guid.NewGuid()).Should().BeTrue();
        }

        [Fact]
        public void TwoDimArrays_Should_Have_Different_References_And_Different_Values_Wrappers()
        {
            ShouldBeSame(10, 10, (i, j) => new Wrapper<Guid[,]>(Generate<Guid>()), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue(); // Guid
            ShouldBeSame(10, 10, (i, j) => new Wrapper<object[,]>(Generate<object>()), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new Wrapper<Guid[,]>(Generate<Guid>(0)), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new Wrapper<object[,]>(Generate<object>(0)), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new Wrapper<Guid[,]>(Generate(10, 10, (i, j) => Guid.NewGuid())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new Wrapper<object[,]>(Generate(10, 10, (i, j) => new object())), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new Wrapper<TwoDimShallow[,]>(Generate(10, 10, (i, j) => new TwoDimShallow())), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<Guid[,]>(Generate<Guid>()), (f, s) => f == s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue(); // Guid
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<object[,]>(Generate<object>()), (f, s) => f == s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<Guid[,]>(Generate<Guid>(0)), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<object[,]>(Generate<object>(0)), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<Guid[,]>(Generate(10, 10, (i, j) => Guid.NewGuid())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<object[,]>(Generate(10, 10, (i, j) => new object())), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperRecord<TwoDimShallow[,]>(Generate(10, 10, (i, j) => new TwoDimShallow())), (f, s) => f != s && CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<Guid[,]>(Generate<Guid>()), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue(); // Guid
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<object[,]>(Generate<object>()), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<Guid[,]>(Generate<Guid>(0)), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<object[,]>(Generate<object>(0)), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<Guid[,]>(Generate(10, 10, (i, j) => Guid.NewGuid())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<object[,]>(Generate(10, 10, (i, j) => new object())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperStruct<TwoDimShallow[,]>(Generate(10, 10, (i, j) => new TwoDimShallow())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<Guid[,]>(Generate<Guid>()), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue(); // Guid
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<object[,]>(Generate<object>()), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<Guid[,]>(Generate<Guid>(0)), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<object[,]>(Generate<object>(0)), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<Guid[,]>(Generate(10, 10, (i, j) => Guid.NewGuid())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 == e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<object[,]>(Generate(10, 10, (i, j) => new object())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();
            ShouldBeSame(10, 10, (i, j) => new WrapperReadonly<TwoDimShallow[,]>(Generate(10, 10, (i, j) => new TwoDimShallow())), (f, s) => CompareAll(f.Value, s.Value, (e1, e2) => e1 != e2)).Should().BeTrue();

        }


        static T[,] Generate<T>(int? dim1 = null, int? dim2 = null, Func<int, int, T> @new = null) => dim1 switch
        {
            null => null,
            0 => new T[0, 0],
            _ => Init(dim1.Value, dim2.Value, @new)
        };

        static bool CompareAll<T>(T[,] array, T[,] clone, Func<T, T, bool> comparer)
        {
            if (array == null) return clone == null;
            if (array == clone) return false;
            if (array.Length != clone.Length) return false;

            if (comparer == null) return true;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (!comparer(array[i, j], clone[i, j])) return false;
                }
            }

            return true;
        }

        static T[,] Init<T>(int dim1, int dim2, Func<int, int, T> @new = null)
        {
            var array = new T[dim1, dim2];
            for (var i = 0; i < dim1; i++)
            {
                for (var j = 0; j < dim2; j++) array[i, j] = @new(i, j);
            }

            return array;
        }

        static bool ShouldBeSame<T>(
            int? dim1 = null,
            int? dim2 = null,
            Func<int, int, T> @new = null,
            Func<T, T, bool> comparer = null)
        {
            var array = Generate(dim1, dim2, @new);
            var clone = array.DeepClone();
            return CompareAll(array, clone, comparer);
        }
    }


}
