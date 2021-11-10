using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Simple.Dotnet.Cloning.Tests.Common;

namespace Simple.Dotnet.Cloning.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class ModerateClassDeepClone
    {
        ModerateClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = ModerateClassGenerator.Generate(Lengths, Lengths));

        [Benchmark]
        public ModerateClass Force() => ForceCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass Simple() => SimpleCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass NCloner() => NClonerCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass FastDeepCloner() => FastDeepClonerCloner.DeepClone(_instance);

        [Benchmark]
        public ModerateClass DeepCopy() => DeepCopyCloner.DeepClone(_instance);
    }

    [MemoryDiagnoser]
    public class ModerateClassShallowClone
    {
        ModerateClass _instance;

        [Params(10, 100)]
        public int Lengths;

        [GlobalSetup]
        public void Setup() => Warmup.WarmupFor(_instance = ModerateClassGenerator.Generate(Lengths, Lengths));

        [Benchmark]
        public ModerateClass AutoMapper() => AutoMapperCloner.ShallowClone(_instance);

        [Benchmark]
        public ModerateClass Force() => ForceCloner.ShallowClone(_instance);

        [Benchmark]
        public ModerateClass Simple() => SimpleCloner.ShallowClone(_instance);
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
            Warmup.WarmupFor(_dictionary = Enumerable.Range(0, Size).ToDictionary(s => s.ToString(), s => ModerateClassGenerator.Generate(10, 10)));
            Warmup.WarmupFor(_hashSet = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(10, 10)).ToHashSet());
            Warmup.WarmupFor(_array = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(10, 10)).ToArray());
            Warmup.WarmupFor(_list = Enumerable.Range(0, Size).Select(i => ModerateClassGenerator.Generate(10, 10)).ToList());
        }

        #region Dictionary

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_Force() => ForceCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_Simple() => SimpleCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_NCloner() => NClonerCloner.DeepClone(_dictionary);

        [Benchmark]
        public Dictionary<string, ModerateClass> Dictionary_DeepCopyCloner() => DeepCopyCloner.DeepClone(_dictionary);

        #endregion

        #region Array

        [Benchmark]
        public ModerateClass[] Array_Force() => ForceCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_Simple() => SimpleCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_NCloner() => NClonerCloner.DeepClone(_array);

        [Benchmark]
        public ModerateClass[] Array_DeepCopyCloner() => DeepCopyCloner.DeepClone(_array);

        #endregion

        #region HashSet

        [Benchmark]
        public HashSet<ModerateClass> HashSet_Force() => ForceCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_Simple() => SimpleCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_NCloner() => NClonerCloner.DeepClone(_hashSet);

        [Benchmark]
        public HashSet<ModerateClass> HashSet_DeepCopyCloner() => DeepCopyCloner.DeepClone(_hashSet);

        #endregion

        #region List

        [Benchmark]
        public List<ModerateClass> List_Force() => ForceCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_Simple() => SimpleCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_FastDeepCloner() => FastDeepClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_NCloner() => NClonerCloner.DeepClone(_list);

        [Benchmark]
        public List<ModerateClass> List_DeepCopyCloner() => DeepCopyCloner.DeepClone(_list);

        #endregion
    }
}
