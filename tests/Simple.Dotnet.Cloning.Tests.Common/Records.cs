using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

namespace Simple.Dotnet.Cloning.Tests.Common 
{
    [DataContract]
    public readonly struct SmallStruct : IComparable<SmallStruct>
    {
        [DataMember(Order = 0)] public int? Value1 { get; }
        [DataMember(Order = 1)] public long Value2 { get; }
        [DataMember(Order = 2)] public Guid Value3 { get; }

        public SmallStruct(int? value1, long value2, Guid value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public int CompareTo(SmallStruct other) => Value2.CompareTo(other.Value2);
    }

    public abstract class AbstractClass { }

    [DataContract]
    public sealed class SmallClass : AbstractClass, IComparable<SmallClass>
    {
        [DataMember(Order = 0)] public int? Value1 { get; set; }
        [DataMember(Order = 1)] public long Value2 { get; set; }
        [DataMember(Order = 2)] public Guid Value3 { get; set; }

        public int CompareTo(SmallClass other) => Value2.CompareTo(other?.Value2 ?? 0L);
    }

    public enum IntEnum
    {
        Value1,
        Value2,
        Value3,
        Value4,
        Value5,
        Value6,
        Value7,
        Value8,
        Value9,
        Value10
    }

    public enum ByteEnum : byte
    {
        Value1,
        Value2,
        Value3,
        Value4,
        Value5,
        Value6,
        Value7,
        Value8,
        Value9,
        Value10
    }

    public enum LongEnum : long
    {
        Value1,
        Value2,
        Value3,
        Value4,
        Value5,
        Value6,
        Value7,
        Value8,
        Value9,
        Value10
    }

    public class StringsSmallClass : AbstractClass
    {
        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }
    }

    public class HugeClass : AbstractClass
    {
        public int Value1 { get; set; }

        public double Value2 { get; set; }

        public double Value3 { get; set; }

        public string Value4 { get; set; }

        public Guid Value5 { get; set; }

        public float Value6 { get; set; }

        public Guid? Value7 { get; set; }

        public double Value8 { get; set; }

        public Guid Value9 { get; set; }

        public DateTime Value10 { get; set; }

        public int Value11 { get; set; }

        public int Value12 { get; set; }

        public int Value13 { get; set; }

        public DateTime Value14 { get; set; }

        public string Value15 { get; set; }

        public int Value16 { get; set; }

        public StandardOnlyClass Value17 { get; set; }

        public long Value18 { get; set; }

        public long Value19 { get; set; }

        public SmallClass Value20 { get; set; }

        public LongEnum Value21 { get; set; }

        public bool Value26 { get; set; } = false;

        public DateTime? Value27 { get; set; } = null;

        public DateTime Value28 { get; set; }

        public StandardOnlyClass[] Value29 { get; set; }

        public ExpandoObject Value30 { get; set; }

        public int Value31 { get; set; }

        public int Value32 { get; set; }

        public string Value33 { get; set; }

        public SmallClass[] Value34 { get; set; }

        public int? Value35 { get; set; }

        public HashSet<SmallClass> Value36 { get; set; }

        public StringsSmallClass[] Value37 { get; set; }

        public int? Value38 { get; set; }

        public int? Value39 { get; set; }

        public SmallStruct Value40 { get; set; }
        
        public DateTime? Value41 { get; set; }

        public int? Value42 { get; set; }

        public long? Value43 { get; set; }

        public int Value44 { get; set; }

        public bool Value45 { get; set; }

        public int? Value46 { get; set; }

        public int Value47 { get; set; }

        public bool Value48 { get; set; }

        public Stack<StandardOnlyClass> Value49 { get; set; }

        public LinkedList<StandardOnlyClass> Value50 { get; set; }

        public string Value51 { get; set; }

        public int Value52 { get; set; }

        public int? Value53 { get; set; }

        public ByteEnum Value54 { get; set; }

        public ModerateClass[] Value55 { get; set; }

        public Dictionary<string, ModerateClass> Value56 { get; set; }

        public double Value57 { get; set; }

        public int Value58 { get; set; }

        public int Value59 { get; set; }

        public IntEnum Value60 { get; set; }

        public int Value70 { get; set; }

