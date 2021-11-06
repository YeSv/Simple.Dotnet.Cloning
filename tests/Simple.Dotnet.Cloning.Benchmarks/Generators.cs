using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Dotnet.Cloning.Benchmarks
{
    public static class CollectionsGenerator
    {
        public static T[] Array<T>(int len, Func<int, T> gen) => Enumerable.Range(0, len).Select(v => gen(v)).ToArray();

        public static HashSet<T> HashSet<T>(int len, Func<int, T> gen) => Enumerable.Range(0, len).Select(v => gen(v)).ToHashSet();

        public static SortedSet<T> SortedSet<T>(int len, Func<int, T> gen) => new SortedSet<T>(Enumerable.Range(0, len).Select(v => gen(v)));

        public static List<T> List<T>(int len, Func<int, T> gen) => Enumerable.Range(0, len).Select(v => gen(v)).ToList();

        public static LinkedList<T> LinkedList<T>(int len, Func<int, T> gen) => new (Enumerable.Range(0, len).Select(v => gen(v)));

        public static Stack<T> Stack<T>(int len, Func<int, T> gen) => new (Enumerable.Range(0, len).Select(v => gen(v)));

        public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>(int len, Func<int, KeyValuePair<TKey, TValue>> gen) => new (Enumerable.Range(0, len).Select(v => gen(v)));

        public static SortedDictionary<TKey, TValue> SortedDictionary<TKey, TValue>(int len, Func<int, KeyValuePair<TKey, TValue>> gen) => new(Dictionary(len, gen));
    }

    public static class StringsSmallClassGenerator
    {
        public static StringsSmallClass Generate(int len) => new Faker<StringsSmallClass>()
            .RuleForType(typeof(string), g => g.Lorem.Letter(len)).Generate();
    }

    public static class StandardOnlyGenerator
    {
        static readonly Faker<StandardOnlyClass> _faker = new ();

        public static StandardOnlyClass Generate() => _faker.Generate();
    }

    public static class SmallClassGenerator
    {
        static readonly Faker<SmallClass> _faker = new ();

        public static SmallClass Generate() => _faker.Generate();
    }

    public static class ModerateClassGenerator
    {
        public static ModerateClass Generate(int collectionsLength, int stringsLength) => new Faker<ModerateClass>()
            .RuleForType(typeof(SmallClass), _ => SmallClassGenerator.Generate())
            .RuleForType(typeof(StandardOnlyClass), _ => StandardOnlyGenerator.Generate())
            .RuleForType(typeof(StringsSmallClass), _ => StringsSmallClassGenerator.Generate(stringsLength))
            .RuleForType(typeof(string), f => f.Lorem.Letter(stringsLength))
            .RuleFor(c => c.Value10, g => CollectionsGenerator.Array(collectionsLength, i => StandardOnlyGenerator.Generate()))
            .RuleFor(c => c.Value17, g => CollectionsGenerator.SortedSet(collectionsLength, i => (decimal)i))
            .RuleFor(c => c.Value24, g => CollectionsGenerator.LinkedList(collectionsLength, i => (long)i))
            .RuleFor(c => c.Value39, g => CollectionsGenerator.HashSet(collectionsLength, i => (long)i))
            .RuleFor(c => c.Value50, g => CollectionsGenerator.Array(collectionsLength, i => StringsSmallClassGenerator.Generate(stringsLength)))
            .Generate();
    }

    public static class HugeClassGenerator
    {
        public static HugeClass Generate(int collectionsLength, int stringsLength) => new Faker<HugeClass>()
            .RuleForType(typeof(SmallClass), _ => SmallClassGenerator.Generate())
            .RuleForType(typeof(StandardOnlyClass), _ => StandardOnlyGenerator.Generate())
            .RuleForType(typeof(StringsSmallClass), _ => StringsSmallClassGenerator.Generate(stringsLength))
            .RuleForType(typeof(ModerateClass), _ => ModerateClassGenerator.Generate(collectionsLength, stringsLength))
            .RuleForType(typeof(string), f => f.Lorem.Letter(stringsLength))
            .RuleFor(c => c.Value29, g => CollectionsGenerator.Array(collectionsLength, i => StandardOnlyGenerator.Generate()))
            .RuleFor(c => c.Value34, g => CollectionsGenerator.Array(collectionsLength, i => SmallClassGenerator.Generate()))
            .RuleFor(c => c.Value36, g => CollectionsGenerator.HashSet(collectionsLength, i => SmallClassGenerator.Generate()))
            .RuleFor(c => c.Value37, g => CollectionsGenerator.Array(collectionsLength, i => StringsSmallClassGenerator.Generate(stringsLength)))
            .RuleFor(c => c.Value49, g => CollectionsGenerator.Stack(collectionsLength, i => StandardOnlyGenerator.Generate()))
            .RuleFor(c => c.Value50, g => CollectionsGenerator.LinkedList(collectionsLength, i => StandardOnlyGenerator.Generate()))
            .RuleFor(c => c.Value55, g => CollectionsGenerator.Array(collectionsLength, i => ModerateClassGenerator.Generate(collectionsLength, stringsLength)))
            .RuleFor(c => c.Value56, g => CollectionsGenerator.Dictionary<string, ModerateClass>(collectionsLength, i => new(i.ToString(), ModerateClassGenerator.Generate(collectionsLength, stringsLength))))
            .RuleFor(c => c.Value75, g => CollectionsGenerator.SortedDictionary<string, StringsSmallClass>(collectionsLength, i => new(i.ToString(), StringsSmallClassGenerator.Generate(stringsLength))))
            .RuleFor(c => c.Value80, g => CollectionsGenerator.List(collectionsLength, i => ModerateClassGenerator.Generate(collectionsLength, stringsLength)))
            .RuleFor(c => c.Value81, g => CollectionsGenerator.List(collectionsLength, i => SmallClassGenerator.Generate()));

    }
}
