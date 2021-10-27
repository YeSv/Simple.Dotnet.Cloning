using FluentAssertions;
using Xunit;

namespace Simple.Dotnet.Cloning.Tests.Object
{

    public class RealObjectTests
    {
        [Fact]
        public void Object_Should_Be_Just_Returned_On_Cloning()
        {
            var obj = new object();

            obj.ShallowClone().Should().Be(obj);
            obj.DeepClone().Should().Be(obj);


            ((object)null).ShallowClone().Should().BeNull();
            ((object)null).DeepClone().Should().BeNull();
        }

        [Fact]
        public void Object_Should_Be_Just_Returned_On_Cloning_Wrappers()
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

            wrapper.DeepClone().Value.Should().Be(wrapper.Value);
            wrapperRec.DeepClone().Value.Should().Be(wrapperRec.Value);
            wrapperReadonly.DeepClone().Value.Should().Be(wrapperReadonly.Value);
            wrapperStruct.DeepClone().Value.Should().Be(wrapperStruct.Value);
        }
    }
}
