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
        public void Setup()
        {
            _instance = HugeClassGenerator.Generate(Lengths, Lengths);
            ForceCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public HugeClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public HugeClass Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class HugeClassShallowClone
    {
        HugeClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup()
        {
            _instance = HugeClassGenerator.Generate(Lengths, Lengths);
            ForceCloner.ShallowClone(_instance);
            SimpleCloner.ShallowClone(_instance);
            AutoMapperCloner.ShallowClone(_instance);
        }


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
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => HugeClassGenerator.Generate(10, 10)));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => HugeClassGenerator.Generate(10, 10)).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, HugeClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public HugeClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public HugeClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<HugeClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<HugeClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<HugeClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<HugeClass> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
