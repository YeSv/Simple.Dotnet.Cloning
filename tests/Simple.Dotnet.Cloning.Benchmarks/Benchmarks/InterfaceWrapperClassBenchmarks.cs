using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;
using System;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class InterfaceWrapperClassBenchmarks
    {
        InterfaceWrapperClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup()
        {
            _instance = new InterfaceWrapperClass
            {
                GuidsMap = CollectionsGenerator.Dictionary<int, Guid>(Lengths, i => new(i, Guid.NewGuid())),
                Ints = CollectionsGenerator.Array(Lengths, i => i),
                Objects = CollectionsGenerator.LinkedList<object>(Lengths, i => i % 2 == 0 ? SmallClassGenerator.Generate() : StandardOnlyGenerator.Generate()),
                StandardOnlyList = CollectionsGenerator.List(Lengths, i => StandardOnlyGenerator.Generate()),
                StructMap = CollectionsGenerator.Dictionary<int, SmallStruct>(Lengths, i => new(i, new(i, (long)i, Guid.NewGuid())))
            };

            ForceCloner.DeepClone(_instance);
            ForceCloner.ShallowClone(_instance);
            SimpleCloner.DeepClone(_instance);
            SimpleCloner.ShallowClone(_instance);
            AutoMapperCloner.ShallowClone(_instance);
        }

        [Benchmark]
        public InterfaceWrapperClass Force_Deep() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public InterfaceWrapperClass Force_Shallow() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public InterfaceWrapperClass AutoMapper_Shallow() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public InterfaceWrapperClass Simple_Deep() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public InterfaceWrapperClass Simple_Shallow() => SimpleCloner.ShallowClone(_instance);
    }
}
