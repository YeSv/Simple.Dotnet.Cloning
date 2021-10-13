using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Dotnet.Cloning
{
    internal static class Types
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
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
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
            typeof(ImmutableArray<>),
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

            // Reflection
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

            // Immutables
            typeof(ImmutableDictionary<,>),
            typeof(ImmutableStack<>),
            typeof(ImmutableSortedSet<>),
            typeof(ImmutableSortedDictionary<,>),
            typeof(ImmutableQueue<>),
            typeof(ImmutableList<>),
            typeof(ImmutableHashSet<>),
        };

        public static HashSet<Type> Collections = new HashSet<Type>
        {
            // System.Collections
            typeof(ArrayList),
            typeof(BitArray),
            typeof(Hashtable),
            typeof(Queue),
            typeof(SortedList),
            typeof(Stack),

            // ObjectModel
            typeof(Collection<>),
            typeof(KeyedCollection<,>),
            typeof(ObservableCollection<>),
            typeof(ReadOnlyCollection<>),
            typeof(ReadOnlyDictionary<,>.KeyCollection),
            typeof(ReadOnlyDictionary<,>.ValueCollection),
            typeof(ReadOnlyDictionary<,>),
            typeof(ReadOnlyObservableCollection<>),

            // Specialized
            typeof(HybridDictionary),
            typeof(ListDictionary),
            typeof(NameObjectCollectionBase.KeysCollection),
            typeof(NameValueCollection),
            typeof(OrderedDictionary),
            typeof(StringCollection),
            typeof(StringDictionary),
            typeof(BitVector32),
            typeof(BitVector32?),

            // Concurrent
            typeof(BlockingCollection<>),
            typeof(ConcurrentBag<>),
            typeof(ConcurrentDictionary<,>),
            typeof(ConcurrentStack<>),

            // Generic
            typeof(Dictionary<,>.KeyCollection),
            typeof(Dictionary<,>.ValueCollection),
            typeof(Dictionary<,>),
            typeof(HashSet<>),
            typeof(LinkedList<>),
            typeof(List<>),
            typeof(Queue<>),
            typeof(SortedDictionary<,>.KeyCollection),
            typeof(SortedDictionary<,>.ValueCollection),
            typeof(SortedDictionary<,>),
            typeof(SortedSet<>),
            typeof(SortedList<,>),
            typeof(Stack<>)
        };
    }
}
