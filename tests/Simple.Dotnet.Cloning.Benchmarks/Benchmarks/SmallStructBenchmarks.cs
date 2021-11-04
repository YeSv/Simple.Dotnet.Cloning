using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class SmallStructClone
    {
        SmallStruct _instance = new(int.MaxValue, long.MaxValue, Guid.NewGuid());

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
        public SmallStruct AutoMapper() => AutoMapperCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public SmallStruct Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class SmallStructCollectionsClone
    {
        SmallStruct _instance = new(int.MaxValue, long.MaxValue, Guid.NewGuid());

        Dictionary<string, SmallStruct> _dictionary;
        List<SmallStruct> _list;
        SmallStruct[] _array;
        HashSet<SmallStruct> _hashSet;

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
        public Dictionary<string, SmallStruct> Dictionary_AutoMapper() => AutoMapperCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, SmallStruct> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public SmallStruct[] Array_AutoMapper() => AutoMapperCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public SmallStruct[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<SmallStruct> HashSet_AutoMapper() => AutoMapperCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<SmallStruct> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<SmallStruct> List_AutoMapper() => AutoMapperCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<SmallStruct> List_Simple() => SimpleCloner.DeepClone(_list);

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
