using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections.Concurrent
{
    public class DictionaryTests
    {
        [Fact]
        public void Dictionary_Null_Should_Be_Null()
        {
            ((ConcurrentDictionary<int, int>)null).DeepClone().Should().BeNull();
            ((ConcurrentDictionary<int, int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void Dictionary_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new DictionaryTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<ConcurrentDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<ConcurrentDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<ConcurrentDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<ConcurrentDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Dictionary_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<DictionaryTests>(10, i => new());
            IsEqual(collection, new Wrapper<ConcurrentDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<ConcurrentDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<ConcurrentDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<ConcurrentDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (ConcurrentDictionary<int, object>)((IDictionary<int, object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, object>)((IReadOnlyDictionary<int, object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, object>)((IEnumerable<KeyValuePair<int, object>>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_DeepClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<DictionaryTests>(10, i => new());

            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)((IDictionary<int, DictionaryTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)((IEnumerable<KeyValuePair<int, DictionaryTests>>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)((IReadOnlyDictionary<int, DictionaryTests>)collection).DeepClone(), (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void Dictionary_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<DictionaryTests>(10, i => new());
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)new Wrapper<IDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)new WrapperRecord<IDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)new WrapperStruct<IDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
            IsEqual(collection, (ConcurrentDictionary<int, DictionaryTests>)new WrapperReadonly<IDictionary<int, DictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s, true).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_Value_Key_Collections_DeepClone_Should_Have_Different_Values()
        {
            var collection = Create<DictionaryTests>(10, i => new());
            var values = collection.Values;
            var keys = collection.Keys;

            IsEqual(keys, keys.DeepClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(values, values.DeepClone(), (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void Dictionary_Value_Key_Collections_ShallowClone_Should_Have_Same_Values()
        {
            var collection = Create<DictionaryTests>(10, i => new());
            var values = collection.Values;
            var keys = collection.Keys;

            IsEqual(keys, keys.ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(values, values.ShallowClone(), (f, s) => f == s).Should().BeTrue();
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

        static ConcurrentDictionary<int, T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new ConcurrentDictionary<int, T>();
            for (var i = 0; i < len.Value; i++) collection.TryAdd(i, @new(i));

            return collection;
        }

        static bool IsEqual<T>(ConcurrentDictionary<int, T> collection, ConcurrentDictionary<int, T> clone, Func<T, T, bool> comparer, bool deep = false)
        {
            if (collection == null || clone == null) return collection == clone;
            if (collection == clone) return false;
            if (collection.Count != clone.Count) return false;

            var (first, second) = (collection.GetEnumerator(), clone.GetEnumerator());
            for (var i = 0; i < collection.Count; i++)
            {
                if (!first.MoveNext() || !second.MoveNext()) return false;
                if (!comparer(first.Current.Value, second.Current.Value)) return false;
            }

            if (deep)
            {
                clone.TryAdd(int.MinValue, default);
                if (clone.Count == collection.Count) return false;
                if (clone.Count() == collection.Count()) return false;
                if (clone.Values.Count() == collection.Values.Count()) return false;
                if (clone.Keys.Count() == collection.Keys.Count()) return false;
            }

            return true;
        }

        // For KeysCollection/ValuesCollection
        static bool IsEqual<T>(IEnumerable<T> collection, IEnumerable<T> clone, Func<T, T, bool> comparer)
        {
            if (collection == null || clone == null) return collection == clone;
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
