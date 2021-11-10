using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Immutable
{
    public class HashSetTests
    {
        [Fact]
        public void HashSet_Null_Should_Be_Null()
        {
            ((ImmutableHashSet<int>)null).DeepClone().Should().BeNull();
            ((ImmutableHashSet<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void HashSet_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new HashSetTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void HashSet_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<ImmutableHashSet<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ImmutableHashSet<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ImmutableHashSet<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ImmutableHashSet<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void HashSet_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<HashSetTests>(10, i => new());
            IsEqual(collection, new Wrapper<ImmutableHashSet<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ImmutableHashSet<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ImmutableHashSet<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ImmutableHashSet<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void HashSet_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ImmutableHashSet<object>)((ISet<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<object>)((ICollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void HashSet_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<HashSetTests>(10, i => new());

            IsEqual(collection, (ImmutableHashSet<HashSetTests>)((ISet<HashSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)((IEnumerable<HashSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)((ICollection<HashSetTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void HashSet_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void HashSet_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<HashSetTests>(10, i => new());
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)new Wrapper<IEnumerable<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)new WrapperRecord<IEnumerable<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)new WrapperStruct<IEnumerable<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableHashSet<HashSetTests>)new WrapperReadonly<IEnumerable<HashSetTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static ImmutableHashSet<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return ImmutableHashSet<T>.Empty;

            var collection = ImmutableHashSet<T>.Empty;
            for (var i = 0; i < len.Value; i++) collection = collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ImmutableHashSet<T> collection, ImmutableHashSet<T> clone, Func<T, T, bool> comparer, bool deep = false)
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
    }
}
