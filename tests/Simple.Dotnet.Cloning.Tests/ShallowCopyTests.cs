using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests
{
    public sealed class ShallowCopyTests
    {
        [Fact]
        public void ShallowClone_Should_Clone_Known_Structs()
        {
            for (var i = 0; i < 100; i++)
            {
                i.ShallowClone().Should().Be(i);
                ((char)i).ShallowClone().Should().Be((char)i);
                ((long)i).ShallowClone().Should().Be(i);
                ((decimal)i).ShallowClone().Should().Be(i);
                ((decimal)i).ShallowClone().Should().Be(i);
                ((uint)i).ShallowClone().Should().Be((uint)i);
                ((ulong)i).ShallowClone().Should().Be((ulong)i);

                var guid = Guid.NewGuid();
                guid.ShallowClone().Should().Be(guid);

                var dateTime = DateTime.UtcNow;
                dateTime.ShallowClone().Should().Be(dateTime);

                var offset = DateTimeOffset.UtcNow;
                offset.ShallowClone().Should().Be(offset);

                var timeSpan = dateTime.TimeOfDay;
                timeSpan.ShallowClone().Should().Be(timeSpan);
            }
        }

        [Fact]
        public void ShallowClone_Should_Clone_Structs()
        {
            for (var i = 0; i < 100; i++)
            {
                var @struct = new Struct(i, i, i);
                var clone = @struct.ShallowClone();

                clone.Int.Should().Be(@struct.Int);
                clone.Decimal.Should().Be(@struct.Decimal);
                clone.Double.Should().Be(@struct.Double);
            }
        }

        [Fact]
        public void ShallowClone_Should_Clone_Structs_With_Properties()
        {
            for (var i = 0; i < 100; i++)
            {
                var @struct = new StructProperties(i, i, i);
                var clone = @struct.ShallowClone();

                clone.Int.Should().Be(@struct.Int);
                clone.Decimal.Should().Be(@struct.Decimal);
                clone.Double.Should().Be(@struct.Double);
            }
        }

        [Fact]
        public void ShallowClone_Should_Clone_Readonly_Structs()
        {
            for (var i = 0; i < 100; i++)
            {
                var @struct = new ReadonlyStruct(i, i, i);
                var clone = @struct.ShallowClone();

                clone.Int.Should().Be(@struct.Int);
                clone.Decimal.Should().Be(@struct.Decimal);
                clone.Double.Should().Be(@struct.Double);
            }
        }

        [Fact]
        public void ShallowClone_Should_Clone_Readonly_Structs_With_Properties()
        {
            for (var i = 0; i < 100; i++)
            {
                var @struct = new ReadonlyStructProperties(i, i, i);
                var clone = @struct.ShallowClone();

                clone.Int.Should().Be(@struct.Int);
                clone.Decimal.Should().Be(@struct.Decimal);
                clone.Double.Should().Be(@struct.Double);
            }
        }

        [Fact]
        public void ShallowClone_Should_Clone_Nulls()
        {
            ((object)null).ShallowClone();
        }

        // TODO: interfaces/objects/abstract classes
        [Fact]
        public void ShallowClone_Should_Clone_Known_Classes()
        {
            AssertEnumerable(Enumerable.Range(0, 100).ToHashSet());
            AssertEnumerable(new SortedSet<int> { 1, 2, 3, 4 });
            AssertArray(Enumerable.Range(0, 100).ToArray());
            AssertDictionary(Enumerable.Range(0, 100).ToDictionary(v => v, v => v.ToString()));

            nameof(ShallowClone_Should_Clone_Known_Classes).ShallowClone().Should().Be(nameof(ShallowClone_Should_Clone_Known_Classes));


            static void AssertDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary) where TKey : IEquatable<TKey> where TValue : IEquatable<TValue>
            {
                var clone = dictionary.ShallowClone();
                (clone != dictionary).Should().BeTrue();
                clone.Count.Should().Be(dictionary.Count);

                foreach (var kvp in clone)
                {
                    dictionary.ContainsKey(kvp.Key).Should().BeTrue();
                    dictionary[kvp.Key].Equals(kvp.Value).Should().BeTrue();
                }
            }

            static void AssertEnumerable<T>(IEnumerable<T> enumerable) where T : IEquatable<T>
            {
                var clone = enumerable.ShallowClone();
                (clone != enumerable).Should().BeTrue();
                clone.Count().Should().Be(enumerable.Count());

                var cloneEnum = clone.GetEnumerator();
                var enumerableEnum = enumerable.GetEnumerator();

                while (cloneEnum.MoveNext() && enumerableEnum.MoveNext()) cloneEnum.Current.Equals(enumerableEnum.Current).Should().BeTrue();
            }

            static void AssertArray<T>(T[] array) where T : IEquatable<T>
            {
                var clone = array.ShallowClone();
                (clone != array).Should().BeTrue();
                clone.Length.Should().Be(array.Length);

                for (var i = 0; i < clone.Length; i++) array[i].Equals(clone[i]).Should().BeTrue();
            }
        }

        public struct Struct
        {
            public readonly int Int;
            public readonly double Double;
            public readonly decimal Decimal;

            public Struct(int @int, double @double, decimal @decimal)
            {
                Int = @int;
                Double = @double;
                Decimal = @decimal;
            }
        }

        public struct StructProperties
        {
            public int Int { get; }
            public readonly double Double { get; }
            public readonly decimal Decimal { get; }

            public StructProperties(int @int, double @double, decimal @decimal)
            {
                Int = @int;
                Double = @double;
                Decimal = @decimal;
            }
        }

        public readonly struct ReadonlyStruct
        {
            public readonly int Int;
            public readonly double Double;
            public readonly decimal Decimal;

            public ReadonlyStruct(int @int, double @double, decimal @decimal)
            {
                Int = @int;
                Double = @double;
                Decimal = @decimal;
            }
        }

        public readonly struct ReadonlyStructProperties
        {
            public int Int { get; }
            public readonly double Double { get; }
            public readonly decimal Decimal { get; }

            public ReadonlyStructProperties(int @int, double @double, decimal @decimal)
            {
                Int = @int;
                Double = @double;
                Decimal = @decimal;
            }
        }
    }
}
