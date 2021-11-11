using FluentAssertions;
using Simple.Dotnet.Cloning.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Records
{
    public class ClassesTests
    {
        [Fact]
        public void ShallowClone_Should_Produce_The_Same_Data()
        {
            IsCloned(SmallClassGenerator.Generate()).Should().BeTrue();
            IsCloned(HugeClassGenerator.Generate(10, 10)).Should().BeTrue();
            IsCloned(ModerateClassGenerator.Generate(10, 10)).Should().BeTrue();
            IsCloned(StringsSmallClassGenerator.Generate(10)).Should().BeTrue();
        }

        [Fact]
        public void DeepClone_Should_Produce_The_Same_Data()
        {
            IsCloned(SmallClassGenerator.Generate(), true).Should().BeTrue();
            IsCloned(HugeClassGenerator.Generate(10, 10), true).Should().BeTrue();
            IsCloned(ModerateClassGenerator.Generate(10, 10), true).Should().BeTrue();
            IsCloned(StringsSmallClassGenerator.Generate(10), true).Should().BeTrue();
        }

        static bool IsCloned<T>(T instance, bool deep = false) where T : class
        {
            var clone = deep ? instance.DeepClone() : instance.ShallowClone();

            if (instance == null) return clone == null;
            if (instance == clone) return false;

            return Extensions.HaveSameData(instance, clone);
        }
    }
}
