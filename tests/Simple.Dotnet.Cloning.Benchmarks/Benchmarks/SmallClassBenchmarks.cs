using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class SmallClassClone
    {
        SmallClass _instance = new() { Value1 = int.MaxValue, Value2 = long.MaxValue, Value3 = Guid.NewGuid() };

        [GlobalSetup]
        public void Setup()
        {
            AutoMapperCloner.DeepClone(_instance);
            ForceCloner.DeepClone(_instance);
            JsonCloner.DeepClone(_instance);
            MessagePackCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public SmallClass AutoMapper() => AutoMapperCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public SmallClass Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallClassCollectionsClone
    {
        SmallClass _instance = new() { Value1 = int.MaxValue, Value2 = long.MaxValue, Value3 = Guid.NewGuid() };

        Dictionary<string, SmallClass> _dictionary;
        List<SmallClass> _list;
        SmallClass[] _array;
        HashSet<SmallClass> _hashSet;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => _instance));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => _instance).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => _instance).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => _instance).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_AutoMapper() => AutoMapperCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public SmallClass[] Array_AutoMapper() => AutoMapperCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public SmallClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<SmallClass> HashSet_AutoMapper() => AutoMapperCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<SmallClass> List_AutoMapper() => AutoMapperCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallClass> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            AutoMapperCloner.DeepClone(instance);
            ForceCloner.DeepClone(instance);
            JsonCloner.DeepClone(instance);
            MessagePackCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
