using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class HugeStructDeepClone
    {
        HugeStruct _instance = HugeStructGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            ForceCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public HugeStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeStruct Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeStructShallowClone
    {
        HugeStruct _instance = HugeStructGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            AutoMapperCloner.ShallowClone(_instance);
            ForceCloner.ShallowClone(_instance);
            SimpleCloner.ShallowClone(_instance);
        }

        [Benchmark]
        public HugeStruct AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeStruct Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeStruct Simple() => SimpleCloner.ShallowClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeStructCollectionsClone
    {
        Dictionary<string, HugeStruct> _dictionary;
        List<HugeStruct> _list;
        HugeStruct[] _array;
        HashSet<HugeStruct> _hashSet;

        [Params(10, 100)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeStructGenerator.Generate()));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeStruct> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
