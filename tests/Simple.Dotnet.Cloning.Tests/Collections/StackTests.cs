using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class StackTests
    {
        [Fact]
        public void Stack_Null_Should_Be_Null()
        {
            ((Stack<int>)null).DeepClone().Should().BeNull();
            ((Stack<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void Stack_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new StackTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Stack_DeepClone_Should_Have_Cloned_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f != s, true).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s, true).Should().BeTrue();
            ShouldBeSame(10, i => new StackTests(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Stack_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<Stack<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<Stack<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<Stack<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<Stack<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Stack_DeepClone_Should_Have_Cloned_Values_Wrappers()
        {
            var collection = Create<StackTests>(10, i => new());
            IsEqual(collection, new Wrapper<Stack<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<Stack<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<Stack<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<Stack<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void Stack_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (Stack<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (Stack<object>)((IReadOnlyCollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Stack_ShallowClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<StackTests>(10, i => new());

            IsEqual(collection, (Stack<StackTests>)((IEnumerable<StackTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (Stack<StackTests>)((IReadOnlyCollection<StackTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void Stack_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Stack_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<StackTests>(10, i => new());
            IsEqual(collection, (Stack<StackTests>)new Wrapper<IEnumerable<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (Stack<StackTests>)new WrapperRecord<IEnumerable<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (Stack<StackTests>)new WrapperStruct<IEnumerable<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (Stack<StackTests>)new WrapperReadonly<IEnumerable<StackTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
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

        static Stack<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new Stack<T>();
            for (var i = 0; i < len.Value; i++) collection.Push(@new(i));

            return collection;
        }

        static bool IsEqual<T>(Stack<T> collection, Stack<T> clone, Func<T, T, bool> comparer)
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