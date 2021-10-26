using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Arrays
{
    public class FiveDimArrays
    {
        [Fact]
        public void FiveDimArrays_Deep_Should_Be_Shallow()
        {
            ShouldBeSame(() => new object(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(() => Guid.NewGuid().ToString(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(() => Guid.NewGuid(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(() => new FiveDimArrays(), (f, s) => f == s, true).Should().BeTrue();


            ShouldBeSame(() => new object(), (f, s) => f == s, false).Should().BeTrue();
            ShouldBeSame(() => Guid.NewGuid().ToString(), (f, s) => f == s, false).Should().BeTrue();
            ShouldBeSame(() => Guid.NewGuid(), (f, s) => f == s, false).Should().BeTrue();
            ShouldBeSame(() => new FiveDimArrays(), (f, s) => f == s, false).Should().BeTrue();
        }


        static bool ShouldBeSame<T>(Func<T> @new, Func<T, T, bool> comparer, bool deep = true)
        {
            var array = Init(@new);
            var clone = deep ? array.DeepClone() : array.ShallowClone();
            return CompareAll(array, clone, comparer);
        }

        static T[,,,,] Init<T>(Func<T> @new)
        {
            var array = new T[3, 3, 3, 3, 3];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        for (var z = 0; z < 3; z++)
                        {
                            for (var q = 0; q < 3; q++) array[i, j, k, z, q] = @new();
                        }
                    }
                }
            }

            return array;
        }

        static bool CompareAll<T>(T[,,,,] array, T[,,,,] clone, Func<T, T, bool> comparer)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    for (var k = 0; k < array.GetLength(2); k++)
                    {
                        for (var z = 0; z < array.GetLength(3); z++)
                        {
                            for (var q = 0; q < array.GetLength(4); q++) 
                            {
                                if (!comparer(array[i, j, k, z, q], clone[i, j, k, z, q])) return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
