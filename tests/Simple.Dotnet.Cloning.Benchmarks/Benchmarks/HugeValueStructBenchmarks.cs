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
        HugeValueStruct _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeValueStructGenerator.Generate());

        [Benchmark]
        public HugeValueStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeValueStruct Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public HugeValueStruct NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeValueStruct FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeValueStruct DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeValueStructShallowClone
    {
        HugeValueStruct _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeValueStructGenerator.Generate());

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
        PriorityQueue<HugeValueStruct, int> _priorityQueue;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeValueStructGenerator.Generate()));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeValueStructGenerator.Generate()).ToList());
            Warmup.WarmupFor(_priorityQueue = new PriorityQueue<HugeValueStruct, int>(Enumerable.Range(0, Size).Select(i => (HugeValueStructGenerator.Generate(), i))));
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeValueStruct> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeValueStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeValueStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public HugeValueStruct[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeValueStruct[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeValueStruct[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeValueStruct> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeValueStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeValueStruct> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeValueStruct> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeValueStruct> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeValueStruct> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion


        #region PriorityQueue

        [Benchmark]
        public PriorityQueue<HugeValueStruct, int> PriorityQueue_Force() => ForceCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeValueStruct, int> PriorityQueue_Simple() => SimpleCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeValueStruct, int> PriorityQueue_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeValueStruct, int> PriorityQueue_NCloner() => NClonerCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeValueStruct, int> PriorityQueue_DeepCopyCloner() => DeepCopyCloner.DeepClone(_priorityQueue);

        #endregion
    }
}
