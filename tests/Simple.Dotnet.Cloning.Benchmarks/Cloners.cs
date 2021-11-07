using AutoMapper;
using Force.DeepCloner;
using MessagePack;
using System.Text.Json;

namespace Simple.Dotnet.Cloning.Benchmarks
{
    public static class AutoMapperCloner
    {
        static readonly IMapper _mapper = new MapperConfiguration(c => { }).CreateMapper();

        public static T ShallowClone<T>(T value) => _mapper.Map<T>(value);
    }

    public static class MessagePackCloner
    {
        public static T DeepClone<T>(T value) => MessagePackSerializer.Deserialize<T>(MessagePackSerializer.Serialize(value));
    }

    public static class JsonCloner
    {
        public static T DeepClone<T>(T value) => JsonSerializer.Deserialize<T>(JsonSerializer.SerializeToUtf8Bytes(value));
    }

    public static class ForceCloner
    {
        public static T DeepClone<T>(T value) => DeepClonerExtensions.DeepClone(value);
        public static T ShallowClone<T>(T value) => DeepClonerExtensions.ShallowClone(value);
    }

    public static class SimpleCloner
    {
        public static T DeepClone<T>(T value) => Cloner.DeepClone(value);
        public static T ShallowClone<T>(T value) => Cloner.ShallowClone(value);
    }
}
