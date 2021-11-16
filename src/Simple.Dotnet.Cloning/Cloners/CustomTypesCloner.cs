using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class CustomTypesCloner
    {
        public static class Collections
        {
            static readonly Type Type = typeof(Collections);
            public static readonly MethodInfo LinkedListOpenedMethod = Type.GetMethod(
                nameof(Collections.CloneLinkedList),
                BindingFlags.Static | BindingFlags.Public)!;
            public static readonly MethodInfo DictionaryOpenedMethod = Type.GetMethod(
                nameof(Collections.CloneDictionary),
                BindingFlags.Static | BindingFlags.Public)!;
            public static readonly MethodInfo SortedDictionaryOpenedMethod = Type.GetMethod(
                nameof(Collections.CloneSortedDictionary),
                BindingFlags.Static | BindingFlags.Public)!;

            #if NET6_0_OR_GREATER
            public static readonly MethodInfo PriorityQueueOpenedMethod = Type.GetMethod(
                nameof(Collections.ClonePriorityQueue),
                BindingFlags.Static | BindingFlags.Public)!;
            #endif

            public static LinkedList<T>? CloneLinkedList<T>(LinkedList<T>? linkedList)
            {
                if (linkedList == null) return null;
                if (linkedList.Count == 0) return new();

                var clone = new LinkedList<T>();
                foreach (var element in linkedList) clone.AddLast(RootCloner<T>.DeepClone(element));

                return clone;
            }

            public static Dictionary<TKey, TValue>? CloneDictionary<TKey, TValue>(Dictionary<TKey, TValue>? dictionary)
            {
                if (dictionary == null) return null;
                if (dictionary.Count == 0) return new(dictionary.Comparer);

                var clone = new Dictionary<TKey, TValue>(dictionary.Count, dictionary.Comparer);
                foreach (var element in dictionary) clone.Add(RootCloner<TKey>.DeepClone(element.Key)!, RootCloner<TValue>.DeepClone(element.Value));

                return clone;
            }

            public static SortedDictionary<TKey, TValue>? CloneSortedDictionary<TKey, TValue>(SortedDictionary<TKey, TValue>? dictionary)
            {
                if (dictionary == null) return null;
                if (dictionary.Count == 0) return new(dictionary.Comparer);

                var clone = new SortedDictionary<TKey, TValue>(dictionary.Comparer);
                foreach (var element in dictionary) clone.Add(RootCloner<TKey>.DeepClone(element.Key)!, RootCloner<TValue>.DeepClone(element.Value)!);

                return clone;
            }

            #if NET6_0_OR_GREATER
            public static PriorityQueue<TElement, TPriority>? ClonePriorityQueue<TElement, TPriority>(PriorityQueue<TElement, TPriority>? priorityQueue)
            {
                if (priorityQueue == null) return null;
                if (priorityQueue.Count == 0) return new(priorityQueue.Comparer);

                var clone = new PriorityQueue<TElement, TPriority>(priorityQueue.Count, priorityQueue.Comparer);
                foreach (var (item, priority) in priorityQueue.UnorderedItems) clone.Enqueue(RootCloner<TElement>.DeepClone(item)!, RootCloner<TPriority>.DeepClone(priority)!);

                return clone;
            }
            #endif
        }
        
        public static class Concurrent
        {
            static readonly Type Type = typeof(Concurrent);
            public static readonly MethodInfo BagOpenedMethod = Type.GetMethod(
                nameof(Concurrent.CloneBag),
                BindingFlags.Static | BindingFlags.Public)!;
            public static readonly MethodInfo DictionaryOpenedMethod = Type.GetMethod(
                nameof(Concurrent.CloneDictionary),
                BindingFlags.Static | BindingFlags.Public)!;
            public static readonly MethodInfo QueueOpenedMethod = Type.GetMethod(
                nameof(Concurrent.CloneQueue),
                BindingFlags.Static | BindingFlags.Public)!;
            public static readonly MethodInfo StackOpenedMethod = Type.GetMethod(
                nameof(Concurrent.CloneStack),
                BindingFlags.Static | BindingFlags.Public)!;

            public static ConcurrentBag<T>? CloneBag<T>(ConcurrentBag<T>? bag)
            {
                if (bag == null) return null;
                if (bag.Count == 0) return new();

                var clone = new ConcurrentBag<T>();
                foreach (var item in bag) clone.Add(RootCloner<T>.DeepClone(item));

                return clone;
            }

            public static ConcurrentQueue<T>? CloneQueue<T>(ConcurrentQueue<T>? queue)
            {
                if (queue == null) return null;
                if (queue.Count == 0) return new();

                var clone = new ConcurrentQueue<T>();
                foreach (var item in queue) clone.Enqueue(RootCloner<T>.DeepClone(item));

                return clone;
            }

            public static ConcurrentStack<T>? CloneStack<T>(ConcurrentStack<T>? stack)
            {
                if (stack == null) return null;
                if (stack.Count == 0) return new();

                var clone = new ConcurrentStack<T>();
                var array = ArrayPool<T>.Shared.Rent(stack.Count);
                try
                {
                    var index = stack.Count;
                    foreach (var item in stack) array[--index] = RootCloner<T>.DeepClone(item);
                    
                    clone.PushRange(array, 0, stack.Count);
                }
                finally
                {
                    ArrayPool<T>.Shared.Return(array, true);
                }

                return clone;
            }

            public static ConcurrentDictionary<TKey, TValue>? CloneDictionary<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? dictionary)
            {
                if (dictionary == null) return null;


                #if NET6_0_OR_GREATER
                    if (dictionary.Count == 0) return new(dictionary.Comparer);
                    var clone = new ConcurrentDictionary<TKey, TValue>(dictionary.Comparer);
                #else
                    if (dictionary.Count == 0) return new();
                    var clone = new ConcurrentDictionary<TKey, TValue>();
                #endif

                foreach (var item in dictionary) clone[RootCloner<TKey>.DeepClone(item.Key)] = RootCloner<TValue>.DeepClone(item.Value);

                return clone;
            }
        }
    }
}
