using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSFactory.Core
{
    public class DirectedGraph<T> : IDataStructure<T> where T : notnull, IComparable<T>
    {
        private readonly Dictionary<T, List<T>> _adjacencyList = new();

        public string TypeName => "Directed Graph";
        public StructureType StructureType => StructureType.Graph; 
        public int Count => _adjacencyList.Count;

        public void Add(T item)
        {
            if (!_adjacencyList.ContainsKey(item))
            {
                _adjacencyList[item] = new List<T>();
            }
        }

        public void AddEdge(T from, T to)
        {
            Add(from);
            Add(to);

            if (!_adjacencyList[from].Contains(to))
            {
                _adjacencyList[from].Add(to);
            }
        }

        public T Remove()
        {
            if (_adjacencyList.Count == 0) throw new InvalidOperationException("Empty Graph");

            T nodeToRemove = _adjacencyList.Keys.Last();

            _adjacencyList.Remove(nodeToRemove);

            foreach (var neighbors in _adjacencyList.Values)
            {
                neighbors.Remove(nodeToRemove);
            }

            return nodeToRemove;
        }

        public T Peek() => _adjacencyList.Keys.LastOrDefault(); // Interface requirement
        public bool Contains(T item) => _adjacencyList.ContainsKey(item);
        public void Clear() => _adjacencyList.Clear();

        public string Display()
        {
            if (_adjacencyList.Count == 0) return "(Empty Graph)";

            var sb = new StringBuilder();
            foreach (var kvp in _adjacencyList)
            {
                sb.Append($"({kvp.Key})");

                if (kvp.Value.Count > 0)
                {
                    sb.Append(" ──> [");
                    sb.Append(string.Join("], [", kvp.Value));
                    sb.Append("]");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator() => _adjacencyList.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}