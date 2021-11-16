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
        HugeStruct _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeStructGenerator.Generate());

        [Benchmark]
        public HugeStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeStruct Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public HugeStruct NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeStruct FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeStruct DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeStructShallowClone
    {
        HugeStruct _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeStructGenerator.Generate());

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
        PriorityQueue<HugeStruct, int> _priorityQueue;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeStructGenerator.Generate()));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeStructGenerator.Generate()).ToList());
            Warmup.WarmupFor(_priorityQueue = new PriorityQueue<HugeStruct, int>(Enumerable.Range(0, Size).Select(i => (HugeStructGenerator.Generate(), i))));
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeStruct> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public HugeStruct[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeStruct[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeStruct[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeStruct> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeStruct> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeStruct> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeStruct> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeStruct> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeStruct> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeStruct> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion


        #region PriorityQueue

        [Benchmark]
        public PriorityQueue<HugeStruct, int> PriorityQueue_Force() => ForceCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeStruct, int> PriorityQueue_Simple() => SimpleCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeStruct, int> PriorityQueue_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeStruct, int> PriorityQueue_NCloner() => NClonerCloner.DeepClone(_priorityQueue);

        [Benchmark]
        public PriorityQueue<HugeStruct, int> PriorityQueue_DeepCopyCloner() => DeepCopyCloner.DeepClone(_priorityQueue);

        #endregion
    }
}
