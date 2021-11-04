using System;
using System.Runtime.Serialization;

namespace Simple.Dotnet.Cloning.Benchmarks
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

    [DataContract]
    public sealed class SmallClass : IComparable<SmallClass>
    {
        [DataMember(Order = 0)] public int? Value1 { get; set; }
        [DataMember(Order = 1)] public long Value2 { get; set; }
        [DataMember(Order = 2)] public Guid Value3 { get; set; }

        public int CompareTo(SmallClass other) => Value2.CompareTo(other?.Value2 ?? 0L);
    }
}
