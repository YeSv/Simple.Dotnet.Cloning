using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static Simple.Dotnet.Cloning.Cloners.CustomTypesCloner;

namespace Simple.Dotnet.Cloning
{
    public static class CloningTypes
    {
        public static HashSet<Type> SafeToCopy = new HashSet<Type>
        {
            // Common:
            typeof(char),
            typeof(char?),
            typeof(byte),
            typeof(byte?),
            typeof(bool),
            typeof(bool?),
            typeof(sbyte),
            typeof(sbyte?),
            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),
            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),
            typeof(nint),
            typeof(nint?),
            typeof(nuint),
            typeof(nuint?),
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?),
            typeof(IntPtr),
            typeof(IntPtr?),
            typeof(UIntPtr),
            typeof(UIntPtr?),

            // Useful structs:
            typeof(Guid),
            typeof(Guid?),
            typeof(TimeSpan),
            typeof(TimeSpan?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(CancellationToken),
            typeof(CancellationToken?),
            typeof(Memory<>),
            typeof(ReadOnlyMemory<>),
            typeof(Span<>),
            typeof(ReadOnlySpan<>),
            typeof(ArraySegment<>),
            typeof(CancellationTokenRegistration),
            typeof(CancellationTokenRegistration?),

            // Classes:
            typeof(string), // immutable
            typeof(TimeZoneInfo),
            typeof(Timer),
            typeof(System.Timers.Timer),

            // Threading
            typeof(Task),
            typeof(Task<>),
            typeof(Thread),
            typeof(TaskCompletionSource<>),
            typeof(CancellationTokenSource),
            typeof(Lazy<>),
            typeof(AsyncLocal<>),
            typeof(ThreadLocal<>),
            typeof(AutoResetEvent),
            typeof(Barrier),
            typeof(CountdownEvent),
            typeof(ManualResetEvent),
            typeof(ManualResetEventSlim),
            typeof(Mutex),
            typeof(Overlapped),
            typeof(ReaderWriterLock),
            typeof(ReaderWriterLockSlim),
            typeof(Semaphore),
            typeof(SemaphoreSlim),
            typeof(WaitHandle),
            typeof(LockCookie),
            typeof(LockCookie?),
            typeof(SpinLock),
            typeof(SpinLock?),
            typeof(SpinWait),
            typeof(SpinWait?),
            typeof(WaitHandle),
            typeof(RegisteredWaitHandle),
            typeof(EventWaitHandle),

            // Reflection
            typeof(Binder),
            typeof(Assembly),
            typeof(Type),
            typeof(TypeInfo),
            typeof(MarshalByRefObject),
            typeof(MemberInfo),
            typeof(ConstructorInfo),
            typeof(PropertyInfo),
            typeof(FieldInfo),
            typeof(LocalVariableInfo),
            typeof(ParameterInfo),
            typeof(EventInfo),
            typeof(ManifestResourceInfo),
            typeof(ILGenerator),

            // Collections
            typeof(LinkedListNode<>), // Cloning will break collection
            typeof(Dictionary<,>.ValueCollection),
            typeof(Dictionary<,>.KeyCollection),
            typeof(SortedDictionary<,>.ValueCollection),
            typeof(SortedDictionary<,>.KeyCollection),

            // Serialization
            typeof(SerializationInfo),
            typeof(SerializationBinder),
            typeof(SerializationObjectManager),
            typeof(SerializationInfoEnumerator),
            typeof(XmlObjectSerializer),
            typeof(XmlSerializableServices),

            // Special
            typeof(DBNull),
            typeof(ApplicationId),
            typeof(MarshalByRefObject),

            // Interfaces
            typeof(IComparer<>),
            typeof(ICustomAttributeProvider),
            typeof(IEqualityComparer<>),
            typeof(IFormatter),
            typeof(IFormatterConverter),
            typeof(IFormatProvider),
            typeof(IServiceProvider),


            // Runtime
            typeof(MemoryFailPoint),
            typeof(GCHandle),
            typeof(GCHandle?),
            typeof(HandleRef),
            typeof(HandleRef?),
            typeof(OSPlatform),
            typeof(OSPlatform?),
            typeof(ICustomFactory),
            typeof(ICustomMarshaler),

            // Netstandard2.1 types 
            #if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            typeof(Index),
            typeof(Index?),
            typeof(Range),
            typeof(Range?),
            #endif

            // Net 6 +
            #if NET6_0_OR_GREATER
            typeof(DateOnly),
            typeof(DateOnly?),
            typeof(TimeOnly),
            typeof(TimeOnly?),
            typeof(PriorityQueue<,>.UnorderedItemsCollection), // Will break the collection
            #endif
        };

        public static readonly Dictionary<Type, Func<Type[], MethodInfo>> Custom = new Dictionary<Type, Func<Type[], MethodInfo>>
        {
            [typeof(LinkedList<>)] = t => Collections.LinkedListOpenedMethod.MakeGenericMethod(t),
            [typeof(Dictionary<,>)] = t => Collections.DictionaryOpenedMethod.MakeGenericMethod(t),
            [typeof(SortedDictionary<,>)] = t => Collections.SortedDictionaryOpenedMethod.MakeGenericMethod(t),

#if NET6_0_OR_GREATER
            [typeof(PriorityQueue<,>)] = t => Collections.PriorityQueueOpenedMethod.MakeGenericMethod(t),
#endif

            [typeof(ConcurrentBag<>)] = t => Concurrent.BagOpenedMethod.MakeGenericMethod(t),
            [typeof(ConcurrentStack<>)] = t => Concurrent.StackOpenedMethod.MakeGenericMethod(t),
            [typeof(ConcurrentQueue<>)] = t => Concurrent.QueueOpenedMethod.MakeGenericMethod(t),
            [typeof(ConcurrentDictionary<,>)] = t => Concurrent.DictionaryOpenedMethod.MakeGenericMethod(t),

            [typeof(ExpandoObject)] = t => Dynamic.ExpandoObjectMethod
        };
    }
}
