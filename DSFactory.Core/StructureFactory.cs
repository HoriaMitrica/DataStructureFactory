using System;

namespace DSFactory.Core
{
    public static class StructureFactory
    {
        public static IDataStructure<T> Create<T>(StructureType type)
        {
            return type switch
            {
                StructureType.Stack => new CustomStack<T>(),
                StructureType.Queue => new CustomQueue<T>(),
                StructureType.DoublyLinkedList => new DoublyLinkedList<T>(),
                _ => throw new ArgumentException("Invalid structure type selected.")
            };
        }
    }
}