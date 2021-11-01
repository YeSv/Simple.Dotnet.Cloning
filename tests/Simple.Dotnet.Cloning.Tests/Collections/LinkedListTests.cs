using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class LinkedListTests
    {
        [Fact]
        public void LinkedList_Null_Should_Be_Null()
        {
            ((LinkedList<int>)null).DeepClone().Should().BeNull();
            ((LinkedList<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void LinkedList_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new LinkedListTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void LinkedList_DeepClone_Should_Have_Cloned_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => new LinkedListTests(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void LinkedList_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<LinkedList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<LinkedList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<LinkedList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<LinkedList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void LinkedList_DeepClone_Should_Have_Cloned_Values_Wrappers()
        {
            var collection = Create<LinkedListTests>(10, i => new());
            IsEqual(collection, new Wrapper<LinkedList<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<LinkedList<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<LinkedList<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<LinkedList<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void LinkedList_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (LinkedList<object>)((IReadOnlyCollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (LinkedList<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (LinkedList<object>)((ICollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void LinkedList_ShallowClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<LinkedListTests>(10, i => new());

            IsEqual(collection, (LinkedList<LinkedListTests>)((IReadOnlyCollection<LinkedListTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (LinkedList<LinkedListTests>)((IEnumerable<LinkedListTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (LinkedList<LinkedListTests>)((ICollection<LinkedListTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void LinkedList_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void LinkedList_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<LinkedListTests>(10, i => new());
            IsEqual(collection, (LinkedList<LinkedListTests>)new Wrapper<IEnumerable<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (LinkedList<LinkedListTests>)new WrapperRecord<IEnumerable<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (LinkedList<LinkedListTests>)new WrapperStruct<IEnumerable<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (LinkedList<LinkedListTests>)new WrapperReadonly<IEnumerable<LinkedListTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
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

        static LinkedList<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new LinkedList<T>();
            for (var i = 0; i < len.Value; i++) collection.AddLast(@new(i));

            return collection;
        }

        static bool IsEqual<T>(LinkedList<T> collection, LinkedList<T> clone, Func<T, T, bool> comparer)
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
