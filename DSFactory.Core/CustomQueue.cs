using System;
using System.Collections.Generic;

namespace DSFactory.Core
{
    
    public class CustomQueue<T> : IDataStructure<T>
    {
        private readonly List<T> _items = new List<T>();

        public string TypeName => "Queue";
        public int Count => _items.Count;

        public void Add(T item) => _items.Add(item); 

        public T Remove() 
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            var item = _items[0];
            _items.RemoveAt(0);
            return item;
        }

        public T Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return _items[0];
        }

        public string Display()
        {
            if (_items.Count == 0) return "(Empty Queue)";
            return "OUT [ " + string.Join(" ] <- [ ", _items) + " ] IN";
        }
        public bool Contains(T item) => _items.Contains(item);

        public void Clear() => _items.Clear();
    }
}