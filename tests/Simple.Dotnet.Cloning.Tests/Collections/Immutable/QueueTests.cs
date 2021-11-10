using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Immutable
{
    public class QueueTests
    {
        [Fact]
        public void Queue_Null_Should_Be_Null()
        {
            ((ImmutableQueue<int>)null).DeepClone().Should().BeNull();
            ((ImmutableQueue<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void Queue_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new QueueTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Queue_DeepClone_Should_Have_Cloned_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f != s, true).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => new QueueTests(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Queue_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<ImmutableQueue<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ImmutableQueue<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ImmutableQueue<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ImmutableQueue<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Queue_DeepClone_Should_Have_Cloned_Values_Wrappers()
        {
            var collection = Create<QueueTests>(10, i => new());
            IsEqual(collection, new Wrapper<ImmutableQueue<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ImmutableQueue<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ImmutableQueue<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ImmutableQueue<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Queue_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ImmutableQueue<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableQueue<object>)((IImmutableQueue<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Queue_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<QueueTests>(10, i => new());

            IsEqual(collection, (ImmutableQueue<QueueTests>)((IEnumerable<QueueTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableQueue<QueueTests>)((IImmutableQueue<QueueTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Queue_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Queue_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<QueueTests>(10, i => new());
            IsEqual(collection, (ImmutableQueue<QueueTests>)new Wrapper<IEnumerable<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableQueue<QueueTests>)new WrapperRecord<IEnumerable<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableQueue<QueueTests>)new WrapperStruct<IEnumerable<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableQueue<QueueTests>)new WrapperReadonly<IEnumerable<QueueTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static ImmutableQueue<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return ImmutableQueue<T>.Empty;

            var collection = ImmutableQueue<T>.Empty;
            for (var i = 0; i < len.Value; i++) collection = collection.Enqueue(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ImmutableQueue<T> collection, ImmutableQueue<T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count() != clone.Count()) return false;

            var (first, second) = (collection.GetEnumerator(), clone.GetEnumerator());
            for (var i = 0; i < collection.Count(); i++)
            {
                if (!first.MoveNext() || !second.MoveNext()) return false;
                if (!comparer(first.Current, second.Current)) return false;
            }

            return true;
        }
    }
}