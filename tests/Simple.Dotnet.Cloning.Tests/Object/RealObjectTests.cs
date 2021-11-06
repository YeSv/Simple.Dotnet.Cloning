using FluentAssertions;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Object
{

    public class RealObjectTests
    {
        [Fact]
        public void Object_Should_Be_Just_Returned_New_On_Cloning()
        {
            var obj = new object();

            obj.ShallowClone().Should().NotBe(obj);
            obj.DeepClone().Should().NotBe(obj);


            ((object)null).ShallowClone().Should().BeNull();
            ((object)null).DeepClone().Should().BeNull();
        }

        [Fact]
        public void Object_Should_Be_Just_Returned_New_On_Cloning_Wrappers()
        {
            var obj = new object();
            var wrapper = new Wrapper<object>(obj);
            var wrapperRec = new WrapperRecord<object>(obj);
            var wrapperStruct = new WrapperStruct<object>(obj);
            var wrapperReadonly = new WrapperReadonly<object>(obj);

            wrapper.ShallowClone().Value.Should().Be(wrapper.Value);
            wrapperRec.ShallowClone().Value.Should().Be(wrapperRec.Value);
            wrapperReadonly.ShallowClone().Value.Should().Be(wrapperReadonly.Value);
            wrapperStruct.ShallowClone().Value.Should().Be(wrapperStruct.Value);

            wrapper.DeepClone().Value.Should().NotBe(wrapper.Value);
            wrapperRec.DeepClone().Value.Should().NotBe(wrapperRec.Value);
            wrapperReadonly.DeepClone().Value.Should().NotBe(wrapperReadonly.Value);
            wrapperStruct.DeepClone().Value.Should().NotBe(wrapperStruct.Value);
        }
    }
}
