using FluentAssertions;
using Simple.Dotnet.Cloning.Tests.Common;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Records
{
    public class AbstractTests
    {
        [Fact]
        public void ShallowClone_Should_Produce_The_Same_Data()
        {
            IsCloned<HugeClass>(HugeClassGenerator.Generate(10, 10)).Should().BeTrue();
            IsCloned<ModerateClass>(ModerateClassGenerator.Generate(10, 10)).Should().BeTrue();
            IsCloned<SmallClass>(SmallClassGenerator.Generate()).Should().BeTrue();
            IsCloned<StringsSmallClass>(StringsSmallClassGenerator.Generate(10)).Should().BeTrue();
        }

        [Fact]
        public void DeepClone_Should_Produce_The_Same_Data()
        {
            IsCloned<HugeClass>(HugeClassGenerator.Generate(10, 10), true).Should().BeTrue();
            IsCloned<ModerateClass>(ModerateClassGenerator.Generate(10, 10), true).Should().BeTrue();
            IsCloned<SmallClass>(SmallClassGenerator.Generate(), true).Should().BeTrue();
            IsCloned<StringsSmallClass>(StringsSmallClassGenerator.Generate(10), true).Should().BeTrue();
        }

        static bool IsCloned<T>(AbstractClass instance, bool deep = false) where T : AbstractClass
        {
            var clone = deep ? instance.DeepClone() : instance.ShallowClone();

            if (instance == null) return clone == null;
            if (instance == clone) return false;

            return Extensions.HaveSameData((T)instance, (T)clone);
        }
    }
}
