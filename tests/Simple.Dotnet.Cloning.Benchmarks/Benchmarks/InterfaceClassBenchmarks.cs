using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;
using System;
using System.Collections.Generic;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class InterfaceClassBenchmarks
    {
        InterfaceClass<Guid> _guidsArray;
        InterfaceClass<Guid> _guidsHashSet;

        InterfaceClass<SmallClass> _classArray;
        InterfaceClass<SmallClass> _classHashSet;

        InterfaceClass<KeyValuePair<Guid, SmallClass>> _classDictionary;
        InterfaceClass<KeyValuePair<Guid, SmallStruct>> _structDictionary;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup()
        {
            WarmupFor(_guidsArray = new() { Values = CollectionsGenerator.Array(Lengths, _ => Guid.NewGuid()) });
            WarmupFor(_guidsHashSet = new() { Values = CollectionsGenerator.HashSet(Lengths, _ => Guid.NewGuid()) });

            WarmupFor(_classArray = new() { Values = CollectionsGenerator.Array(Lengths, _ =>  SmallClassGenerator.Generate()) });
            WarmupFor(_classHashSet = new() { Values = CollectionsGenerator.HashSet(Lengths, _ => SmallClassGenerator.Generate()) });

            WarmupFor(_classDictionary = new() { Values = CollectionsGenerator.Dictionary<Guid, SmallClass>(Lengths, _ => new(Guid.NewGuid(), SmallClassGenerator.Generate())) });
            WarmupFor(_structDictionary = new() { Values = CollectionsGenerator.Dictionary<Guid, SmallStruct>(Lengths, _ => new(Guid.NewGuid(), new(int.MaxValue, long.MaxValue, Guid.NewGuid()))) });
        }

        [Benchmark]
        public InterfaceClass<Guid> Force_GuidsArray() => ForceCloner.DeepClone(_guidsArray);

        [Benchmark]
        public InterfaceClass<Guid> Simple_GuidsArray() => SimpleCloner.DeepClone(_guidsArray);

        [Benchmark]
        public InterfaceClass<Guid> Force_GuidsHashSet() => ForceCloner.DeepClone(_guidsHashSet);

        [Benchmark]
        public InterfaceClass<Guid> Simple_GuidsHashSet() => SimpleCloner.DeepClone(_guidsHashSet);

        [Benchmark]
        public InterfaceClass<SmallClass> Force_SmallClassArray() => ForceCloner.DeepClone(_classArray);

        [Benchmark]
        public InterfaceClass<SmallClass> Simple_SmallClassArray() => SimpleCloner.DeepClone(_classArray);

        [Benchmark]
        public InterfaceClass<SmallClass> Force_SmallClassHashSet() => ForceCloner.DeepClone(_classHashSet);

        [Benchmark]
        public InterfaceClass<SmallClass> Simple_SmallClassHashSet() => SimpleCloner.DeepClone(_classHashSet);

        [Benchmark]
        public InterfaceClass<KeyValuePair<Guid, SmallClass>> Force_SmallClassDictionary() => ForceCloner.DeepClone(_classDictionary);

        [Benchmark]
        public InterfaceClass<KeyValuePair<Guid, SmallClass>> Simple_SmallClassDictionary() => SimpleCloner.DeepClone(_classDictionary);

        [Benchmark]
        public InterfaceClass<KeyValuePair<Guid, SmallStruct>> Force_SmallStructDictionary() => ForceCloner.DeepClone(_structDictionary);

        [Benchmark]
        public InterfaceClass<KeyValuePair<Guid, SmallStruct>> Simple_SmallStructDictionary() => SimpleCloner.DeepClone(_structDictionary);


        static void WarmupFor<T>(T instance)
        {
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
