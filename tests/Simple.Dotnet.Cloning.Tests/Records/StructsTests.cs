using FluentAssertions;
using Simple.Dotnet.Cloning.Tests.Common;
using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Records
{
    public class StructsTests
    {
        [Fact]
        public void Structs_Deep_Clone_Should_Copy_By_Value()
        {
            IsCloned(new SmallStruct(12, 24, Guid.NewGuid()), deep: true).Should().BeTrue();
            IsCloned(HugeValueStructGenerator.Generate(), deep: true).Should().BeTrue();
            IsCloned(HugeStructGenerator.Generate(), (f, s) => f.Value14 != s.Value14 && f.Value6 != s.Value6 && f.Value12 != s.Value12, true).Should().BeTrue();
        }

        [Fact]
        public void Structs_Shallow_Clone_Should_Copy()
        {
            IsCloned(new SmallStruct(12, 24, Guid.NewGuid())).Should().BeTrue();
            IsCloned(HugeValueStructGenerator.Generate()).Should().BeTrue();
            IsCloned(HugeStructGenerator.Generate(), (f, s) => f.Value14 == s.Value14 && f.Value6 == s.Value6 && f.Value12 == s.Value12).Should().BeTrue();
        }

        static bool IsCloned<T>(T instance, Func<T, T, bool> check = null, bool deep = false)
        {
            var clone = deep ? instance.DeepClone() : instance.ShallowClone();

            if (check?.Invoke(instance, clone) == false) return false;

            return Extensions.HaveSameData(instance, clone);
        }
    }
}
