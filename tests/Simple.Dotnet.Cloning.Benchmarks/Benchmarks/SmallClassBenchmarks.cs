using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class SmallClassDeepClone
    {
        SmallClass _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = SmallClassGenerator.Generate());

        [Benchmark]
        public SmallClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallClassShallowClone
    {
        SmallClass _instance;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = SmallClassGenerator.Generate());

        [Benchmark]
        public SmallClass AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public SmallClass Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public SmallClass Simple() => SimpleCloner.ShallowClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallClassCollectionsClone
    {
        Dictionary<string, SmallClass> _dictionary;
        List<SmallClass> _list;
        SmallClass[] _array;
        HashSet<SmallClass> _hashSet;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => SmallClassGenerator.Generate()));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public SmallClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<SmallClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<SmallClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion
    }
}
