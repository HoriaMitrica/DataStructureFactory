using System;

namespace DSFactory.Core
{
    public static class StructureFactory
    {
        public static IDataStructure<T> Create<T>(StructureType type) where T : IComparable<T>
        {
            return type switch
            {
                StructureType.Stack => new CustomStack<T>(),
                StructureType.Queue => new CustomQueue<T>(),
                StructureType.DoublyLinkedList => new DoublyLinkedList<T>(),
                StructureType.BinarySearchTree => new BinarySearchTree<T>(),
                _ => throw new ArgumentException("Invalid structure type selected.")
            }; 
        }
    }
}