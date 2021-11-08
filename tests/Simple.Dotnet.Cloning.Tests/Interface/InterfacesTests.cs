using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Interface
{
    public class InterfacesTests
    {
        [Fact]
        public void Interfaces_Shallow_Should_Return_The_Same_Type_But_Different_Reference()
        {
            IsCloned((IEquatable<int>)4, false).Should().BeTrue();
            IsCloned(GenerateAsync(), false).Should().BeTrue();
            IsCloned(Enumerable.Range(0, 100), false, (f, s) => f.Count() == s.Count()).Should().BeTrue();
            IsCloned(Enumerable.Range(0, 100).OrderBy(i => i), false, (f, s) => f.Count() == s.Count()).Should().BeTrue();
        }

        [Fact]
        public void Interfaces_Deep_Should_Return_The_Same_Type_And_Different_Reference_With_Values()
        {
            IsCloned(GenerateAsync(), check: (f, s) => ForAll(f, s, (e1, e2) => e1 == e2).Result).Should().BeTrue();
            IsCloned((IEnumerable<int>)Enumerable.Range(0, 100).ToArray(), check: (f, s) => ForAll(f, s, (e1, e2) => e1 == e2)).Should().BeTrue();
            IsCloned((IEnumerable<InterfacesTests>)Enumerable.Range(0, 100).Select(i => new InterfacesTests()).ToArray(), check: (f, s) => ForAll(f, s, (e1, e2) => e1 != e2)).Should().BeTrue();
            IsCloned(Enumerable.Range(0, 100).ToArray().OrderBy(i => i), check: (f, s) => ForAll(f, s, (e1, e2) => e1 == e2)).Should().BeTrue();
        }

        static bool IsCloned<T>(T instance, bool deep = true, Func<T, T, bool> check = null) where T : class
        {
            var clone = deep ? instance.DeepClone() : instance.ShallowClone();
            if (instance == null) return clone == null;
            if (object.ReferenceEquals(instance, clone)) return false;

            return check?.Invoke(instance, clone) ?? true;
        }


        static async IAsyncEnumerable<int> GenerateAsync()
        {
            yield return 0;
            yield return -1;
            yield return 0;
        }

        static async Task<bool> ForAll<T>(IAsyncEnumerable<T> f, IAsyncEnumerable<T> s, Func<T, T, bool> comparer)
        {
            var (enumf, enums) = (f.GetAsyncEnumerator(), s.GetAsyncEnumerator());
            for (; ; )
            {
                var (firstNext, secondNext) = (await enumf.MoveNextAsync(), await enums.MoveNextAsync());
                if (!firstNext && !secondNext) return true;
                if (firstNext != secondNext) return false;

                if (!comparer(enumf.Current, enums.Current)) return false;
            }

            return true;
        }

        static bool ForAll<T>(IEnumerable<T> f, IEnumerable<T> s, Func<T, T, bool> comparer)
        {
            var (enumf, enums) = (f.GetEnumerator(), s.GetEnumerator());
            for (; ; )
            {
                var (firstNext, secondNext) = (enumf.MoveNext(), enums.MoveNext());
                if (!firstNext && !secondNext) return true;
                if (firstNext != secondNext) return false;

                if (!comparer(enumf.Current, enums.Current)) return false;
            }

            return true;
        }
    }
}
