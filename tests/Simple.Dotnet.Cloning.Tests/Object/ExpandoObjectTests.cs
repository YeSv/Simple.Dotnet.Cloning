using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Object
{
    public class ExpandoObjectTests
    {
        [Fact]
        public void ExpandoObject_As_Null_Cloned_Should_Be_Null()
        {
            ((ExpandoObject)null).ShallowClone().Should().BeNull();
            ((ExpandoObject)null).DeepClone().Should().BeNull();
        }
        
        [Fact]
        public void ExpandoObject_Should_Be_Shallow_Cloned()
        {
            dynamic obj = new ExpandoObject();
            obj.String = Guid.NewGuid().ToString();
            obj.Guid = Guid.NewGuid();
            obj.Nullable = new int?();
            obj.Enumerable = (IEnumerable<int>)Enumerable.Range(0, 100).ToArray();
            obj.Object = (object)new ExpandoObjectTests();
            obj.HashSet = new HashSet<object> { "test", 10, Guid.NewGuid() };
            obj.Null = (object)null;
            obj.HashNull = (HashSet<object>)null;
            obj.Expando = new ExpandoObject();

            dynamic clone = ((ExpandoObject)obj).ShallowClone();
            (((ExpandoObject)clone) != ((ExpandoObject)obj)).Should().BeTrue();
            ShouldBeEqual<object>(obj.Null, clone.Null);
            ShouldBeEqual<HashSet<object>>(obj.HashNull, clone.HashNull);
            ShouldBeEqual<Guid>(obj.Guid, clone.Guid);
            ShouldBeEqual<int?>(obj.Nullable, clone.Nullable);
            ShouldBeEqual<IEnumerable<int>>(obj.Enumerable, clone.Enumerable);
            ShouldBeEqual<object>(obj.Object, clone.Object);
            ShouldBeEqual<HashSet<object>>(obj.HashSet, clone.HashSet);
            ShouldBeEqual<ExpandoObject>(obj.Expando, clone.Expando);
        }

        [Fact]
        public void ExpandoObject_Should_Be_Deep_Cloned()
        {
            dynamic obj = new ExpandoObject();
            obj.String = Guid.NewGuid().ToString();
            obj.Guid = Guid.NewGuid();
            obj.Nullable = new int?();
            obj.Enumerable = (IEnumerable<int>)Enumerable.Range(0, 100).ToArray();
            obj.Object = (object)new ExpandoObjectTests();
            obj.HashSet = new HashSet<object> { "test", 10, Guid.NewGuid() };
            obj.Null = (object)null;
            obj.HashNull = (HashSet<object>)null;
            obj.Expando = new ExpandoObject();

            dynamic clone = ((ExpandoObject)obj).DeepClone();
            (((ExpandoObject)clone) != ((ExpandoObject)obj)).Should().BeTrue();
            ShouldBeEqual<Guid>(obj.Guid, clone.Guid);
            ShouldBeEqual<int?>(obj.Nullable, clone.Nullable);
            ShouldBeEqual<object>(obj.Null, clone.Null);
            ShouldBeEqual<HashSet<object>>(obj.HashNull, clone.HashNull);
            ShoulBeStructurallyEqual<IEnumerable<int>>(obj.Enumerable, clone.Enumerable);
            ShouldNotBeEqualByReference<object>(obj.Object, clone.Object);
            ShoulBeStructurallyEqual<HashSet<object>>(obj.HashSet, clone.HashSet);
            ShouldNotBeEqualByReference<ExpandoObject>(obj.Expando, clone.Expando);
        }

        static void ShouldBeEqual<T>(dynamic first, dynamic second) => ((T)first).Should().Be((T)second);

        static void ShouldNotBeEqualByReference<T>(dynamic first, dynamic second) where T : class => (((T)first) != ((T)second)).Should().BeTrue();

        static void ShoulBeStructurallyEqual<T>(dynamic first, dynamic second) where T : class
        {
            (((T)first) != ((T)second)).Should().BeTrue();
            ((T)first).Should().BeEquivalentTo((T)second);
        }
    }
}
