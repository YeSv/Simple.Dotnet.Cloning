using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class HugeClassDeepClone
    {
        HugeClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeClassGenerator.Generate(Lengths, Lengths));

        [Benchmark]
        public HugeClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeClass Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public HugeClass NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeClass FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public HugeClass DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeClassShallowClone
    {
        HugeClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = HugeClassGenerator.Generate(Lengths, Lengths));

        [Benchmark]
        public HugeClass AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeClass Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public HugeClass Simple() => SimpleCloner.ShallowClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeClassCollectionsClone
    {
        Dictionary<string, HugeClass> _dictionary;
        List<HugeClass> _list;
        HugeClass[] _array;
        HashSet<HugeClass> _hashSet;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeClassGenerator.Generate(10, 10)));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public HugeClass[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeClass[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public HugeClass[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeClass> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeClass> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeClass> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeClass> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeClass> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeClass> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeClass> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion
    }
}
