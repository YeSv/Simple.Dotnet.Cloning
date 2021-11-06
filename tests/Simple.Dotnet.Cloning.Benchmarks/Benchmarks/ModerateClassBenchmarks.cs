using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class ModerateClassClone
    {
        ModerateClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup()
        {
            _instance = ModerateClassGenerator.Generate(Lengths, Lengths);
            AutoMapperCloner.DeepClone(_instance);
            ForceCloner.DeepClone(_instance);
            SimpleCloner.DeepClone(_instance);
        }

        [Benchmark]
        public ModerateClass AutoMapper() => AutoMapperCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass Simple() => SimpleCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class ModerateClassCollectionsClone
    {
        Dictionary<string, ModerateClass> _dictionary;
        List<ModerateClass> _list;
        ModerateClass[] _array;
        HashSet<ModerateClass> _hashSet;

        [Params(10)]
        public int Size;

        [GlobalSetup]
        public void Setup()
        {
            WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => ModerateClassGenerator.Generate(100, 100)));
            WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(100, 100)).ToHashSet());
            WarmupFor(_array = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(100, 100)).ToArray());
            WarmupFor(_list = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(100, 100)).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_AutoMapper() => AutoMapperCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public ModerateClass[] Array_AutoMapper() => AutoMapperCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<ModerateClass> HashSet_AutoMapper() => AutoMapperCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<ModerateClass> List_AutoMapper() => AutoMapperCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_Simple() => SimpleCloner.DeepClone(_list);

        #endregion

        static void WarmupFor<T>(T instance)
        {
            AutoMapperCloner.DeepClone(instance);
            ForceCloner.DeepClone(instance);
            SimpleCloner.DeepClone(instance);
        }
    }
}
