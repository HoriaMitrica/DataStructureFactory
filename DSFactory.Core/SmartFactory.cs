using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DSFactory.Core
{
    public static class SmartFactory
    {
        public static IDataStructure<T> CreateSmart<T>(IEnumerable<T> rawData) where T : IComparable<T>
        {
            var list = rawData.ToList();
            if (list.Count == 0) return new CustomStack<T>();

            if (typeof(T) == typeof(string))
            {
                int edgeCount = 0;
                var edgePattern = new Regex(@"^(.+)(->|-|>)(.+)$");

                foreach (var item in list)
                {
                    if (edgePattern.IsMatch(item.ToString()!)) edgeCount++;
                }

                if (edgeCount > list.Count / 2)
                {
                    var graph = new DirectedGraph<T>(); 
                    
                    foreach (var item in list)
                    {
                        var match = edgePattern.Match(item.ToString()!);
                        if (match.Success)
                        {
                            T from = (T)(object)match.Groups[1].Value.Trim();
                            T to = (T)(object)match.Groups[3].Value.Trim();
                            graph.AddEdge(from, to);
                        }
                        else
                        {
                            graph.Add(item); 
                        }
                    }
                    return graph;
                }
            }

            bool isSortedAsc = true;
            bool isSortedDesc = true;

            for (int i = 0; i < list.Count - 1; i++)
            {
                int cmp = list[i].CompareTo(list[i + 1]);
                if (cmp > 0) isSortedAsc = false;
                if (cmp < 0) isSortedDesc = false;
            }

            if (isSortedAsc || isSortedDesc)
            {
                var bst = new BinarySearchTree<T>();

                list.Sort(); 
                AddBalanced(bst, list, 0, list.Count - 1);
                
                return bst;
            }

            var dll = new DoublyLinkedList<T>();
            foreach (var item in list) dll.Add(item);
            return dll;
        }

        private static void AddBalanced<T>(BinarySearchTree<T> bst, List<T> sortedData, int min, int max) 
            where T : IComparable<T>
        {
            if (min > max) return;

            int mid = (min + max) / 2;
            bst.Add(sortedData[mid]);

            AddBalanced(bst, sortedData, min, mid - 1);
            AddBalanced(bst, sortedData, mid + 1, max); 
        }
    }
}