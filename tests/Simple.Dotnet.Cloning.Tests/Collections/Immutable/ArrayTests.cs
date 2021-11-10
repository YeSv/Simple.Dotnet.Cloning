using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Immutable
{
    public class ArrayTests
    {
        [Fact]
        public void Array_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new ArrayTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Array_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(1, i => new());
            new Wrapper<ImmutableArray<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperRecord<ImmutableArray<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperReadonly<ImmutableArray<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperStruct<ImmutableArray<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void Array_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<ArrayTests>(10, i => new());
            IsEqual(collection, new Wrapper<ImmutableArray<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ImmutableArray<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ImmutableArray<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ImmutableArray<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Array_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ImmutableArray<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableArray<object>)((ICollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Array_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<ArrayTests>(10, i => new());

            IsEqual(collection, (ImmutableArray<ArrayTests>)((IEnumerable<ArrayTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableArray<ArrayTests>)((ICollection<ArrayTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Array_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void Array_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<ArrayTests>(10, i => new());
            IsEqual(collection, (ImmutableArray<ArrayTests>)new Wrapper<IEnumerable<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableArray<ArrayTests>)new WrapperRecord<IEnumerable<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableArray<ArrayTests>)new WrapperStruct<IEnumerable<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableArray<ArrayTests>)new WrapperReadonly<IEnumerable<ArrayTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static ImmutableArray<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return ImmutableArray<T>.Empty;
            if (len == 0) return ImmutableArray<T>.Empty;

            var collection = ImmutableArray<T>.Empty;
            for (var i = 0; i < len.Value; i++) collection = collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ImmutableArray<T> collection, ImmutableArray<T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection.Length != clone.Length) return false;

            for (var i = 0; i < collection.Length; i++)
            {
                if (!comparer(collection[i], clone[i])) return false;
            }
            
            return true;
        }
    }
}
