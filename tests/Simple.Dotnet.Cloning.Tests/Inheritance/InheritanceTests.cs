using FluentAssertions;
using System;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Inheritance
{
    public class InheritanceTests
    {
        [Fact]
        public void ShallowClone_Should_Copy_Fields_From_Hierarchy()
        {
            IsCloned(new BaseClass(11, 12, 13) { Public = 14 }).Should().BeTrue();
            IsCloned(new ChildClass1(11, 12, 13) { Public = 14, Public1 = Guid.NewGuid() }).Should().BeTrue();
            IsCloned(new ChildClass2(11, 12, 13) { Public = 14, Public1 = Guid.NewGuid(), Child2 = 15 }).Should().BeTrue();
        }

        [Fact]
        public void DeepClone_Should_Copy_Fields_From_Hierarchy()
        {
            IsCloned(new BaseClass(11, 12, 13) { Public = 14 }, true).Should().BeTrue();
            IsCloned(new ChildClass1(11, 12, 13) { Public = 14, Public1 = Guid.NewGuid() }, true).Should().BeTrue();
            IsCloned(new ChildClass2(11, 12, 13) { Public = 14, Public1 = Guid.NewGuid(), Child2 = 15 }, true).Should().BeTrue();
        }

        static bool IsCloned<T>(T instance, bool deep = false) where T : class
        {
            var clone = deep ? instance.DeepClone() : instance.ShallowClone();

            if (clone == instance) return false;
            return Extensions.HaveSameData(instance, clone);
        }


        public class BaseClass
        {
            private int _private;
            protected int _protected;
            internal int _internal;

            public int Private => _private;
            public int Protected => _protected;
            public int Internal => _internal;

            public int Public { get; set; }

            public BaseClass(int @private, int @protected, int @internal)
            {
                _private = @private;
                _protected = @protected;
                _internal = @internal;
            }
        }

        public class ChildClass1 : BaseClass
        {
            private Guid _private1;
            protected Guid _protected1;
            internal Guid _internal1;

            public Guid Private1 => _private1;
            public Guid Protected1 => _protected1;
            public Guid Internal1 => _internal1;

            public Guid Public1 { get; set; }

            public ChildClass1(int @private, int @protected, int @internal) : base(@private, @protected, @internal)
            {
                _private1 = Guid.NewGuid();
                _internal1 = Guid.NewGuid();
                _protected1 = Guid.NewGuid();
            }
        }

        public class ChildClass2 : ChildClass1
        {
            public int Child2 { get; set; }

            public ChildClass2(int @private, int @protected, int @internal) : base(@private, @protected, @internal)
            { }
        }
    }
}
