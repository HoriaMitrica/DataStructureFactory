using System;
using System.Collections;
using System.Collections.Generic;

namespace DSFactory.Core
{
    public class CustomStack<T> : IDataStructure<T>
    {

        private readonly List<T> _items = new List<T>();
        public StructureType StructureType => StructureType.Stack;

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string TypeName => "Stack";
        public int Count => _items.Count;

        public void Add(T item) => _items.Add(item);

        public T Remove()
        {
            if (Count == 0) throw new InvalidOperationException("Stack is empty.");
            var item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Stack is empty.");
            return _items[_items.Count - 1];
        }

        public string Display()
        {
            if (_items.Count == 0) return "[Empty Stack]";
            
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("--- TOP ---");
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                sb.AppendLine($"|  {_items[i]}  |");
            }
            sb.AppendLine("-----------");
            return sb.ToString();
        }

        public bool Contains(T item) => _items.Contains(item);

        public void Clear() => _items.Clear();
    }
   
}