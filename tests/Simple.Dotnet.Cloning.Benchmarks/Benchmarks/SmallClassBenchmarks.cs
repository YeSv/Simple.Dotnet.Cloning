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
        SmallClass _instance = SmallClassGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            ForceCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public SmallClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass Simple() => SimpleCloner.DeepClone(_instance);
    }


    [MemoryDiagnoser]
    public class SmallClassShallowClone
    {
        SmallClass _instance = SmallClassGenerator.Generate();

        [GlobalSetup]
        public void Setup()
        {
            AutoMapperCloner.ShallowClone(_instance);
            ForceCloner.ShallowClone(_instance);
            SimpleCloner.ShallowClone(_instance);
        }

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
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => SmallClassGenerator.Generate()));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => SmallClassGenerator.Generate()).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public SmallClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<SmallClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<SmallClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
