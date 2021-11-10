using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Concurrent
{
    public class ConcurrentBagTests
    {
        [Fact]
        public void ConcurrentBag_Null_Should_Be_Null()
        {
            ((ConcurrentBag<int>)null).DeepClone().Should().BeNull();
            ((ConcurrentBag<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void ConcurrentBag_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new ConcurrentBagTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void ConcurrentBag_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<ConcurrentBag<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ConcurrentBag<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ConcurrentBag<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ConcurrentBag<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void ConcurrentBag_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<ConcurrentBagTests>(10, i => new());
            IsEqual(collection, new Wrapper<ConcurrentBag<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ConcurrentBag<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ConcurrentBag<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ConcurrentBag<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void ConcurrentBag_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ConcurrentBag<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ConcurrentBag<object>)((IReadOnlyCollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void ConcurrentBag_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<ConcurrentBagTests>(10, i => new());

            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)((IEnumerable<ConcurrentBagTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)((IReadOnlyCollection<ConcurrentBagTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void ConcurrentBag_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void ConcurrentBag_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<ConcurrentBagTests>(10, i => new());
            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)new Wrapper<IEnumerable<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)new WrapperRecord<IEnumerable<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)new WrapperStruct<IEnumerable<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentBag<ConcurrentBagTests>)new WrapperReadonly<IEnumerable<ConcurrentBagTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        static bool ShouldBeSame<T>(
            int? len = null,
            Func<int, T> @new = null,
            Func<T, T, bool> comparer = null,
            bool deep = false)
        {
            var collection = Create(len, @new);
            return IsEqual(collection, deep ? collection.DeepClone() : collection.ShallowClone(), comparer);
        }

        static ConcurrentBag<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new ConcurrentBag<T>();
            for (var i = 0; i < len.Value; i++) collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ConcurrentBag<T> collection, ConcurrentBag<T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count != clone.Count) return false;

            var (first, second) = (collection.GetEnumerator(), clone.GetEnumerator());
            for (var i = 0; i < collection.Count; i++)
            {
                if (!first.MoveNext() || !second.MoveNext()) return false;
                if (!comparer(first.Current, second.Current)) return false;
            }

            if (deep)
            {
                clone.Add(default);
                if (clone.Count == collection.Count) return false;
                if (clone.Count() == collection.Count()) return false;
            }

            return true;
        }
    }
}
