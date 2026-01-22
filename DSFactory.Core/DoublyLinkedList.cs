using System;
using System.Collections;

namespace DSFactory.Core
{
    public class DoublyLinkedList<T> : IDataStructure<T>
    {
        private Node<T>? _head;
        private Node<T>? _tail;
        private int _count;
        public StructureType StructureType => StructureType.DoublyLinkedList;

        public IEnumerator<T> GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public string TypeName => "Doubly Linked List";
        public int Count => _count;

        public void Add(T item)
        {
            var newNode = new Node<T>(item);

            if (_head == null)
            {
                _head = newNode;
                _tail = newNode;
            }
            else
            {
                _tail!.Next = newNode; 
                newNode.Prev = _tail;
                _tail = newNode;
            }
            _count++;
        }

        public T Remove()
        {
            if (_head == null) throw new InvalidOperationException("List is empty");

            T data = _head.Data;

            _head = _head.Next;

            if (_head != null)
            {
                _head.Prev = null;
            }
            else
            {
                _tail = null;
            }

            _count--;
            return data;
        }

        public T Peek()
        {
             if (_head == null) throw new InvalidOperationException("List is empty");
             return _head.Data;
        }

        public string Display()
        {
            if (_head == null) return "null <-> null";

            var sb = new System.Text.StringBuilder();
            sb.Append("null <-> ");
            
            var current = _head;
            while (current != null)
            {
                sb.Append($"[{current.Data}] <-> ");
                current = current.Next;
            }
            sb.Append("null");
            return sb.ToString();
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public bool Contains(T item)
        {
            var current = _head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Data, item))
                    return true;
                current = current.Next;
            }
            return false;
        }
    }
}