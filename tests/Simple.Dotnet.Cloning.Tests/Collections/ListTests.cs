using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class ListTests
    {
        [Fact]
        public void List_Null_Should_Be_Null()
        {
            ((List<int>)null).DeepClone().Should().BeNull();
            ((List<int>)null).ShallowClone().Should().BeNull();
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
            new Wrapper<List<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<List<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<List<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<List<object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void List_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<ListTests>(10, i => new());
            IsEqual(collection, new Wrapper<List<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<List<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<List<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<List<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void List_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (List<object>)((IList<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (List<object>)((IEnumerable<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (List<object>)((ICollection<object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void List_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<ListTests>(10, i => new());

            IsEqual(collection, (List<ListTests>)((IList<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (List<ListTests>)((IEnumerable<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (List<ListTests>)((ICollection<ListTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
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
            IsEqual(collection, (List<ListTests>)new Wrapper<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (List<ListTests>)new WrapperRecord<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (List<ListTests>)new WrapperStruct<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (List<ListTests>)new WrapperReadonly<IEnumerable<ListTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
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

        static List<T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new List<T>();
            for (var i = 0; i < len.Value; i++) collection.Add(@new(i));

            return collection;
        }

        static bool IsEqual<T>(List<T> collection, List<T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count != clone.Count) return false;
            if (collection.Capacity != clone.Capacity) return false;

            for (var i = 0; i < collection.Count; i++)
            {
                if (!comparer(collection[i], clone[i])) return false;
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