        public LongEnum? Value71 { get; set; }

        public long Value72 { get; set; }

        public int Value73 { get; set; }

        public int Value74 { get; set; }

        public SortedDictionary<string, StringsSmallClass> Value75 { get; set; }

        public int Value76 { get; set; }

        public DateTime Value77 { get; set; }

        public DateTime Value78 { get; set; }

        public long Value79 { get; set; } = 0L;

        public List<ModerateClass> Value80 { get; set; }

        public List<SmallClass> Value81 { get; set; }

        public long Value82 { get; set; }

        public decimal Value83 { get; set; }

        public long Value84 { get; set; }

        public decimal Value85 { get; set; }

        public bool? Value86 { get; set; }

        public DateTime Value87 { get; set; }

        public bool Value88 { get; set; }

        public bool Value89 { get; set; }

        public long? Value90 { get; set; }

        public decimal? Value91 { get; set; }

        public bool Value92 { get; set; }

        public long? Value93 { get; set; }

        public decimal? Value94 { get; set; }

        public bool? Value95 { get; set; }

        public int Value96 { get; set; }

        public int Value97 { get; set; }

        public int Value98 { get; set; }

        public ModerateClass Value99 { get; set; }

        public string Value100 { get; set; }
    }

    public class StandardOnlyClass : AbstractClass 
    {
        public int Value1 { get; set; }

        public long Value2 { get; set; }

        public double Value3 { get; set; }

        public double Value4 { get; set; }
        public double Value5 { get; set; }

        public double Value6 { get; set; }

        public bool Value7 { get; set; }

        public int[] Value8 { get; set; }

        public int Value9 { get; set; }

        public int Value10 { get; set; }

        public bool Value11 { get; set; }
    }

    public class ModerateClass : AbstractClass
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public int Value3 { get; set; }

        public int Value4 { get; set; }

        public int Value5 { get; set; }

        public bool Value6 { get; set; }

        public int Value7 { get; set; }

        public double Value8 { get; set; }

        public int Value9 { get; set; }

        public StandardOnlyClass[] Value10 { get; set; }

        public int Value11 { get; set; }

        public bool Value12 { get; set; }

        public int? Value13 { get; set; }

        public bool Value14 { get; set; }

        public int Value15 { get; set; }

        public long Value16 { get; set; }

        public SortedSet<decimal> Value17 { get; set; }

        public long Value18 { get; set; }

        public decimal Value19 { get; set; }

        public int? Value20 { get; set; }

        public int? Value21 { get; set; }

        public long? Value22 { get; set; }

        public decimal? Value23 { get; set; }

        public LinkedList<long> Value24 { get; set; }

        public decimal Value25 { get; set; }

        public long Value26 { get; set; }

        public decimal Value27 { get; set; }

        public long Value28 { get; set; }

        public bool Value29 { get; set; }

        public decimal? Value30 { get; set; }

        public decimal? Value31 { get; set; }

        public long? Value32 { get; set; }

        public decimal? Value33 { get; set; }

        public long? Value34 { get; set; }

        public decimal? Value35 { get; set; }

        public long? Value36 { get; set; }

        public decimal? Value37 { get; set; }

        public double? Value38 { get; set; }

        public HashSet<long> Value39 { get; set; }

        public bool Value40 { get; set; }

        public bool Value41 { get; set; }

        public long? Value42 { get; set; }

        public int Value43 { get; set; }

        public int? Value44 { get; set; }

        public string Value45 { get; set; }

        public DateTime? value46 { get; set; }

        public decimal? Value47 { get; set; }

        public decimal?[] Value48 { get; set; }

        public bool Value49 { get; set; }

        public StringsSmallClass[] Value50 { get; set; }
    }

    public class InterfaceClass<T>
    {
        public IEnumerable<T> Values { get; set; }
    }

    public class InterfaceWrapperClass
    {
        public IEnumerable<int> Ints { get; set; }
        public IEnumerable<KeyValuePair<int, Guid>> GuidsMap { get; set; }
        public IEnumerable<object> Objects { get; set; }
        public IReadOnlyList<StandardOnlyClass> StandardOnlyList { get; set; }
        public IDictionary<int, SmallStruct> StructMap { get; set; }
    }
}
