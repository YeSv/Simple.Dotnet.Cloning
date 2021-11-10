using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Immutable
{
    public class SortedSetTests : IComparable<SortedSetTests>
    {
        [Fact]
        public void SortedSet_Null_Should_Be_Null()
        {
            ((ImmutableSortedSet<int>)null).DeepClone().Should().BeNull();
            ((ImmutableSortedSet<int>)null).ShallowClone().Should().BeNull();
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
            new Wrapper<ImmutableSortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ImmutableSortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ImmutableSortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ImmutableSortedSet<int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void SortedSet_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<SortedSetTests>(10, i => new());
            IsEqual(collection, new Wrapper<ImmutableSortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ImmutableSortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ImmutableSortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ImmutableSortedSet<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<int>(10, i => i);

            IsEqual(collection, (ImmutableSortedSet<int>)((ISet<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<int>)((IEnumerable<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<int>)((ICollection<int>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedSet_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<SortedSetTests>(10, i => new());

            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)((ISet<SortedSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)((IEnumerable<SortedSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)((ICollection<SortedSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
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
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)new Wrapper<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)new WrapperRecord<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)new WrapperStruct<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableSortedSet<SortedSetTests>)new WrapperReadonly<IEnumerable<SortedSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static ImmutableSortedSet<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return ImmutableSortedSet<T>.Empty;

            var collection = ImmutableSortedSet<T>.Empty;
            for (var i = 0; i < len.Value; i++) collection = collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ImmutableSortedSet<T> collection, ImmutableSortedSet<T> clone, Func<T, T, bool> comparer, bool deep = false)
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
