using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Object
{
    public class FakeObjectTests
    {
        [Fact]
        public void FakeObject_Should_Be_Checked_For_Null()
        {
            ((object)(string)null).DeepClone().Should().BeNull();
            ((object)(string)null).ShallowClone().Should().BeNull();
        }

        [Fact]
        public void FakeObject_Should_Be_Shallow_Cloned()
        {
            ShouldBeEqual<int>(0, f => f.ShallowClone());
            ShouldBeEqual<Guid>(Guid.Empty, f => f.ShallowClone());
            ShouldBeStructurallyEqual<IEnumerable<FakeObjectTests>>(new FakeObjectTests[] { new(), new(), new() }, f => f.ShallowClone());
            ShouldBeStructurallyEqual<IReadOnlyDictionary<int, FakeObjectTests>>(new Dictionary<int, FakeObjectTests> { [1] = new(), [2] = new() }, f => f.ShallowClone());
        }

        [Fact]
        public void FakeObject_Should_Be_Deep_Cloned()
        {
            ShouldBeEqual<int>(0, f => f.DeepClone());
            ShouldBeEqual<Guid>(Guid.Empty, f => f.DeepClone());
            ShouldNotBeStructurallyEqual<IEnumerable<FakeObjectTests>>(new FakeObjectTests[] { new(), new(), new() }, f => f.DeepClone());
            ShouldNotBeStructurallyEqual<IReadOnlyDictionary<int, FakeObjectTests>>(new Dictionary<int, FakeObjectTests> { [1] = new(), [2] = new() }, f => f.DeepClone());
        }

        static void ShouldBeEqual<T>(object first, Func<object, object> cloner)
        {
            var second = cloner(first);
            (first != second).Should().BeTrue();
            ((T)first).Should().Be((T)second);
        }
        static void ShouldBeStructurallyEqual<T>(object first, Func<object, object> cloner) where T : class
        {
            var second = cloner(first);
            (first != second).Should().BeTrue();
            (((T)first) != ((T)second)).Should().BeTrue();
            ((T)first).Should().BeEquivalentTo((T)second);
        }

        static void ShouldNotBeStructurallyEqual<T>(object first, Func<object, object> cloner) where T : class
        {
            var second = cloner(first);
            (((T)first) != ((T)second)).Should().BeTrue();
            ((T)first).Should().NotBe((T)second);
        }
    }
}
