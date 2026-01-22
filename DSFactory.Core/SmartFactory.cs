using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFactory.Core
{
    public static class SmartFactory
    {
        public static IDataStructure<T> SmartCreate<T>(IEnumerable<T> rawData) where T : IComparable<T>
        {
            var dataList = rawData.ToList();
            if (dataList.Count == 0) return new CustomStack<T>();

            bool isSorted = true;
            for (int i = 0; i < dataList.Count - 1; i++)
            {
                if (dataList[i].CompareTo(dataList[i+1]) > 0)
                {
                    isSorted = false;
                    break;
                }
            }

            IDataStructure<T> structure;

            if (isSorted)
            {
                structure = new BinarySearchTree<T>(); 
            }
            else
            {
                structure = new DoublyLinkedList<T>();
            }

            foreach (var item in dataList)
            {
                structure.Add(item);
            }

            return structure;
        }
    }
}