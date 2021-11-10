using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class SmallStructDeepClone
    {
        SmallStruct _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = new(int.MaxValue, long.MaxValue, Guid.NewGuid()));

        [Benchmark]
        public SmallStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallStructShallowClone
    {
        SmallStruct _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = new(int.MaxValue, long.MaxValue, Guid.NewGuid()));

        [Benchmark]
        public SmallStruct AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public SmallStruct Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public SmallStruct Simple() => SimpleCloner.ShallowClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallStructCollectionsClone
    {
        Dictionary<string, SmallStruct> _dictionary;
        List<SmallStruct> _list;
        SmallStruct[] _array;
        HashSet<SmallStruct> _hashSet;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => new SmallStruct(int.MaxValue, long.MaxValue, Guid.NewGuid())));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => new SmallStruct(int.MaxValue, long.MaxValue, Guid.NewGuid())).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => new SmallStruct(int.MaxValue, long.MaxValue, Guid.NewGuid())).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => new SmallStruct(int.MaxValue, long.MaxValue, Guid.NewGuid())).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public SmallStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<SmallStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<SmallStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion
    }
}
