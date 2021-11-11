## NuGet
[Simple.Dotnet.Cloning (yet to be created)](https://www.nuget.org/packages/Simple.Dotnet.Cloning/)

## Table of contents
1. [Motivation](#1-motivation)
2. [API](#2-api)
3. [How it works](#3-how-it-works)
4. [Extending](#4-extending)
5. [Benchmarks](#5-benchmarks)
6. [Considerations](#6-cons)
7. [Suggestions](#7-suggestions)

# 1. Motivation

The motivation behind this project is to create a cloner that:

1. Performant
2. Does not allocate some "garbage" while cloning
3. Simple in use

There is an already cool library that does the same - [DeepCloner](https://github.com/force-net/DeepCloner). Actually, this project is inspired by a `DeepCloner` library because I use it a lot so consider `SimpleCloner` as a faster implementation that allocates less but without some features described later. The main drawback of `DeepCloner` is that it makes lots of allocations to track references but you might want to clone your class or struct with `int`s or other classes that do not contain recursive references, this library is made for such situations it does not have this feature and thus does not create additional allocations to maintain itself - in the end - `Why should you pay for something you don't need?`, unfortunatelly, this means that it `can't` clone `self-referencing` or `recursive` types (for now at least) but it allows you to implement your custom cloner for such type if required and also provides cloners for standard types like `Dictionary<TKey, TValue>` and `LinkedList<T>` that have recursive references, you can always override those implementations if needed - check [extending](#4-extending) section below.

# 2. API

Well, API is the simplest as possible, all you need is to add `using  Simple.Dotnet.Cloning` in your cs file and use one of the extensions below:

``` csharp

// .NET 5.0 Program.cs without Main

using System;
using Simple.Dotnet.Cloning;

var quote = new Quote 
{
    Text = "Hello There!",
    Person = "Obi-Wan Kenobi"
};

// Shallow cloning
var shallowClone = quote.ShallowClone(); // Extension
// OR
shallowClone = Cloner.ShallowClone(quote); // The same but using Cloner class

// Deep cloning
var deepClone = quote.DeepClone(); // Extension
// OR
deepClone = Cloner.DeepClone(quote); // The same but using Cloner class


public class Quote {
    public string Text { get; set; }
    public string Person { get; set; }
}


```

As you can see API is pretty straighforward all you need to do is to use `.ShallowClone()` or `.DeepClone()`. `DeepClone` will clone all the reference types in the whole references tree for both `struct`s and `class`es, while `ShallowClone` will only create a new instance and copy all fields to a new instance of `class` or `struct`.

# 3. How it works

When you first use `ShallowClone` or `DeepClone` a new cloner is generated using `ILGenerator` and cached so next calls to this methods use already generated code, it's super efficient and library tries to generate the code you might wrote by hand yourself. This happens for each field and field's type one by one until the whole method is generated, there are some assumptions that library takes to be able to clone instance of class or struct as efficient as possible, let's discuss it below:

1. Safe to copy types/immutable types/ignored types (aka: `SafeToCopy`)

    Some types can be copied as is, for example `int`, `int?` or even more complex ones like `string`, you don't need to copy or reallocate the string once more - it is immutable so during deep cloning it's just copied from source object as is. Some types are copied by default like `int`s, `double`s, etc or structs like `DateTime` which can be copied as is.
    The whole list of such types is available [here](https://github.com/YeSv/Simple.Dotnet.Cloning/blob/main/src/Simple.Dotnet.Cloning/CloningTypes.cs#L18). Let's call such types as `SafeToCopy`. Good news, you can modify this list if you want by either adding a new type or removing already existing one.

2. Value types (structs) that have all fields as `SafeToCopy`
    
    Structs that have all fields that are considered as `SafeToCopy` can be cloned as is, for example:

    ``` csharp

    public struct StructExample 
    {
        public int Integer { get; set; } // Can be readonly without setter
        public double Double { get; set; }
        public Guid Guid { get; set; }
        public string String { get; set; }
    }

    var instance = new StructExample(); // Omit field initialization, does not matter for this example

    // We can clone it simply as is
    var clone = instance;

    // This is exactly what library does when generating a cloner for this type of struct

    // NOTE: for ShallowClone() struct is just cloned as above (why would you need to shallow clone a struct using this library ?)

    ```

3. Value types that have some fields that require deep cloning

    In case when struct contains types that are not considered as safe to copy as some of it's fields, we can do something like this:

    ``` csharp

    // Well, now we have a field that should be deep cloned - int[] Ints
    public struct StructExample 
    {
        public int Integer { get; set; } // Can be readonly without setter
        public double Double { get; set; }
        public Guid Guid { get; set; }
        public int[] Ints { get; set; }
    }

     var instance = new StructExample(); // Omit field initialization, does not matter for this example

    // Clone all safe to copy fields
    var clone = instance;

    // Now clone ints array
    clone.Ints = instance.Ints.DeepClone();

    // This is exactly what library generates under the hood
    // NOTE: in case if struct is readonly you can't do such assignment in C#, but in IL you can, also IL code assigns fields using references so structs are not copied multiple times on such assignments

    ```    

4. Reference types with all fields as `SafeToCopy`

    Such cases can be handled using code below:

    ``` csharp

    public class ClassExample 
    {
        public int Integer { get; set; } // Can be readonly without setter
        public double Double { get; set; }
        public Guid Guid { get; set; }
    }

    var instance = new ClassExample(); // Omit field initialization, does not matter for this example

    // Initialize instance of a clone:
    var clone = new ClassExample();

    // Now copy fields one by one:
    clone.Integer = instance.Integer;
    clone.Double = instance.Double;
    clone.Guid = instance.Guid;

    // That's it, the same code is generated using ILGenerator but for properties backing fields are used

    // NOTE: it's okay if class does not have parameterless constructor, instance will still be created as if class has one

    ```    

5. Reference types that have fields that are not `SafeToCopy`

    This time we will also create a clone but for fields that are not `SafeToCopy` - `DeepClone` will be called:

    ``` csharp

    public class ClassExample 
    {
        public int Integer { get; set; } // Can be readonly without setter
        public double Double { get; set; }
        public int[] Ints { get; set; }
    }

    var instance = new ClassExample(); 

    // Initialize instance of a clone:
    var clone = new ClassExample();

    // Now copy fields one by one:
    clone.Integer = instance.Integer;
    clone.Double = instance.Double;

    // Use deep clone on not `SafeToCopy` field:
    clone.Ints = instance.Ints.DeepClone();

    ```

6. Interfaces/Abstract/Object types or classes

    For fields or types like `IEnumerable<T>` or `object` or instances casted to abstract classes we need to know exact type at runtime because the real type can be literally anything, for example:

    ``` csharp

    abstract class Animal {}
    class Cat : Animal {}

    IEnumerable<int> enumerable = new int[] { 1, 2, 3 };
    object obj = "Why am I an object?";
    Animal animal = new Cat();

    // How should we clone such values?
    // We don't know the real type till runtime

    ```

    For such cases we can only clone instances during runtime:
    
    ``` csharp

    // Imagine that process is running
    // To clone animal or enumerable or obj we should get real type:
    var type = enumerable.GetType();

    // Do some magic tricks and clone as :
    var clone = DeepClone(enumerable, type); // Deep clone by specifiing real type (not a real method, just for demonstration purposes)

    // To do operation above - library maintains a dictionary at runtime and generates lambdas per type and caches them.

    ```

    You can check a code for doing something like `DeepClone<T>(T instance, Type type)` here: [ObjectCloner](https://github.com/YeSv/Simple.Dotnet.Cloning/blob/main/src/Simple.Dotnet.Cloning/Cloners/ObjectCloner.cs).

7. Classes that not marked as abstract and not sealed

    What happens when you have class hierarchy but you cast to type that is not marked as abstract:

    ``` csharp

    // Hierarchy
    public class Parent { public int Value { get; set; } }
    public class Child : Parent { public int ChildValue {get; set; } }

    // Cast to parent
    Parent instance = new Child();

    // What will happen?
    var clone = instance.DeepClone();
    
    // As for now - ChildValue will not be cloned and Parent type will be constructed instead of a Child

    ```

    A reason for the behavior above - is performance. Checking each reference type for real type has performance impact and that's why this is not implemented as for now. Please add your suggestion on how this can be done :) I assume that attribute like `[CloneAtRuntime]` can be added to field/property/class itself...

8. Nullables
    
    Nullables are handled as you might expect - first `HasValue` is checked and then `Value` is cloned:

    ``` csharp

        var nullable = new int?();

        // clone:
        var clone = nullable.HasValue ? new int?(nullable.Value.DeepClone()) : nullable;

    ```

9. Recursive types

    Recursive types is a hard topic. To clone a recursive type some sort of reference tracking is needed which requires additional allocations, so it's not implemented (see [motivation](#1-motivation) section), for example:

    ``` csharp

    public class Node<T> {
        public T Value { get; set; }
        public Node<T> Next { get; set; } 
    }

    // How can we clone such object?
    
    var node = new Node<int> { Value = 1, Next = new Node<int> { Value = 2, Next = null  }};

    // Well, simple
    var clone = new Node<int>();
    clone.Value  = node.Value;
    clone.Next = node.Next.DeepClone(); // recursive cloning

    ```

    Such recursive types are supported by library, you can clone this type without reference tracking because there is no references that are linked to previous nodes, but let's imagine such type:

    ``` csharp

    public class Node<T> {
        public T Value { get; set; }
        public Node<T> Next { get; set; } 
        public Node<T> Previous { get; set; }
    }

    ```

    This time, we need some sort of reference tracking, because if we will try to deep clone `Previous` field the whole cloning process will stuck in an infinite loop, the same happens with `LinkedList<T>` or `Dictionary<TKey, TValue>`:

    ``` csharp

    // Pseudocode - properties may be called differently
    public class LinkedListNode<T> {
        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Previous { get; set; }
        public LinkedList<T> Parent { get; set; }
        public T Value { get; set; }
    }

    public class Dictionary<TKey, TValue>.ValueCollection {
        public Dictionary<TKey, TValue> Dictionary { get; set; }
    }

    public class Dictionary<TKey, TValue> {
        public ValueCollection Values { get; set; }
    }

    ```

    For such types - we are in trouble... We can't clone them without some sort of reference tracking and the more nodes you have - more references you need to track - more allocations you need to make, such a nightmare!

    Don't worry though, library provides cloners for `Dictionary`/`SortedDictionary` and `LinkedList` so such commonly used collections can be cloned, also you can specify your own cloner for recursive type if needed as described in section [extending](#4-extending).

10. Collections
    
    Collections play crucial role in every program and library tries to clone them as fast as possible. For example, `Array` is a core collection for types like `Queue<T>`, `Stack<T>`, `HashSet<T>` so their cloning performance depends of `Array` cloning performance, knowing that we should clone arrays as fast as possible, library supports single/jagged and multi-dimention (up to 4 dimentions) cloning, it also clones array of `SafeToCopy` types without cloning each element but rather copying array content shallowly, for example:

    ``` csharp

    // Array of ints and int is considered as `SafeToCopy` type 
    var ints = new [] { 1, 2, 3, 4, 5 };
    
    // Array of quotes is not considered as `SafeToCopy` type
    var quotes = new [] { new Quote(), new Quote(), new Quote() };

    // We can clone both arrays like this:
    var clone = new Quote[quotes.Length];
    for (var i = 0; i < clone.Length; i++) clone[i] = Cloner.DeepClone(quotes[i]);

    // But we can clone ints array much faster
    var intsClone = new int[ints.Length];
    ints.AsSpan().CopyTo(intsClone); // For `SafeToCopy` types we can use faster implementation and do not call DeepClone

    ```

    `Dictionary<TKey, TValue>`/`SortedDictionary<TKey, TValue>`/`LinkedList<T>` are cloned using their APIs in [CustomTypesCloner](https://github.com/YeSv/Simple.Dotnet.Cloning/blob/main/src/Simple.Dotnet.Cloning/Cloners/CustomTypesCloner.cs)

    You can check benchmark results in the [benchmarks](#5-benchmarks) section.

11. Synchronization primitives/collections

    Synchronization primitives like `Mutex`/`Semaphore`/`SemaphoreSlim` etc are cloned as is (nothing happens basically), how would you clone a `Mutex`? :)
    Collections from `System.Collections.Concurrent` namespace are cloned using their API methods like `.Add`/`.Push`/`Enqueue` etc...

12. Exceptions/Delegates/Streams

    Such types are cloned as is. `Action<T>` is not something we can clone so the same reference is returned because such type is immutable.

13. Immutable collections

    Immutable collections are not cloned using their APIs because methods like `.Add`/`.Push` etc allocate a new one each time so library uses field by field cloning.

14. Tasks and related threading types

    `Timer`s, `Task`s, `Thread`s, `CancellationToken`s and sources are cloned as is (which means that the same instance is returned).

15. Other types not specified here...

    Library can't handle all possible .NET types and add them to for example to `SafeToCopy` collections or provide special-like cloners but you are able to change it by adding types to `SafeToCopy` collection and specifying your custom cloner

# 4. Extending

You can extend cloners collection and `SafeToCopy` collection so types that are missed by default can be cloned efficiently.
Let's say you wrote a class which should not be copied by it's value but rather clone should be the same, you definately don't want to allocate a new instance each time you clone it, you can do something like this:

``` csharp

    // Class that you are sure won't be modified so it can be cloned by reference
    public sealed class CopyByRefClass 
    {
        public int Int { get; set; } 
        public object[] Objects { get; set; }
    }

    // Or even generic one
    public sealed class CopyByRefClassGeneric<T> 
    {
        public T Value { get; set; } 
    }

    // Somewhere in Program.cs or in Startup.cs
    // By adding types to this collection they will be cloned as is - cloning won't generate a new instance but rather existing one will be returned
    CloningTypes.SafeToCopy.Add(typeof(CopyByRefClass));
    CloningTypes.SafeToCopy.Add(typeof(CopyByRefClassGeneric<>)); // You should specify opened generic type here

    // Or you don't want to clone dictionaries
    CloningTypes.SafeToCopy.Add(typeof(Dictionary<,>));

```

Let's say that you have a class that has references to itself, like you wrote your custom linked list implementation or tree implementation where child nodes can refer to parent nodes. Such types are recurring so library can't clone them. Well, you can write your own cloner and specify that you want to use it for type:

``` csharp

    // To clone this type we need some sort of reference tracking...which this library lacks
    public sealed class AvlNode<T> 
    {
        public T Value { get; set; }
        public AvlNode<T>? Parent { get; set; }
        public AvlNode<T>? Left { get; set; }
        public AvlNode<T>? Right { get; set; }
    }

    // We can write our custom cloner
    // NOTE: Cloner type should be static!!! (can be generic or non generic)
    public static class AvlNodeCloner
    {
        // Cache opened cloner method, required to be used by library
        public static readonly MethodInfo Method = typeof(AvlNodeCloner).GetMethod(
            nameof(AvlNodeClone.Clone), 
            BindingFlags.Static | BindingFlags.Public);

        // You might better not use a recursion but rather a loop, but such implementation is shorter
        // I did not debug it and haven't checked that it's working properly :) just for demonstaration purposes
        public static AvlNode<T> Clone<T>(AvlNode<T> node) 
        {
            if (node == null) return null; // It's better to handle nulls :D

            var clone = new AvlNode<T>();
            clone.Parent = node.Parent; // Copy parent
            clone.Value = node.Value.DeepClone(); // Deep clone value

            if (node.Right != null) 
            {
                clone.Right = Clone(node.Right); // Recursively clone right node
                clone.Right.Parent = clone;
            }

            if (node.Left != null) 
            {
                clone.Left = Clone(node.Left); // Recursively clone left node
                clone.Left.Parent = clone;
            }

            return clone;
        }
    }

    // Somewhere in Program.cs:
    // Now we need to register custom cloner
    // Func<Type[], MethodInfo>
    CloningTypes.Custom.Add(typeof(AvlNode<>), t => AvlNodeCloner.Method.MakeGenericMethod(t)); // t is array of types, for example if AvlNode<int>, t is [typeof(int)]
    // In case if class is not generic (if we wrote AvlNode class - non generic) - t in func can be ommited it will contain an empty array,

```

# 5. Benchmarks
    
All benchmarks are available here: [benchmarks](https://github.com/YeSv/Simple.Dotnet.Cloning/tree/main/tests/Simple.Dotnet.Cloning.Benchmarks)
Types that were tested are available here: [types](https://github.com/YeSv/Simple.Dotnet.Cloning/blob/main/tests/Simple.Dotnet.Cloning.Tests.Common/Records.cs)

In short:
1. `HugeStruct` is a struct with 15 fields both of reference and value types
2. `HugeClass` has over 100 fields - collections, other classes, structs etc
3. `SmallStruct` is a struct with 3 value type fields (Guid, int?, long)
4. `HugeValueStruct` has 16 value type fields
5. `SmallClass` has three fields

[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) was used for benchmarking.

[Force](https://github.com/force-net/DeepCloner) was used for both deep and shallow cloning.

[AutoMapper](https://github.com/AutoMapper/AutoMapper) was used for shallow cloning.

[FastDeepCloner](https://github.com/AlenToma/FastDeepCloner) was used for deep cloning.

[NCloner](https://github.com/mijay/NClone) - was used for deep cloning.

[DeepCopy](https://github.com/ReubenBond/DeepCopy) - was used for deep cloning.

Runtime: .NET 5.0

Results:

(+) - winner

(-) - exception

(?) - questionable

1. `HugeStruct` deep clone
```
    |         Method |         Mean |     Error |    StdDev |  Gen 0 | Allocated |
    |--------------- |-------------:|----------:|----------:|-------:|----------:|
    |          Force |    378.53 ns |  7.555 ns | 10.341 ns | 0.0815 |     384 B |
 (+)|         Simple |     80.44 ns |  0.380 ns |  0.317 ns | 0.0373 |     176 B |
    |        NCloner |    175.46 ns |  1.063 ns |  0.887 ns | 0.0849 |     400 B |
    | FastDeepCloner | 15,679.01 ns | 82.525 ns | 73.156 ns | 1.0071 |   4,840 B |
    |       DeepCopy |    252.62 ns |  1.347 ns |  1.260 ns | 0.0372 |     176 B |
```
2. `HugeStruct` collections deep clone (Size - collection's length and string lengths if any)
```
    |                    Method | Size |         Mean |       Error |      StdDev |  Gen 0 |  Gen 1 | Allocated |
    |-------------------------- |----- |-------------:|------------:|------------:|-------:|-------:|----------:|
    |          Dictionary_Force |   10 |   6,439.3 ns |    60.85 ns |    50.81 ns | 1.8463 | 0.0687 |      8 KB |
 (+)|         Dictionary_Simple |   10 |   2,401.0 ns |    48.08 ns |    49.37 ns | 0.7668 | 0.0191 |      4 KB |
    | Dictionary_FastDeepCloner |   10 |   2,587.8 ns |    12.80 ns |    11.35 ns | 1.4687 | 0.0305 |      7 KB |
 (-)|        Dictionary_NCloner |   10 | 330,229.5 ns | 6,494.52 ns | 8,670.00 ns | 0.9766 |      - |      5 KB |
    | Dictionary_DeepCopyCloner |   10 |   9,544.9 ns |    45.38 ns |    40.23 ns | 2.7924 | 0.1068 |     13 KB |

    |               Array_Force |   10 |   3,685.8 ns |    22.26 ns |    18.59 ns | 1.4191 | 0.0381 |      7 KB |
    |              Array_Simple |   10 |     953.7 ns |     6.10 ns |     5.09 ns | 0.6676 | 0.0134 |      3 KB |
    |      Array_FastDeepCloner |   10 |   5,882.1 ns |    93.50 ns |    87.46 ns | 1.1444 |      - |      5 KB |
 (+)|             Array_NCloner |   10 |     261.8 ns |     1.90 ns |     1.78 ns | 0.3467 | 0.0038 |      2 KB |
    |      Array_DeepCopyCloner |   10 |   3,919.6 ns |    30.29 ns |    26.85 ns | 1.6327 | 0.0381 |      8 KB |

    |             HashSet_Force |   10 |   5,915.4 ns |    35.89 ns |    31.81 ns | 1.7776 | 0.0610 |      8 KB |
    |            HashSet_Simple |   10 |   2,299.8 ns |     9.10 ns |     8.07 ns | 0.9346 | 0.0267 |      4 KB |
 (?)|    HashSet_FastDeepCloner |   10 |   1,535.2 ns |    27.87 ns |    28.62 ns | 0.2384 |      - |      1 KB |
 (+)|           HashSet_NCloner |   10 |   1,513.0 ns |    30.09 ns |    28.14 ns | 0.9041 | 0.0191 |      4 KB |
    |    HashSet_DeepCopyCloner |   10 |   9,166.2 ns |    72.42 ns |    67.74 ns | 2.6550 | 0.0916 |     12 KB |

    |                List_Force |   10 |   3,899.2 ns |    24.46 ns |    21.69 ns | 1.4267 | 0.0381 |      7 KB |
    |               List_Simple |   10 |     992.3 ns |    19.83 ns |    36.26 ns | 0.6733 | 0.0153 |      3 KB |
    |       List_FastDeepCloner |   10 |   4,858.3 ns |    21.52 ns |    20.13 ns | 1.7166 | 0.0305 |      8 KB |
 (+)|              List_NCloner |   10 |     747.2 ns |     6.87 ns |     5.74 ns | 0.4549 | 0.0048 |      2 KB |
    |       List_DeepCopyCloner |   10 |   3,971.0 ns |    37.21 ns |    32.98 ns | 1.6403 | 0.0458 |      8 KB |
```
* Actually interesting how `NClone` provided both fastest result and least allocations
* (?) - FastDeepCloner only allocated 1KB - something is really strange there...

3. `HugeClass` deep clone (Length - inner collections lengths and string lengths if any)
```
    |         Method | Lengths |          Mean |        Error |       StdDev |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
    |--------------- |-------- |--------------:|-------------:|-------------:|----------:|----------:|---------:|----------:|
    |          Force |      10 |     247.96 us |     4.822 us |     5.922 us |   60.5469 |   25.3906 |        - |    299 KB |
 (+)|         Simple |      10 |      52.52 us |     1.048 us |     1.862 us |   24.7803 |    8.2397 |        - |    123 KB |
 (-)|        NCloner |      10 |     332.91 us |     6.381 us |     7.349 us |   11.2305 |    0.4883 |        - |     52 KB |
    | FastDeepCloner |      10 |   3,643.53 us |    10.027 us |     9.379 us |  183.5938 |   54.6875 |        - |    853 KB |
    |       DeepCopy |      10 |     154.62 us |     0.632 us |     0.561 us |   26.3672 |    8.7891 |        - |    127 KB |

    |          Force |     100 |  57,732.51 us | 1,134.273 us | 1,626.741 us | 2444.4444 | 1000.0000 | 333.3333 | 23,479 KB |
 (+)|         Simple |     100 |  16,966.50 us |   335.886 us |   561.190 us | 1687.5000 |  687.5000 | 187.5000 |  9,224 KB |
 (-)|        NCloner |     100 |   1,089.96 us |    11.565 us |    10.818 us |   58.5938 |   23.4375 |   1.9531 |    448 KB |
    | FastDeepCloner |     100 | 249,228.70 us | 1,631.372 us | 1,446.168 us | 8333.3333 | 1333.3333 |        - | 51,708 KB |
    |       DeepCopy |     100 |  28,924.61 us |   556.715 us |   641.114 us | 1687.5000 |  687.5000 | 187.5000 |  9,268 KB |
```
* `NCloner` throws an exception when dictionary is used (it finds recursive reference)
* `DeepCopy` actually achieved very good results

3. `HugeClass` collections deep clone (Size - collections and inner collections lengths and string lengths if any)
```
    |                    Method | Size |          Mean |       Error |      StdDev |     Gen 0 |    Gen 1 |    Gen 2 | Allocated |
    |-------------------------- |----- |--------------:|------------:|------------:|----------:|---------:|---------:|----------:|
    |          Dictionary_Force |   10 |  7,043.774 us | 136.5682 us | 204.4089 us |  453.1250 | 281.2500 | 125.0000 |  2,893 KB |
 (+)|         Dictionary_Simple |   10 |    749.908 us |   5.1918 us |   4.8564 us |  200.1953 |  99.6094 |        - |  1,230 KB |
    | Dictionary_FastDeepCloner |   10 | 38,521.535 us | 204.0324 us | 180.8694 us | 1384.6154 | 230.7692 |        - |  8,523 KB |
 (-)|        Dictionary_NCloner |   10 |    320.590 us |   6.0813 us |   7.4683 us |    0.4883 |        - |        - |      3 KB |
    | Dictionary_DeepCopyCloner |   10 |  2,013.803 us |  13.9209 us |  12.3405 us |  207.0313 | 101.5625 |        - |  1,270 KB |

    |               Array_Force |   10 |  6,918.871 us | 131.0067 us | 140.1757 us |  468.7500 | 304.6875 | 140.6250 |  2,892 KB |
 (+)|              Array_Simple |   10 |    750.457 us |   4.0980 us |   3.6327 us |  200.1953 |  99.6094 |        - |  1,230 KB |
    |      Array_FastDeepCloner |   10 | 40,475.264 us | 285.4302 us | 222.8451 us | 1384.6154 | 230.7692 |        - |  8,524 KB |
 (-)|             Array_NCloner |   10 |    317.698 us |   6.1435 us |   7.5448 us |   11.2305 |   0.4883 |        - |     53 KB |
    |      Array_DeepCopyCloner |   10 |  2,009.401 us |  26.7981 us |  25.0670 us |  203.1250 | 101.5625 |        - |  1,267 KB |

    |             HashSet_Force |   10 |  6,956.630 us | 135.9055 us | 251.9096 us |  429.6875 | 250.0000 | 101.5625 |  2,891 KB |
 (+)|            HashSet_Simple |   10 |    758.049 us |   4.5134 us |   4.2218 us |  200.1953 |  99.6094 |        - |  1,230 KB |
    |    HashSet_FastDeepCloner |   10 |      1.586 us |   0.0148 us |   0.0139 us |    0.2384 |        - |        - |      1 KB |
 (-)|           HashSet_NCloner |   10 |      1.364 us |   0.0156 us |   0.0145 us |    0.4406 |   0.0019 |        - |      2 KB |
    |    HashSet_DeepCopyCloner |   10 |  1,960.830 us |  24.4612 us |  22.8810 us |  207.0313 | 103.5156 |        - |  1,269 KB |

    |                List_Force |   10 |  7,106.354 us | 140.7465 us | 274.5153 us |  453.1250 | 281.2500 | 125.0000 |  2,893 KB |
 (+)|               List_Simple |   10 |    752.582 us |   2.0828 us |   1.7393 us |  200.1953 |  99.6094 |        - |  1,230 KB |
    |       List_FastDeepCloner |   10 | 38,590.474 us | 120.8423 us | 113.0359 us | 1384.6154 | 230.7692 |        - |  8,524 KB |
 (-)|              List_NCloner |   10 |    331.219 us |   6.4581 us |   7.1782 us |   11.2305 |   0.4883 |        - |     53 KB |
    |       List_DeepCopyCloner |   10 |  1,997.253 us |  15.9839 us |  14.9513 us |  203.1250 | 101.5625 |        - |  1,267 KB |
```
* `NCloner` throws an exception when dictionary is used (it finds recursive reference)
* `DeepCopy` actually achieved very good results

4. `SmallClass` deep clone
```
    |         Method |         Mean |     Error |    StdDev |  Gen 0 | Allocated |
    |--------------- |-------------:|----------:|----------:|-------:|----------:|
    |          Force |    72.811 ns | 0.3304 ns | 0.3090 ns | 0.0391 |     184 B |
 (+)|         Simple |     4.307 ns | 0.0266 ns | 0.0236 ns | 0.0102 |      48 B |
    |        NCloner |   450.783 ns | 1.0098 ns | 0.9445 ns | 0.1135 |     536 B |
    | FastDeepCloner | 1,875.743 ns | 5.8108 ns | 5.4354 ns | 0.2785 |   1,312 B |
    |       DeepCopy |    24.448 ns | 0.0712 ns | 0.0666 ns | 0.0102 |      48 B |
```

5. `SmallClass` collections deep clone (Size - collection's size)
```
    |                    Method | Size |         Mean |       Error |      StdDev |       Median |  Gen 0 |  Gen 1 | Allocated |
    |-------------------------- |----- |-------------:|------------:|------------:|-------------:|-------:|-------:|----------:|
    |          Dictionary_Force |   10 |   1,890.4 ns |    10.09 ns |     7.88 ns |   1,892.5 ns | 0.6332 |      - |   2,992 B |
 (+)|         Dictionary_Simple |   10 |     456.3 ns |     1.80 ns |     1.68 ns |     456.8 ns | 0.1955 |      - |     920 B |
    | Dictionary_FastDeepCloner |   10 |  19,690.8 ns |    84.74 ns |    79.26 ns |  19,702.7 ns | 1.3123 |      - |   6,184 B |
 (-)|        Dictionary_NCloner |   10 | 324,061.7 ns | 5,833.81 ns | 6,242.11 ns | 325,325.3 ns | 0.4883 |      - |   3,409 B |
    | Dictionary_DeepCopyCloner |   10 |   2,263.9 ns |    10.24 ns |     9.58 ns |   2,265.2 ns | 0.6714 |      - |   3,176 B |

    |               Array_Force |   10 |     933.7 ns |     2.49 ns |     2.32 ns |     932.7 ns | 0.3290 |      - |   1,552 B |
 (+)|              Array_Simple |   10 |     122.1 ns |     0.72 ns |     0.64 ns |     122.1 ns | 0.1240 |      - |     584 B |
    |      Array_FastDeepCloner |   10 |  23,668.8 ns |    87.65 ns |    81.99 ns |  23,670.1 ns | 1.4343 |      - |   6,856 B |
    |             Array_NCloner |   10 |   4,656.6 ns |    29.25 ns |    25.93 ns |   4,655.2 ns | 1.0910 | 0.0076 |   5,136 B |
    |      Array_DeepCopyCloner |   10 |     442.1 ns |     3.69 ns |     3.08 ns |     440.5 ns | 0.1240 |      - |     584 B |

    |             HashSet_Force |   10 |   1,258.2 ns |     6.15 ns |     5.75 ns |   1,259.2 ns | 0.4215 |      - |   1,984 B |
 (+)|            HashSet_Simple |   10 |     287.9 ns |     1.46 ns |     1.22 ns |     287.7 ns | 0.2003 |      - |     944 B |
    |    HashSet_FastDeepCloner |   10 |   1,553.4 ns |    31.10 ns |    53.65 ns |   1,524.5 ns | 0.2384 |      - |   1,128 B |
    |           HashSet_NCloner |   10 |   1,377.8 ns |    27.55 ns |    31.73 ns |   1,377.7 ns | 0.4406 |      - |   2,080 B |
    |    HashSet_DeepCopyCloner |   10 |   1,840.4 ns |    15.52 ns |    12.96 ns |   1,832.8 ns | 0.5474 | 0.0038 |   2,576 B |

    |                List_Force |   10 |   1,018.0 ns |    12.15 ns |    10.14 ns |   1,014.3 ns | 0.3414 |      - |   1,608 B |
 (+)|               List_Simple |   10 |     131.0 ns |     1.20 ns |     1.07 ns |     130.7 ns | 0.1309 | 0.0005 |     616 B |
    |       List_FastDeepCloner |   10 |  22,952.4 ns |   307.07 ns |   287.24 ns |  22,785.5 ns | 1.4954 |      - |   7,088 B |
    |              List_NCloner |   10 |   5,230.5 ns |    35.24 ns |    32.96 ns |   5,223.9 ns | 1.1444 |      - |   5,400 B |
    |       List_DeepCopyCloner |   10 |     482.9 ns |     1.85 ns |     1.73 ns |     482.8 ns | 0.1307 |      - |     616 B |
```
* `DeepCopy` is really fast actually
* `Force` achieves nice performance when working with dictionaries

6. `HugeValueStruct` deep clone
```
    |         Method |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
    |--------------- |------------:|----------:|----------:|-------:|----------:|
    |          Force |   310.50 ns |  4.477 ns |  3.968 ns | 0.0544 |     256 B |
 (+)|         Simple |    68.69 ns |  0.315 ns |  0.263 ns | 0.0085 |      40 B |
    |        NCloner |   186.23 ns |  0.715 ns |  0.669 ns | 0.0865 |     408 B |
    | FastDeepCloner | 7,048.90 ns | 34.966 ns | 32.707 ns | 0.6332 |   2,992 B |
    |       DeepCopy |   273.99 ns |  1.571 ns |  1.469 ns | 0.0081 |      40 B |  
```
* You see allocated here because one of fields is `BigInteger` which contains array

7. `SmallStruct` deep clone (test that cloner does not allocate anything when structs without reference types or structs that contain references are cloned)
```
    |         Method |       Mean |      Error |     StdDev |  Gen 0 | Allocated |
    |--------------- |-----------:|-----------:|-----------:|-------:|----------:|
    |          Force |  19.407 ns |  0.0639 ns |  0.0597 ns |      - |         - |
 (+)|         Simple |   3.524 ns |  0.0188 ns |  0.0176 ns |      - |         - |
    |        NCloner | 132.989 ns |  2.3786 ns |  3.5601 ns | 0.0627 |     296 B |
    | FastDeepCloner | 983.546 ns | 15.7042 ns | 14.6897 ns | 0.2441 |   1,152 B |
    |       DeepCopy |  18.340 ns |  0.0606 ns |  0.0567 ns |      - |         - |
```
8. `HugeClass` shallow clone (I guess AutoMapper just returns the same instance, still `Simple` is faster)
```
    |     Method | Lengths |     Mean |    Error |   StdDev |  Gen 0 |  Gen 1 | Allocated |
    |----------- |-------- |---------:|---------:|---------:|-------:|-------:|----------:|
 (?)| AutoMapper |      10 | 94.16 ns | 0.641 ns | 0.568 ns |      - |      - |         - |
    |      Force |      10 | 90.93 ns | 1.085 ns | 0.962 ns | 0.1564 |      - |     736 B |
 (+)|     Simple |      10 | 68.35 ns | 1.269 ns | 1.187 ns | 0.1564 | 0.0004 |     736 B |

 (?)| AutoMapper |     100 | 99.55 ns | 0.399 ns | 0.333 ns |      - |      - |         - |
    |      Force |     100 | 94.41 ns | 1.626 ns | 1.521 ns | 0.1564 | 0.0004 |     736 B |
 (+)|     Simple |     100 | 68.39 ns | 0.916 ns | 0.857 ns | 0.1564 | 0.0004 |     736 B |
```
9. `SmallClass` shallow clone
```
    |     Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
    |----------- |----------:|----------:|----------:|-------:|----------:|
    | AutoMapper | 92.138 ns | 0.8030 ns | 0.7119 ns |      - |         - |
    |      Force | 47.838 ns | 0.6368 ns | 0.5957 ns | 0.0102 |      48 B |
 (+)|     Simple |  4.795 ns | 0.0997 ns | 0.0884 ns | 0.0102 |      48 B |
```

# 6. Considerations

It's better to use [DeepCloner](https://github.com/force-net/DeepCloner) or any other cloner you prefer when:

1. You don't care about performance
2. You use recursive types and don't want to write cloners for those types
3. Some of types you clone are non-abstract and casted to base class (described in this [section](#3-how-it-works))
4. You don't care about additional allocations
5. You need reference tracking, for example cloning an array with the same objects will produce another array with different cloned objects, but you need another array with new objects with the same references

# 7. Suggestions

Please feel free to write comments/suggestions about implementation and possible improvements or desirable features. Enjoy :)