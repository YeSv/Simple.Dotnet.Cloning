using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class PriorityQueueTests
    {
        [Fact]
        public void PriorityQueue_Null_Should_Be_Null()
        {
            ((PriorityQueue<string, int>)null).DeepClone().Should().BeNull();
            ((PriorityQueue<string, int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void PriorityQueue_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new PriorityQueueTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void PriorityQueue_DeepClone_Should_Have_Cloned_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f != s, true).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => new PriorityQueueTests(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void PriorityQueue_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<PriorityQueue<object, int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<PriorityQueue<object, int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<PriorityQueue<object, int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<PriorityQueue<object, int>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void PriorityQueue_DeepClone_Should_Have_Cloned_Values_Wrappers()
        {
            var collection = Create<PriorityQueueTests>(10, i => new());
            IsEqual(collection, new Wrapper<PriorityQueue<PriorityQueueTests, int>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<PriorityQueue<PriorityQueueTests, int>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<PriorityQueue<PriorityQueueTests, int>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<PriorityQueue<PriorityQueueTests, int>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static PriorityQueue<T, int> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return Unwrap(new());

            var collection = new PriorityQueue<T, int>();
            for (var i = 0; i < len.Value; i++) collection.Enqueue(@new(i), i);

            return Unwrap(collection);

            static PriorityQueue<T, int> Unwrap(PriorityQueue<T, int> collection)
            {
                if (collection.UnorderedItems.Count() < -1) throw new InvalidOperationException("Never occures");
                return collection;
            }
        }

        static bool IsEqual<T>(PriorityQueue<T, int> collection, PriorityQueue<T, int> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count != clone.Count) return false;

            var (first, second) = (collection.UnorderedItems.GetEnumerator(), clone.UnorderedItems.GetEnumerator());
            for (var i = 0; i < collection.Count; i++)
            {
                if (!first.MoveNext() || !second.MoveNext()) return false;
                if (!comparer(first.Current.Element, second.Current.Element)) return false;
            }

            if (deep)
            {
                clone.Enqueue(default(T), default);
                if (clone.Count == collection.Count) return false;
                if (clone.UnorderedItems.Count() == collection.UnorderedItems.Count()) return false;
            }

            return true;
        }
    }
}