using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class HugeValueStructDeepClone
    {
        HugeValueStruct _instance = HugeValueStructGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            ForceCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public HugeValueStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeValueStruct Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeValueStructShallowClone
    {
        HugeValueStruct _instance = HugeValueStructGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            AutoMapperCloner.ShallowClone(_instance);
            ForceCloner.ShallowClone(_instance);
            SimpleCloner.ShallowClone(_instance);
        }

        [Benchmark]
        public HugeValueStruct AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeValueStruct Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeValueStruct Simple() => SimpleCloner.ShallowClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeValueStructCollectionsClone
    {
        Dictionary<string, HugeValueStruct> _dictionary;
        List<HugeValueStruct> _list;
        HugeValueStruct[] _array;
        HashSet<HugeValueStruct> _hashSet;

        [Params(10, 100)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeValueStructGenerator.Generate()));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeValueStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeValueStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeValueStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeValueStruct> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
