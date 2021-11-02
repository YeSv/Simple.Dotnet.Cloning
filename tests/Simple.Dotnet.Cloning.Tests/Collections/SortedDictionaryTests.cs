﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Collections
{
    public class SortedDictionaryTests
    {
        [Fact]
        public void SortedDictionary_Null_Should_Be_Null()
        {
            ((SortedDictionary<int, int>)null).DeepClone().Should().BeNull();
            ((SortedDictionary<int, int>)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void SortedDictionary_ShallowClone_Should_Have_The_Same_Values()
        {
            ShouldBeSame(10, i => new object(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => i.ToString(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => Guid.NewGuid(), (f, s) => f == s).Should().BeTrue();
            ShouldBeSame(10, i => new SortedDictionaryTests(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_ShallowClone_Should_Be_Same_Wrappers()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<SortedDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<SortedDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<SortedDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<SortedDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void SortedDictionary_DeepClone_Should_Have_Cloned_Values()
        {
            var collection = Create<SortedDictionaryTests>(10, i => new());
            IsEqual(collection, new Wrapper<SortedDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperRecord<SortedDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperStruct<SortedDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, new WrapperReadonly<SortedDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_ShallowClone_Should_Be_Same_As_Interface()
        {
            var collection = Create<object>(10, i => new());

            IsEqual(collection, (SortedDictionary<int, object>)((IDictionary<int, object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, object>)((IReadOnlyDictionary<int, object>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, object>)((IEnumerable<KeyValuePair<int, object>>)collection).ShallowClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_ShallowClone_Should_Be_Different_As_Interface()
        {
            var collection = Create<SortedDictionaryTests>(10, i => new());

            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)((IDictionary<int, SortedDictionaryTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)((IEnumerable<KeyValuePair<int, SortedDictionaryTests>>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)((IReadOnlyDictionary<int, SortedDictionaryTests>)collection).DeepClone(), (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_ShallowClone_Should_Have_Same_Collection_As_Interface()
        {
            var collection = Create<object>(10, i => new());
            new Wrapper<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperRecord<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperReadonly<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
            new WrapperStruct<IDictionary<int, object>>(collection).ShallowClone().Value.Should().BeSameAs(collection);
        }

        [Fact]
        public void SortedDictionary_DeepClone_Should_Have_Different_Values_As_Interface()
        {
            var collection = Create<SortedDictionaryTests>(10, i => new());
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)new Wrapper<IDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)new WrapperRecord<IDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)new WrapperStruct<IDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
            IsEqual(collection, (SortedDictionary<int, SortedDictionaryTests>)new WrapperReadonly<IDictionary<int, SortedDictionaryTests>>(collection).DeepClone().Value, (f, s) => f != s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_Value_Key_Collections_DeepClone_Should_Have_Same_Values()
        {
            var collection = Create<SortedDictionaryTests>(10, i => new());
            var values = collection.Values;
            var keys = collection.Keys;

            IsEqual(keys, keys.DeepClone(), (f, s) => f == s).Should().BeTrue();
            IsEqual(values, values.DeepClone(), (f, s) => f == s).Should().BeTrue();
        }

        [Fact]
        public void SortedDictionary_Value_Key_Collections_ShallowClone_Should_Have_Same_Values()
        {
            var collection = Create<SortedDictionaryTests>(10, i => new());
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

        static SortedDictionary<int, T> Create<T>(int? len = null, Func<int, T> @new = null)
        {
            if (len == null) return null;
            if (len == 0) return new();

            var collection = new SortedDictionary<int, T>();
            for (var i = 0; i < len.Value; i++) collection.Add(i, @new(i));

            return collection;
        }

        static bool IsEqual<T>(SortedDictionary<int, T> collection, SortedDictionary<int, T> clone, Func<T, T, bool> comparer)
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
