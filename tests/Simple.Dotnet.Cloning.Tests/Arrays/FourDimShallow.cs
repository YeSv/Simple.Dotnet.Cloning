using FluentAssertions;
using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Arrays
{
    public class FourDimShallow
    {
        [Fact]
        public void FourDimArrays_Should_Have_Different_References_But_Same_Values()
        {
            ShouldBeSame<string>().Should().BeTrue();
            ShouldBeSame<object>().Should().BeTrue();
            ShouldBeSame<Guid>().Should().BeTrue();

            ShouldBeSame<string>(0).Should().BeTrue();
            ShouldBeSame<object>(0).Should().BeTrue();
            ShouldBeSame<Guid>(0).Should().BeTrue();

            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => (object)null).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => Guid.NewGuid()).Should().BeTrue();
        }

        [Fact]
        public void FourDimArrays_Should_Have_Different_References_But_Same_Values_Wrappers()
        {
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<Guid[,,,]>(Generate<Guid>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<object[,,,]>(Generate<object>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<Guid[,,,]>(Generate<Guid>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<object[,,,]>(Generate<object>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<Guid[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => Guid.NewGuid())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<object[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new object())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new Wrapper<FourDimShallow[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new FourDimShallow())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();

            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<Guid[,,,]>(Generate<Guid>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<object[,,,]>(Generate<object>()), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<Guid[,,,]>(Generate<Guid>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<object[,,,]>(Generate<object>(0)), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<Guid[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => Guid.NewGuid())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<object[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new object())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperRecord<FourDimShallow[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new FourDimShallow())), (f, s) => f == s && f.Value == s.Value).Should().BeTrue();

            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<Guid[,,,]>(Generate<Guid>()), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<object[,,,]>(Generate<object>()), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<Guid[,,,]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<object[,,,]>(Generate<object>(0)), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<Guid[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => Guid.NewGuid())), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<object[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new object())), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperStruct<FourDimShallow[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new FourDimShallow())), (f, s) => f.Value == s.Value).Should().BeTrue();

            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<Guid[,,,]>(Generate<Guid>()), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<object[,,,]>(Generate<object>()), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<Guid[,,,]>(Generate<Guid>(0)), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<object[,,,]>(Generate<object>(0)), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<Guid[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => Guid.NewGuid())), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<object[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new object())), (f, s) => f.Value == s.Value).Should().BeTrue();
            ShouldBeSame(3, 3, 3, 3, (i, j, k, z) => new WrapperReadonly<FourDimShallow[,,,]>(Generate(3, 3, 3, 3, (i, j, k, z) => new FourDimShallow())), (f, s) => f.Value == s.Value).Should().BeTrue();
        }


        static T[,,,] Generate<T>(int? dim1 = null, int? dim2 = null, int? dim3 = null, int? dim4 = null, Func<int, int, int, int, T> @new = null) => dim1 switch
        {
            null => null,
            0 => new T[0, 0, 0, 0],
            _ => Init(dim1.Value, dim2.Value, dim3.Value, dim4.Value, @new)
        };

        static T[,,,] Init<T>(int dim1, int dim2, int dim3, int dim4, Func<int, int, int, int, T> @new = null)
        {
            var array = new T[dim1, dim2, dim3, dim4];
            for (var i = 0; i < dim1; i++)
            {
                for (var j = 0; j < dim2; j++)
                {
                    for (var k = 0; k < dim3; k++)
                    {
                        for (var z = 0; z < dim4; z++) array[i, j, k, z] = @new(i, j, k, z);
                    }
                }
            }

            return array;
        }

        static bool ShouldBeSame<T>(
            int? dim1 = null,
            int? dim2 = null,
            int? dim3 = null,
            int? dim4 = null,
            Func<int, int, int, int, T> @new = null,
            Func<T, T, bool> comparer = null)
        {
            var array = Generate(dim1, dim2, dim3, dim4, @new);
            var clone = array.ShallowClone();

            if (array == null) return clone == null;
            if (array.Length == 0) return clone.Length == 0;
            if (array == clone) return false;
            if (array.Length != clone.Length) return false;


            if (comparer == null) return true;
            for (var i = 0; i < dim1.Value; i++)
            {
                for (var j = 0; j < dim2.Value; j++)
                {
                    for (var k = 0; k < dim3.Value; k++)
                    {
                        for (var z = 0; z < dim4.Value; z++)
                        {
                            if (!comparer(array[i, j, k, z], clone[i, j, k, z])) return false;
                        }
                    }
                }
            }

            return true;
        }
    }


}
