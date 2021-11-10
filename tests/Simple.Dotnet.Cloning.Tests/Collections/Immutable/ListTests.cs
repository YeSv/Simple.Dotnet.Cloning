using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Immutable
{
    public class ListTests
    {
        [Fact]
        public void List_Null_Should_Be_Null()
        {
            ((ImmutableList<int>)null).DeepClone().Should().BeNull();
            ((ImmutableList<int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void List_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new ListTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void List_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<ImmutableList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ImmutableList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ImmutableList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ImmutableList<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void List_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<ListTests>(10, i => new());
            IsEqual(collection, new Wrapper<ImmutableList<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ImmutableList<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ImmutableList<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ImmutableList<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void List_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ImmutableList<object>)((IImmutableList<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableList<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ImmutableList<object>)((ICollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void List_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<ListTests>(10, i => new());

            IsEqual(collection, (ImmutableList<ListTests>)((IImmutableList<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableList<ListTests>)((IEnumerable<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableList<ListTests>)((ICollection<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void List_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IEnumerable<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void List_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<ListTests>(10, i => new());
            IsEqual(collection, (ImmutableList<ListTests>)new Wrapper<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableList<ListTests>)new WrapperRecord<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableList<ListTests>)new WrapperStruct<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ImmutableList<ListTests>)new WrapperReadonly<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static ImmutableList<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return ImmutableList<T>.Empty;

            var collection = ImmutableList<T>.Empty;
            for (var i = 0; i < len.Value; i++) collection = collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(ImmutableList<T> collection, ImmutableList<T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count != clone.Count) return false;

            for (var i = 0; i < collection.Count; i++)
            {
                if (!comparer(collection[i], clone[i])) return false;
            }
            
            return true;
        }
    }
}
