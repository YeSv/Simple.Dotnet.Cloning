using System.Collections.Generic;

namespace Simple.Dotnet.Cloning.Cloners
{
    internal static class RecurringTypesCloner
    {
        // TODO: How to handle such types without specifiing them here?)
        public static LinkedList<T> CloneLinkedList<T>(LinkedList<T> linkedList)
        {
            if (linkedList == null) return null;
            if (linkedList.Count == 0) return new();

            var clone = new LinkedList<T>();
            foreach (var element in linkedList) clone.AddLast(RootCloner<T>.DeepClone(element));

            return clone;
        }
    }
}
