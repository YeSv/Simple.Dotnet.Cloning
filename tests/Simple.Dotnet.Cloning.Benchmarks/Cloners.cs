using AutoMapper;
using Force.DeepCloner;
using MessagePack;
using System;
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

    public static class NClonerCloner
    {
        public static T DeepClone<T>(T value)
        {
            try
            {
                return NClone.Clone.ObjectGraph(value); // Fails for dictionaries and linked lists
            }
            catch 
            {
                Console.WriteLine("NCloner exception occurred");
                return default;
            }
        }
    }

    public static class DeepCopyCloner
    {
        public static T DeepClone<T>(T value)
        {
            try
            {
                return DeepCopy.DeepCopier.Copy(value);
            }
            catch
            {
                Console.WriteLine("DeepCopy exception occurred");
                return default;
            }
        }
    }

    public static class FastDeepClonerCloner
    {
        public static T DeepClone<T>(T value) => (T)FastDeepCloner.DeepCloner.Clone(value);
    }

    public static class SimpleCloner
    {
        public static T DeepClone<T>(T value) => Cloner.DeepClone(value);
        public static T ShallowClone<T>(T value) => Cloner.ShallowClone(value);
    }


    public static class Warmup
    {
        public static void WarmupFor<T>(T instance)
        {
            NClonerCloner.DeepClone(instance);
            DeepCopyCloner.DeepClone(instance);
            FastDeepClonerCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
            ForceCloner.DeepClone(instance);
        }
    }
}
