
namespace Simple.Dotnet.Cloning.Tests
{
    public sealed class Wrapper<T>
    {
        public T Value { get; set; }

        public Wrapper(T instance) => Value = instance;
    }

    public readonly struct WrapperReadonly<T>
    {
        public T Value { get; }

        public WrapperReadonly(T value) => Value = value;
    }

    public struct WrapperStruct<T>
    {
        public T Value { get; }

        public WrapperStruct(T value) => Value = value;
    }

    public record WrapperRecord<T>(T Value);
}
