using System;
using System.Collections;
using System.Collections.Generic;

namespace DSFactory.Core
{
    public class BinarySearchTree<T> : IDataStructure<T> where T : IComparable<T>
    {
        private TreeNode<T>? _root;
        private int _count;

        public string TypeName => "Binary Search Tree";
        public StructureType StructureType => StructureType.BinarySearchTree;
        public int Count => _count;

        public void Add(T item)
        {
            _root = AddRecursive(_root, item);
            _count++;
        }

        private TreeNode<T> AddRecursive(TreeNode<T>? node, T item)
        {
            if (node == null) return new TreeNode<T>(item);

            int comparison = item.CompareTo(node.Data);

            if (comparison < 0)
                node.Left = AddRecursive(node.Left, item);
            else
                node.Right = AddRecursive(node.Right, item);

            return node;
        }

        public bool Contains(T item)
        {
            var current = _root;
            while (current != null)
            {
                int comparison = item.CompareTo(current.Data);
                if (comparison == 0) return true;
                current = comparison < 0 ? current.Left : current.Right;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(_root).GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal(TreeNode<T>? node)
        {
            if (node != null)
            {
                foreach (var item in InOrderTraversal(node.Left)) yield return item;
                yield return node.Data;
                foreach (var item in InOrderTraversal(node.Right)) yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T Remove()
        {
            if (_root == null) throw new InvalidOperationException("Empty Tree");
            T data = _root.Data;
            if (_count == 1) { _root = null; _count = 0; return data; }

            throw new NotImplementedException("Full BST Deletion is a separate lecture!");
        }

        public T Peek() => _root != null ? _root.Data : throw new InvalidOperationException("Empty");
        public void Clear() { _root = null; _count = 0; }

        public string Display()
        {
            if (_root == null) return "(Empty Tree)";
            return DisplayRecursive(_root);
        }

        private string DisplayRecursive(TreeNode<T>? node, string prefix = "", bool isTail = true)
        {
            if (node == null) return "";

            var sb = new System.Text.StringBuilder();

            if (node.Right != null)
            {
                string rightPrefix = prefix + (isTail ? "      " : "│     ");
                sb.Append(DisplayRecursive(node.Right, rightPrefix, false));
            }

            sb.Append(prefix);

            if (prefix == "")
                sb.Append("");
            else
                sb.Append(isTail ? "└── " : "┌── ");

            sb.AppendLine($"[{node.Data}]");

            if (node.Left != null)
            {
                string leftPrefix = prefix + (isTail ? "      " : "│     ");
                sb.Append(DisplayRecursive(node.Left, leftPrefix, true));
            }

            return sb.ToString();
        }
    }
}