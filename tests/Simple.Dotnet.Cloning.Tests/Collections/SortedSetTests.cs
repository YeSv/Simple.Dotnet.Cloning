using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class SortedSetTests : IComparable<SortedSetTests>
    {
        [Fact]
        public void SortedSet_Null_Should_Be_Null()
        {
            ((SortedSet<int>)null).DeepClone().Should().BeNull();
            ((SortedSet<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => i, (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new SortedSetTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<int>(10, i => i);
            new Wrapper<SortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<SortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<SortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<SortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void SortedSet_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<SortedSetTests>(10, i => new());
            IsEqual(collection, new Wrapper<SortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<SortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<SortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<SortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<int>(10, i => i);

            IsEqual(collection, (SortedSet<int>)((ISet<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (SortedSet<int>)((IEnumerable<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (SortedSet<int>)((ICollection<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<SortedSetTests>(10, i => new());

            IsEqual(collection, (SortedSet<SortedSetTests>)((ISet<SortedSetTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedSet<SortedSetTests>)((IEnumerable<SortedSetTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedSet<SortedSetTests>)((ICollection<SortedSetTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<int>(10, i => i);
            new Wrapper<IEnumerable<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void SortedSet_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<SortedSetTests>(10, i => new());
            IsEqual(collection, (SortedSet<SortedSetTests>)new Wrapper<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedSet<SortedSetTests>)new WrapperRecord<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedSet<SortedSetTests>)new WrapperStruct<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedSet<SortedSetTests>)new WrapperReadonly<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
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

        static SortedSet<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new SortedSet<T>();
            for (var i = 0; i < len.Value; i++) collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(SortedSet<T> collection, SortedSet<T> clone, Func<T, T, bool> comparer)
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

            return true;
        }

        public int CompareTo(SortedSetTests other) => 0;
    }
}
